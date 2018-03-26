using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDisplay : MonoBehaviour {

    private bool showAreas = true;
    public Dictionary<Area, List<Vector2>> areaBoundings = new Dictionary<Area, List<Vector2>>();

    public void SetAreas(List<Area> areas, MapData mapData) {
        foreach (Area a in areas) {
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < a.nodeList.Count; i++) {
                DisplayNode displayNode = ChangeOSMNodeToDisplayNode(a.nodeList[i], mapData);
                points.Add(new Vector2(displayNode.x, displayNode.y));
            }
            areaBoundings.Add(a, points);
        }
        GameObject.FindObjectOfType<MapGenerator>().UpdateTextures();
    }

    private bool IsPointInPolygon(List<Vector2> poly, Vector2 point) {
        int i, j;
        bool c = false;
        for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++) {
            if ((((poly[i].x <= point.x) && (point.x < poly[j].x))
                    || ((poly[j].x <= point.x) && (point.x < poly[i].x)))
                    && (point.y < (poly[j].y - poly[i].y) * (point.x - poly[i].x)
                        / (poly[j].x - poly[i].x) + poly[i].y))

                c = !c;
        }
        return c;
    }

public DisplayNode ChangeOSMNodeToDisplayNode(OSMNode node, MapData mapData) {
        MapPoint nodeInLatLon = new MapPoint((double)node.lon, (double)node.lat);
        Vector2 mapRelativePoint;
        switch (mapData.metadata.GetMapDataType()) {
            case MapDataType.Binary:
                // Map coordinates in WM -> transform node coordinates to WM.
                CoordinateConverter converter = new CoordinateConverter();
                MapPoint nodeInWebMercator = converter.ProjectPointToWebMercator(nodeInLatLon);
                mapRelativePoint = mapData.GetMapSpecificCoordinatesRelativeToTopLeftFromWebMercator(nodeInWebMercator);
                break;
            case MapDataType.ASCIIGrid:
                // Map and node coordinates in LatLon, no conversion needed.
                mapRelativePoint = mapData.GetMapSpecificCoordinatesRelativeToTopLeftFromLatLon(nodeInLatLon);
                break;
            default:
                throw new System.Exception("Mapdata type not recognized.");
        }
        return new DisplayNode((int)mapRelativePoint.x, (int)mapRelativePoint.y);
    }

    public Color GetAreaColor(float x, float y) {
        if (!showAreas) return Color.black;
        foreach (Area a in areaBoundings.Keys) {
            if(IsPointInPolygon(areaBoundings[a], new Vector2(x,y))) {
            
                return Color.green;
            }
        }
        return Color.black;
    }
	
}
