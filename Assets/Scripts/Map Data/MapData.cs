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
    public MapMetadata metadata;
    protected CoordinateConverter converter;

    public MapData(MapData mapData) : this(mapData.data, mapData.metadata) {
    }

    public MapData(int width, int height, MapMetadata metadata) : this(new float[width, height], metadata) {
    }

    public MapData(float[,] data, MapMetadata metadata) {
        this.data = data;
        scale = 1 / (float)Mathf.Max(data.GetLength(0), data.GetLength(1));
        this.metadata = metadata;
        converter = new CoordinateConverter(this.metadata.GetCellsize());
    }

    public static MapData ForTesting(float[,] data) {
        DummyMetadata metadata = new DummyMetadata();
        metadata.minHeight = data.Cast<float>().Min();
        metadata.maxHeight = data.Cast<float>().Max();
        metadata.cellsize = 1;
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
        Vector2 topLeftVector = GetTopLeft();
        double centerXRelativeToLowerLeftCorner = data.GetLength(0) / 2.0;
        double centerYRelativeToLowerLeftCorner = data.GetLength(1) / 2.0;
        double topLeftLon = converter.TransformCoordinateByLatLonDistance(centerXRelativeToLowerLeftCorner + topLeftVector.x, metadata.GetLowerLeftCornerX());
        double topLeftLat = converter.TransformCoordinateByLatLonDistance(centerYRelativeToLowerLeftCorner + topLeftVector.y, metadata.GetLowerLeftCornerY());
        return new MapPoint(topLeftLon, topLeftLat);
    }

    /// <summary>
    /// Returns the MapPoint(x,y) in WebMercator of the center of the top left cell of this map.
    /// </summary>
    public MapPoint GetTopLeftAsWebMercator() {
        Vector2 topLeftVector = GetTopLeft();
        double centerXRelativeToLowerLeftCorner = data.GetLength(0) / 2.0;
        double centerYRelativeToLowerLeftCorner = data.GetLength(1) / 2.0;
        double topLeftXWebMercator = converter.TransformCoordinateByWebMercatorDistance(centerXRelativeToLowerLeftCorner + topLeftVector.x, metadata.GetLowerLeftCornerX());
        double topLeftYWebMercator = converter.TransformCoordinateByWebMercatorDistance(centerYRelativeToLowerLeftCorner + topLeftVector.y, metadata.GetLowerLeftCornerY());
        return new MapPoint(topLeftXWebMercator, topLeftYWebMercator);
    }

    /// <summary>
    /// Returns the MapPoint(lon,lat) of the center of the cell that is at position 
    /// Vector2(x,y) relative to the center of the top left cell of the map. 
    /// </summary>
    public MapPoint GetLatLonCoordinates(Vector2 positionOnMap) {
        if (positionOnMap.x < -0.5 || positionOnMap.x > GetWidth() - 0.5
            || positionOnMap.y > 0.5 || positionOnMap.y < -GetHeight() + 0.5) {
            throw new System.ArgumentException("Index out of bounds! (" + positionOnMap.x + ", " + positionOnMap.y + ")");
        }
        MapPoint topLeft = GetTopLeftLatLonPoint();
        double x = converter.TransformCoordinateByLatLonDistance((double)positionOnMap.x, (double)topLeft.x);
        double y = converter.TransformCoordinateByLatLonDistance((double)positionOnMap.y, (double)topLeft.y);
        return new MapPoint(x, y);
    }

    /// <summary>
    /// Returns the MapPoint(x,y) in WebMercator of the center of the cell that 
    /// is at positionVector2(x,y) relative to the center of the top left cell of the map. 
    /// </summary>
    public MapPoint GetWebMercatorCoordinates(Vector2 positionOnMap) {
        if (positionOnMap.x < -0.5 || positionOnMap.x > GetWidth() - 0.5
            || positionOnMap.y > 0.5 || positionOnMap.y < -GetHeight() + 0.5) {
            throw new System.ArgumentException("Index out of bounds! (" + positionOnMap.x + ", " + positionOnMap.y + ")");
        }
        MapPoint topLeft = GetTopLeftAsWebMercator();
        double x = converter.TransformCoordinateByWebMercatorDistance((double)positionOnMap.x, (double)topLeft.x);
        double y = converter.TransformCoordinateByWebMercatorDistance((double)positionOnMap.y, (double)topLeft.y);
        return new MapPoint(x, y);
    }

    /// <summary>
    /// Takes a MapPoint(lon,lat) as parameter and returns a Vector2(x,y) that gives 
    /// the map-specific coordinates relative to the center point of the map. 
    /// </summary>
    public Vector2 GetMapSpecificCoordinatesFromLatLon(MapPoint latLonPoint) {
        float maxXDistance = (float)converter.TransformCoordinateByLatLonDistance(0, (GetWidth() / 2.0));
        float maxYDistance = (float)converter.TransformCoordinateByLatLonDistance(0, (GetHeight() / 2.0));

        MapPoint sliceTopLeft = GetTopLeftLatLonPoint();
        double cells = (GetWidth() - 1) / 2.0;
        double sliceCenterLon = converter.TransformCoordinateByLatLonDistance(cells, sliceTopLeft.x);
        double sliceCenterLat = converter.TransformCoordinateByLatLonDistance(-((GetHeight() - 1) / 2.0), sliceTopLeft.y);

        float xVectorFromCenter = converter.TransformationInMapCellsBetweenLatLonCoordinates(sliceCenterLon, latLonPoint.x);
        float yVectorFromCenter = converter.TransformationInMapCellsBetweenLatLonCoordinates(sliceCenterLat, latLonPoint.y);

        if (Math.Abs(latLonPoint.x) > maxXDistance | Math.Abs(latLonPoint.y) > maxYDistance) {
            throw new System.ArgumentException("Index out of bounds! Point (" + latLonPoint.x + ", " + latLonPoint.y + ") not on " +
                "map slice with center point (" + sliceCenterLon + ", " + sliceCenterLat + ") and width " + GetWidth() + " and height " + GetHeight() + ".");
        }

        return new Vector2(xVectorFromCenter, yVectorFromCenter);
    }

    /// <summary>
    /// Takes a MapPoint(x,y) in WebMercator as parameter and returns a Vector2(x,y) that gives 
    /// the map-specific coordinates relative to the center point of the map. 
    /// </summary>
    public Vector2 GetMapSpecificCoordinatesFromWebMercator(MapPoint webMercatorPoint) {
        float maxXDistance = (float)converter.TransformCoordinateByWebMercatorDistance(0, (GetWidth() / 2.0));
        float maxYDistance = (float)converter.TransformCoordinateByWebMercatorDistance(0, (GetHeight() / 2.0));

        MapPoint sliceTopLeft = GetTopLeftAsWebMercator();
        double sliceCenterX = converter.TransformCoordinateByWebMercatorDistance(((GetWidth() - 1) / 2.0), sliceTopLeft.x);
        double sliceCenterY = converter.TransformCoordinateByWebMercatorDistance(-((GetHeight() - 1) / 2.0), sliceTopLeft.y);

        float xVectorFromCenter = converter.TransformationInMapCellsBetweenWebMercatorCoordinates(sliceCenterX, webMercatorPoint.x);
        float yVectorFromCenter = converter.TransformationInMapCellsBetweenWebMercatorCoordinates(sliceCenterY, webMercatorPoint.y);

        if (Math.Abs(xVectorFromCenter) > maxXDistance | Math.Abs(yVectorFromCenter) > maxYDistance) {
            throw new System.ArgumentException("Index out of bounds! Point (" + webMercatorPoint.x + ", " + webMercatorPoint.y + ") not on " +
                "map slice with center point (" + sliceCenterX + ", " + sliceCenterY + ") and width " + GetWidth() + " and height " + GetHeight() + ".");
        }

        // Flip the conversion on the y-axis to negative. Map-specific coordinates grow down.
        return new Vector2(xVectorFromCenter, -yVectorFromCenter);
    }

    /// <summary>
    /// Takes a MapPoint(lon,lat) as parameter and returns a Vector2(x,y) that gives 
    /// the map-specific coordinates relative to top left cell of the map. 
    /// </summary>
    public Vector2 GetMapSpecificCoordinatesRelativeToTopLeftFromLatLon(MapPoint latLonPoint) {
        MapPoint sliceTopLeft = GetTopLeftLatLonPoint();

        float xVectorFromTopLeft = converter.TransformationInMapCellsBetweenLatLonCoordinates(sliceTopLeft.x, latLonPoint.x);
        float yVectorFromTopLeft = converter.TransformationInMapCellsBetweenLatLonCoordinates(sliceTopLeft.y, latLonPoint.y);

        // Flip the conversion on the y-axis to negative. Map-specific coordinates grow down.
        return new Vector2(xVectorFromTopLeft, -(yVectorFromTopLeft));
    }

    /// <summary>
    /// Takes a MapPoint(x,y) in WebMercator as a parameter and returns a Vector2(x,y) that gives 
    /// the map-specific coordinates relative to top left cell of the map. 
    /// </summary>
    public Vector2 GetMapSpecificCoordinatesRelativeToTopLeftFromWebMercator(MapPoint webMercatorPoint) {
        MapPoint sliceTopLeft = GetTopLeftAsWebMercator();

        float xVectorFromTopLeft = converter.TransformationInMapCellsBetweenWebMercatorCoordinates(sliceTopLeft.x, webMercatorPoint.x);
        float yVectorFromTopLeft = converter.TransformationInMapCellsBetweenWebMercatorCoordinates(sliceTopLeft.y, webMercatorPoint.y);

        // Flip the conversion on the y-axis to negative. Map-specific coordinates grow down.
        return new Vector2(xVectorFromTopLeft, -yVectorFromTopLeft);
    }

    public virtual float GetRaw(int x, int y) {
        return data[x, y];
    }

    public float GetHeightMultiplier() {
        return (1 / metadata.GetCellsize()) * scale;
    }

    public float GetNormalized(int x, int y) {
        return (GetRaw(x, y) - metadata.GetMinHeight()) * GetHeightMultiplier();
    }

    public float GetSquished(int x, int y) {
        return (GetRaw(x, y) - metadata.GetMinHeight()) / (metadata.GetMaxHeight() - metadata.GetMinHeight());
    }

    public virtual MapDataSlice AsSlice() {
        return new MapDataSlice(this, 0, 0, GetWidth(), GetHeight());
    }

    public List<MapDataSlice> GetSlices(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int sliceWidth, int sliceHeight, bool doOffset = true) {
        if (sliceHeight <= (doOffset ? 1 : 0) || sliceWidth <= (doOffset ? 1 : 0)) {
            throw new System.ArgumentException("Too small slice width (" + sliceWidth + ") or height (" + sliceHeight + ")");
        }
        List<MapDataSlice> slices = new List<MapDataSlice>();
        int rowLen = 0, i = 0;
        for (int y = topLeftY; y < bottomRightY; y += sliceHeight - (doOffset ? 1 : 0)) {
            for (int x = topLeftX; x < bottomRightX; x += sliceWidth - (doOffset ? 1 : 0)) {
                MapDataSlice slice = new MapDataSlice(this, x, y, sliceWidth, sliceHeight);
                if(x > topLeftX) {
                    MapDataSlice other = slices.Last();
                    MapNeighborRelation relation = new MapNeighborRelation(other, slice, NeighborType.LeftRight);
                    slice.AddNeighbor(relation);
                    other.AddNeighbor(relation);
                }
                if(y > topLeftY) {
                    MapDataSlice other = slices[i - rowLen];
                    MapNeighborRelation relation = new MapNeighborRelation(other, slice, NeighborType.TopBottom);
                    slice.AddNeighbor(relation);
                    other.AddNeighbor(relation);
                } else {
                    rowLen++;
                }
                i++;
                slices.Add(slice);
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
