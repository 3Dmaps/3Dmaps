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

    public List<MapData> GetSlices(int sliceSize) {
        List<MapData> slices = new List<MapData>();
        for(int y = 0; y < GetHeight(); y += sliceSize - 1) {
            for(int x = 0; x < GetWidth(); x += sliceSize - 1) {
                slices.Add(new MapDataSlice(this, x, y, sliceSize, sliceSize));
            }
        }
        return slices;
    }

}