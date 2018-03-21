/// <summary>
/// Container for various ASCIIGrid height map metadata
/// </summary>
public class ASCIIGridMetadata : MapMetadata {
    public const string nrowsKey = "nrows", ncolsKey = "ncols", xllcornerKey = "xllcorner", yllcornerKey = "yllcorner", cellsizeKey = "cellsize", nodatavalueKey = "NODATA_value", minheightKey = "minheight", maxheightKey = "maxheight";
    
    public int nrows = 0, ncols = 0;
    public double xllcorner = 0, yllcorner = 0;
    public float cellsize = 0f, nodatavalue = -9999f, minheight = 0f, maxheight = 1000f;

    public float GetCellsize() {
        return cellsize;
    }

    public double GetLowerLeftCornerX() {
        return xllcorner;
    }

    public double GetLowerLeftCornerY() {
        return yllcorner;
    }

    public MapDataType GetMapDataType() {
        return MapDataType.ASCIIGrid;
    }

    public float GetMaxHeight() {
        return maxheight;
    }

    public float GetMinHeight() {
        return minheight;
    }

    public void Set(string key, string value) {
        switch (key) {
            case nrowsKey:
                nrows = int.Parse(value);
                break;
            case ncolsKey:
                ncols = int.Parse(value);
                break;
            case xllcornerKey:
                xllcorner = double.Parse(value);
                break;
            case yllcornerKey:
                yllcorner = double.Parse(value);
                break;
            case nodatavalueKey:
                nodatavalue = float.Parse(value);
                break;
            case minheightKey:
                minheight = float.Parse(value);
                break;
            case maxheightKey:
                maxheight = float.Parse(value);
                break;
            case cellsizeKey:
                cellsize = float.Parse(value);
                break;
        }
    }

}
