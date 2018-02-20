using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains and handles access to map height data and related metadata (stored in a MapMetadata object)
/// </summary>

public class MapData {
    protected float[,] data;
    private float scale;
    protected MapMetadata metadata;
    protected CoordinateConverter converter;

    public MapData(MapData mapData) : this(mapData.data, mapData.metadata) {
    }

    public MapData(int width, int height, MapMetadata metadata) : this(new float[width, height], metadata) {
    }

    public MapData(float[,] data, MapMetadata metadata) {
        this.data = data;
        scale = 1 / (float)Mathf.Max(data.GetLength(0), data.GetLength(1));
        this.metadata = metadata;
        this.converter = new CoordinateConverter(this.metadata.cellsize);
    }

    public static MapData ForTesting(float[,] data) {
        MapMetadata metadata = new MapMetadata();
        metadata.Set(MapMetadata.minheightKey, data.Cast<float>().Min().ToString());
        metadata.Set(MapMetadata.maxheightKey, data.Cast<float>().Max().ToString());
        metadata.Set(MapMetadata.cellsizeKey, "1");
        return new MapData(data, metadata);
    }

    public void Set(int x, int y, float h) {
        data[x, y] = h;
    }

    public virtual int GetWidth() {
        return data.GetLength(0);
    }

    public virtual int GetHeight() {
        return data.GetLength(1);
    }

    public float GetScale() {
        return scale;
    }

    public virtual Vector2 GetTopLeft() {
        return new Vector2((GetWidth() - 1) / -2f, (GetHeight() - 1) / 2f);
    }

    /// <summary>
    /// Returns the MapPoint(lon,lat) of the center of the top left cell of this map.
    /// </summary>
    public MapPoint GetTopLeftLatLonPoint() {
        Vector2 topLeftVector = this.GetTopLeft();
        double centerXRelativeToLowerLeftCorner = data.GetLength(0) / 2;
        double centerYRelativeToLowerLeftCorner = data.GetLength(1) / 2;
        double topLeftLon = converter.TransformCoordinateByDistance(centerXRelativeToLowerLeftCorner + topLeftVector.x, this.metadata.xllcorner);
        double topLeftLat = converter.TransformCoordinateByDistance(centerYRelativeToLowerLeftCorner + topLeftVector.y, this.metadata.yllcorner);
        return new MapPoint(topLeftLon, topLeftLat);
    }

    /// <summary>
    /// Returns the MapPoint(x,y) of the center of the top left cell of this map in 
    /// WebMercator.
    /// </summary>
    public MapPoint GetTopLeftAsWebMercator() {
        MapPoint topLeftCorner = GetTopLeftLatLonPoint();
        return converter.ProjectPointToWebMercator(topLeftCorner);
    }

    /// <summary>
    /// Returns the MapPoint(lon,lat) of the center of the cell that is at position x,y
    /// elative to the center of the top left cell of the map. 
    /// </summary>
    public MapPoint GetLatLonCoordinates(Vector2 positionOnMap) {
        if (positionOnMap.x < -0.5 || positionOnMap.x > this.GetWidth() - 0.5
            || positionOnMap.y > 0.5 || positionOnMap.y < -this.GetHeight() + 0.5) {
            throw new System.ArgumentException("Index out of bounds! (" + positionOnMap.x + ", " + positionOnMap.y + ")");
        }
        MapPoint topLeft = this.GetTopLeftLatLonPoint();
        double x = converter.TransformCoordinateByDistance((double)positionOnMap.x, (double)topLeft.x);
        double y = converter.TransformCoordinateByDistance((double)positionOnMap.y, (double)topLeft.y);
        return new MapPoint(x, y);
    }
    public MapPoint GetWebMercatorCoordinates(Vector2 positionOnMap) {
        MapPoint latLonPoint = this.GetLatLonCoordinates(positionOnMap);
        return converter.ProjectPointToWebMercator(latLonPoint);
    }

    // NOT WORKING PROPERLY FOR SLICES.
    public virtual Vector2 GetMapSpecificCoordinatesFromLatLon(MapPoint latLonPoint) {
        float maxXDistance = (float)converter.TransformCoordinateByDistance(0, (this.GetWidth() / 2.0));
        float maxYDistance = (float)converter.TransformCoordinateByDistance(0, (this.GetHeight() / 2.0));
        if (Math.Abs(latLonPoint.x) > maxXDistance | Math.Abs(latLonPoint.y) > maxYDistance) {
            throw new System.ArgumentException("Index out of bounds! (" + latLonPoint.x + ", " + latLonPoint.y + ")");
        }

        MapPoint sliceTopLeft = this.GetTopLeftLatLonPoint();
        double sliceCenterLon = converter.TransformCoordinateByDistance(((this.GetWidth() - 1) / 2.0), sliceTopLeft.x);
        double sliceCenterLat = converter.TransformCoordinateByDistance(-((this.GetHeight() - 1) / 2.0), sliceTopLeft.y);
        
        float xVectorFromCenter = converter.DistanceBetweenCoordinates(sliceCenterLon, latLonPoint.x);
        float yVectorFromCenter = converter.DistanceBetweenCoordinates(sliceCenterLat, latLonPoint.y);
        return new Vector2(xVectorFromCenter, yVectorFromCenter);

        //double centerLonWholeMap = converter.TransformCoordinateByDistance((data.GetLength(0) / 2.0), metadata.xllcorner);
        //double centerLatWholeMap = converter.TransformCoordinateByDistance((data.GetLength(1) / 2.0), metadata.yllcorner);
        //double centerLonSlice = converter.TransformCoordinateByDistance((data.GetLength(0) / 2.0), metadata.xllcorner);
        //double centerLatSlice = converter.TransformCoordinateByDistance((data.GetLength(1) / 2.0), metadata.yllcorner);
    }

    public virtual float GetRaw(int x, int y) {
        return data[x, y];
    }

    public float GetHeightMultiplier() {
        return (1 / metadata.cellsize) * scale;
    }

    public float GetNormalized(int x, int y) {
        return (GetRaw(x, y) - metadata.minheight) * GetHeightMultiplier();
    }

    public float GetSquished(int x, int y) {
        return (GetRaw(x, y) - metadata.minheight) / (metadata.maxheight - metadata.minheight);
    }

    public MapDataSlice AsSlice() {
        return new MapDataSlice(this, 0, 0, GetWidth(), GetHeight());
    }

    public List<MapDataSlice> GetSlices(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int sliceWidth, int sliceHeight, bool doOffset = true) {
        if (sliceHeight <= (doOffset ? 1 : 0) || sliceWidth <= (doOffset ? 1 : 0)) {
            throw new System.ArgumentException("Too small slice width (" + sliceWidth + ") or height (" + sliceHeight + ")");
        }
        List<MapDataSlice> slices = new List<MapDataSlice>();
        for (int y = topLeftY; y < bottomRightY; y += sliceHeight - (doOffset ? 1 : 0)) {
            for (int x = topLeftX; x < bottomRightX; x += sliceWidth - (doOffset ? 1 : 0)) {
                slices.Add(new MapDataSlice(this, x, y, sliceWidth, sliceHeight));
            }
        }
        return slices;
    }

    public List<MapData> GetSlices(int sliceSize) {
        return GetSlices(0, 0, GetWidth(), GetHeight(), sliceSize, sliceSize).ConvertAll(s => (MapData)s);
    }

    public List<DisplayReadySlice> GetDisplayReadySlices(int sliceSize, int lod) {
        List<MapDataSlice> slices = GetSlices(sliceSize).ConvertAll(s => (MapDataSlice)s); // TODO: Get rid of these double conversions
        List<DisplayReadySlice> displayReadies = slices.ConvertAll(s => s.AsDisplayReadySlice(lod));
        return displayReadies;
    }
}