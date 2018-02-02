using UnityEngine;
public class MapDataSlice : MapData {

    private int topLeftX, topLeftY, width, height;

    public MapDataSlice(MapData mapData, int topLeftX, int topLeftY, int width, int height) : base(mapData){
        this.topLeftX = topLeftX; this.topLeftY = topLeftY;
        this.width = width; this.height = height;
    }

    public override int GetWidth() {
        return topLeftX + width > base.GetWidth() ? base.GetWidth() - topLeftX : width + 1;
    }
    public override int GetHeight() {
        return topLeftY + height > base.GetHeight() ? base.GetHeight() - topLeftY : height + 1;
    }

    public override Vector2 GetTopLeft(){
        return new Vector2((base.GetWidth() - 1) / -2f + topLeftX, (base.GetHeight() - 1) / 2f - topLeftY);
    }

    public override float GetRaw(int x, int y) {
        if(x >= GetWidth() || y >= GetHeight() || x < 0 || y < 0) {
            throw new System.ArgumentException("Index out of bounds!");
        }
        return data[x + this.topLeftX, y + this.topLeftY];
    }
}