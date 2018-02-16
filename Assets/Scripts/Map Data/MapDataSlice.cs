using UnityEngine;

/// <summary>
/// Part of a bigger MapData; gives access to only a portion of the data, handles like
/// a complete MapData object
/// </summary>

public class MapDataSlice : MapData {

    protected int topLeftX, topLeftY, width, height;

    public MapDataSlice(MapData mapData, int topLeftX, int topLeftY, int width, int height) : base(mapData){
        this.topLeftX = topLeftX; this.topLeftY = topLeftY;
        this.width = width; this.height = height;
    }

    public MapDataSlice(MapDataSlice slice) : this(slice, slice.topLeftX, slice.topLeftY, slice.width, slice.height) {
    }

    public int GetX() {
        return topLeftX;
    }

    public int GetY() {
        return topLeftY;
    }

    public void SetX(int x) {
        topLeftX = x;
    }

    public void SetY(int y) {
        topLeftY = y;
    }

    public override int GetWidth() {
        return topLeftX + width > base.GetWidth() ? base.GetWidth() - topLeftX : width;
    }

    public override int GetHeight() {
        return topLeftY + height > base.GetHeight() ? base.GetHeight() - topLeftY : height;
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