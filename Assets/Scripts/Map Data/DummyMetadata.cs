/// <summary>
/// Dummy metadata, used for testing purposes
/// </summary>
public class DummyMetadata : MapMetadata {

    public float cellsize = 0f, maxHeight = 1000f, minHeight = 0f;
    public double lowerLeftX = 0d, lowerLeftY = 0d;

    public float GetCellsize() {
        return cellsize;
    }

    public double GetLowerLeftCornerX() {
        return lowerLeftX;
    }

    public double GetLowerLeftCornerY() {
        return lowerLeftY;
    }

    public float GetMaxHeight() {
        return maxHeight;
    }

    public float GetMinHeight() {
        return minHeight;
    }

}
