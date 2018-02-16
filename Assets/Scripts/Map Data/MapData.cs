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

    public MapData(MapData mapData) : this(mapData.data, mapData.metadata){
    }

    public MapData(int width, int height, MapMetadata metadata) : this(new float[width, height], metadata) {
    }

    public MapData(float[,] data, MapMetadata metadata) {
        this.data = data;
        scale = 1 / (float)Mathf.Max(data.GetLength(0), data.GetLength(1));
        this.metadata = metadata;
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

    public virtual float GetRaw(int x, int y) {
        return data[x, y];
    }

    public float GetHeightMultiplier() {
        return (1 / metadata.cellsize) * scale;
    }

    public float GetNormalized(int x, int y){
        return (GetRaw(x, y) - metadata.minheight) * GetHeightMultiplier();
    }

    public float GetSquished(int x, int y) {
        return (GetRaw(x, y) - metadata.minheight) / (metadata.maxheight - metadata.minheight);
    }

    public MapDataSlice AsSlice() {
        return new MapDataSlice(this, 0, 0, GetWidth(), GetHeight());
    }

    public List<MapDataSlice> GetSlices(int topLeftX, int topLeftY, int bottomRightX, int bottomRightY, int sliceWidth, int sliceHeight, bool doOffset = true) {
        if(sliceHeight <= (doOffset ? 1 : 0) || sliceWidth <= (doOffset ? 1 : 0)) {
            throw new System.ArgumentException("Too small slice width (" + sliceWidth + ") or height (" + sliceHeight + ")");
        }
        List<MapDataSlice> slices = new List<MapDataSlice>();
        for(int y = topLeftY; y < bottomRightY; y += sliceHeight - (doOffset ? 1 : 0)) {
            for(int x = topLeftX; x < bottomRightX; x += sliceWidth - (doOffset ? 1: 0)) {
                slices.Add(new MapDataSlice(this, x, y, sliceWidth, sliceHeight));
            }
        }
        return slices;
    }

    public List<MapData> GetSlices(int sliceSize) {
        return GetSlices(0, 0, GetWidth(), GetHeight(), sliceSize, sliceSize).ConvertAll(s => (MapData) s);
    }

    public List<DisplayReadySlice> GetDisplayReadySlices(int displayWidth, int displayHeight, int topLeftX, int topLeftY, int[,] lodMatrix) {
        int sliceWidth = displayWidth / lodMatrix.GetLength(0) + lodMatrix.GetLength(0);
        int sliceHeight = displayHeight / lodMatrix.GetLength(1) + lodMatrix.GetLength(1);
        List<MapDataSlice> slices = GetSlices(topLeftX, topLeftY, topLeftX + displayWidth - (displayWidth % lodMatrix.GetLength(0)), topLeftY + displayHeight - (displayHeight % lodMatrix.GetLength(1)), sliceWidth, sliceHeight);
        // List<DisplayReadySlice> displayReadies = slices.ConvertAll(s => s.AsDisplayReadySlice(lodMatrix[0,0])); // TODO: Get rid of lodMatrix
        List<DisplayReadySlice> displayReadies = new List<DisplayReadySlice>();
        for(int y = 0; y < lodMatrix.GetLength(1); y++) {
            for(int x = 0; x < lodMatrix.GetLength(0); x++) {
                MapDataSlice slice = slices[y * lodMatrix.GetLength(0) + x];
                displayReadies.Add(new DisplayReadySlice(slice, lodMatrix[x, y]));
            }
        }
        return displayReadies;
    }
}