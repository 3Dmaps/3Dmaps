using UnityEngine;

/// <summary>
/// Part of a bigger MapData; gives access to only a portion of the data, handles like
/// a complete MapData object
/// </summary>

public class MapDataSlice : MapData {

    private int topLeftX, topLeftY, width, height, LOD;

    public MapDataSlice(MapData mapData, int topLeftX, int topLeftY, int width, int height, int LOD) : base(mapData){
        this.topLeftX = topLeftX; this.topLeftY = topLeftY;
        this.width = width; this.height = height;
        this.LOD = LOD;
    }

    public override int GetWidth() {

        if (topLeftX + width <= base.GetWidth()) return width;

        int trueValue = base.GetWidth() - topLeftX;
        int lod = LOD == 0 ? 1 : LOD * 2;
        int remain = trueValue % lod;
        return (remain == 0 ? trueValue : trueValue - remain) + 1;

        //return topLeftX + width > base.GetWidth() ? base.GetWidth() - topLeftX : width;
    }

    public override int GetHeight() {

        if (topLeftY + height <= base.GetHeight()) return height;

        int trueValue = base.GetHeight() - topLeftY;
        int lod = LOD == 0 ? 1 : LOD * 2;
        int remain = trueValue % lod;
        return (remain == 0 ? trueValue : trueValue - remain) + 1;

        //return topLeftY + height > base.GetHeight() ? base.GetHeight() - topLeftY : height;
    }

    public override Vector2 GetTopLeft(){
        return new Vector2((base.GetWidth() - 1) / -2f + topLeftX, (base.GetHeight() - 1) / 2f - topLeftY);
    }

    public override float GetRaw(int x, int y) {
        if(x >= GetWidth() || y >= GetHeight() || x < 0 || y < 0) {
            throw new System.ArgumentException("Index out of bounds! (" + x + ", " + y + ")");
        }
        return data[x + this.topLeftX, y + this.topLeftY];
    }
}