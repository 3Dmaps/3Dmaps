public class MapMetadata {

    public const string nrowsKey = "nrows", ncolsKey = "ncols", cellsizeKey = "cellsize", nodatavalueKey = "NODATA_value", minheightKey = "minheight", maxheightKey = "maxheight";

    public int nrows = 0, ncols = 0;
    public float cellsize = 0f, nodatavalue = -9999f, minheight = 0f, maxheight = 1000f;
    public void Set(string key, string value) {
        // Ugly, but will hopefully do for now >:
        switch(key) {
            case nrowsKey:
                nrows = int.Parse(value);
                break;
            case ncolsKey:
                ncols = int.Parse(value);
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