using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks whether a point is contained within an OSM area and returns the area color
/// for the point if point is inside the area.  
/// </summary>

public class AreaDisplay : MonoBehaviour {

    private bool showAreas = true;
	private List<List<DisplayNode>> areaBoundings = new List<List<DisplayNode>>();
    private List<List<DisplayNode>> rivers = new List<List<DisplayNode>>();

    private List<List<int>> areaBoundingBoxes = new List<List<int>>();
    private List<List<int>> riverBoundingBoxes = new List<List<int>>();

	private List<Color> areaColors = new List<Color> ();

    public const int riverWidthConstant = 4;

	public void AddArea(Color color, List<DisplayNode> areaBounds) {
		areaColors.Add (color);
		areaBoundings.Add (areaBounds);
        areaBoundingBoxes.Add (BoundingBoxUtil.BoundingBox(areaBounds));

	}

    public void AddRiver(List<DisplayNode> riverNodes) {		
		rivers.Add (riverNodes);
        riverBoundingBoxes.Add (BoundingBoxUtil.BoundingBox(riverNodes));               
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
        if (!showAreas) return Color.black;        
        Color color = GetAreaColor(x, y);
        if (InRiver(x, y)) {
            color = Color.blue;
        }
        return color;
    }

    public Color GetAreaColor(float x, float y) {

        for (int areaNum = 0; areaNum < areaBoundings.Count; areaNum++) {
            
            List<int> boundingBoxArea = areaBoundingBoxes[areaNum];
            if(isPointInsideBoundingBox(boundingBoxArea, (int) x, (int) y)) {            
			    if(IsPointInPolygon(areaBoundings[areaNum], new DisplayNode((int) x, (int) y))) {
				    return areaColors[areaNum];
                }
            }
        }
        return Color.black;
    }

    public bool InRiver(float x, float y) {

        for (int riverNum = 0; riverNum < rivers.Count; riverNum++) {
            List<int> boundingBoxRiver = riverBoundingBoxes[riverNum];              
            if (isPointInsideBoundingBox(boundingBoxRiver, (int) x, (int) y)) {                                  
                for (int i = 0; i < rivers[riverNum].Count - 1; i++) {
                    DisplayNode point = new DisplayNode((int) x, (int) y);                                           
                    DisplayNode riverNode1 = rivers[riverNum][i];
                    DisplayNode riverNode2 = rivers[riverNum][i + 1];                            
                    if (SegmentUtil.FindDistanceToSegmentRiver(point, riverNode1, riverNode2) < riverWidthConstant) {                                       
                        return true;
                    }

                }
            }
        }
        return false;
    }        
    

}
