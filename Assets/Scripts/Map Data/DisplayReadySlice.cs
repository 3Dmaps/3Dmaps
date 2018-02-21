using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A MapDataSlice with LOD-related functionality included
/// </summary>

public class DisplayReadySlice : MapDataSlice {

    public int lod;
    public DisplayReadySlice(MapDataSlice slice, int lod) : base(slice) {
        this.lod = lod;
    }

    private int GetActualLOD() {
        return lod == 0 ? 1 : lod * 2;
    }

    private int SimplificationIncrement(int coordinate, int dimension) {
        int actualLOD = GetActualLOD();
        int increment = coordinate + actualLOD >= dimension ? dimension - coordinate - 1 : actualLOD;
        return increment > 0 ? increment : 1;
    }

    public int SimplificationIncrementForY(int y) {
        return SimplificationIncrement(y, GetHeight());
    }

    public int SimplificationIncrementForX(int x) {
        return SimplificationIncrement(x, GetWidth());
    }

}
