using System.Collections.Generic;
using UnityEngine;
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
        for(int y = 0; y < GetHeight(); y += sliceSize) {
            for(int x = 0; x < GetWidth(); x += sliceSize) {
                slices.Add(new MapDataSlice(this, x, y, sliceSize + 1, sliceSize + 1));
            }
        }
        return slices;
    }

}