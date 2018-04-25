using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A MapDataSlice with LOD-related functionality included
/// </summary>

public class DisplayReadySlice : MapDataSlice {

    public MapDataSlice baseSlice;
	public int lod;
    public List<DisplayNeighborRelation> displayNeighbors;

	public DisplayReadySlice(MapDataSlice slice, int lod) : base(slice) {
        this.baseSlice = slice;
		this.lod = lod;
        this.neighbors = slice.GetNeighbors();
        this.displayNeighbors = new List<DisplayNeighborRelation>(expectedNumberOfNeighbors);
	}

    public override DisplayReadySlice AsDisplayReadySlice(int lod) {
        this.lod = lod;
        return this;
    }

	public int GetActualLOD() {
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

    public List<DisplayNeighborRelation> GetDisplayNeighbors() {
        return this.displayNeighbors;
    }

    public void AddDisplayNeighbor(DisplayNeighborRelation relation) {
        relation.AddDisplayReadySlice(this);
        this.displayNeighbors.Add(relation);
    }

    public void AddDisplay(MapDisplay display) {
        foreach(DisplayNeighborRelation neighbor in this.displayNeighbors) {
            neighbor.AddDisplay(this, display);
        }
    }
	
}
