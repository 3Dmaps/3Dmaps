using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks whether a point is contained within an OSM area and returns the area color
/// for the point if point is inside the area. Areas consist of a list of DisplayNodes.
/// This list is treated as a polygon with normal areas and as a series of lines with rivers.
/// </summary>

public class AreaDisplay : MonoBehaviour {

    private bool showAreas = true;

	private List<DisplayPoly> displayPolys = new List<DisplayPoly>();

    public const int riverWidthConstant = 4;

	public void AddArea(Color color, List<DisplayNode> areaBounds) {
		DisplayPoly poly = new DisplayPoly (areaBounds);
		poly.boundingBox = BoundingBoxUtil.BoundingBox (areaBounds);
		poly.color = color;
		poly.type = PolyType.Area;

		displayPolys.Add (poly);
	}

    public void AddRiver(List<DisplayNode> riverNodes) {		
		DisplayPoly poly = new DisplayPoly (riverNodes);
		poly.boundingBox = BoundingBoxUtil.BoundingBox (riverNodes);
		poly.color = Color.blue;
		poly.type = PolyType.River;

		displayPolys.Add (poly);
	}

	public void displayAreas() {
		GameObject.FindObjectOfType<MapGenerator>().UpdateTextures();
	}

	private bool IsPointInPolygon(List<DisplayNode> areaBounds, DisplayNode point) {
        int i, j;
        bool c = false;
        for (i = 0, j = areaBounds.Count - 1; i < areaBounds.Count; j = i++) {
            if ((((areaBounds[i].x <= point.x) && (point.x < areaBounds[j].x))
                    || ((areaBounds[j].x <= point.x) && (point.x < areaBounds[i].x)))
                    && (point.y < (areaBounds[j].y - areaBounds[i].y) * (point.x - areaBounds[i].x)
                        / (areaBounds[j].x - areaBounds[i].x) + areaBounds[i].y))
                c = !c;
        }
        return c;
    }
    public bool isPointInsideBoundingBox(List<int> box, int x, int y) {
        if (x > box[2] || x < box[0] || y > box[3] || y < box[1]) {
            return false;
        } 
        return true;
    }

    

    public Color GetPointColor(float x, float y) {
		if (!showAreas) {
			return Color.clear;  
		}

        return GetAreaColor(x, y);
    }

    public Color GetAreaColor(float x, float y) {

		foreach (DisplayPoly poly in displayPolys) {
			if (isPointInsideBoundingBox (poly.boundingBox, (int)x, (int)y)) {            
				if (poly.type == PolyType.Area) {
					if (IsPointInPolygon (poly.displayNodes, new DisplayNode ((int)x, (int)y))) {
						return poly.color;
					}
				} else if (poly.type == PolyType.River) {
					if (InRiver	(poly.displayNodes, x, y)) {
						return poly.color;
					}
				}
			}
		}
		return Color.clear;
    }

    public bool InRiver(List<DisplayNode> displayNodes, float x, float y) {
		for (int i = 0; i < displayNodes.Count - 1; i++) {
			DisplayNode point = new DisplayNode((int) x, (int) y);                                           
			DisplayNode riverNode1 = displayNodes[i];
			DisplayNode riverNode2 = displayNodes[i + 1];                            
			if (SegmentUtil.FindDistanceToSegmentRiver(point, riverNode1, riverNode2) < riverWidthConstant) {                                       
				return true;
			}
		}
		return false;
    }        
}
