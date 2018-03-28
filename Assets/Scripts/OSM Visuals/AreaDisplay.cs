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
	private List<Color> areaColors = new List<Color> ();

	public void AddArea(Color color, List<DisplayNode> areaBounds) {
		areaColors.Add (color);
		areaBoundings.Add (areaBounds);
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

    public Color GetAreaColor(float x, float y) {
        if (!showAreas) return Color.black;
		for (int areaNum = 0; areaNum < areaBoundings.Count; areaNum++) {
			if(IsPointInPolygon(areaBoundings[areaNum], new DisplayNode((int) x, (int) y))) {
				return areaColors[areaNum];
            }
        }
        return Color.black;
    }
	
}
