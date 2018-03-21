using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles full Trail pipeline from loading XML file to converting coordinates
/// to displaying trails in Unity.  
/// </summary>

public class OSMGenerator : MonoBehaviour {

	private MapData mapData;
	private TrailDisplay display;
	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data
	List<DisplayNode> displayNodes;


	public void GenerateTrails(MapGenerator mapGenerator, string mapName) {
		mapData = mapGenerator.mapData;
		display = GetComponent<TrailDisplay>();
		display.mapData = mapData;

		ColorHandler colorHandler = new ColorHandler ();
        OSMData osmData = DataImporter.GetOSMData(mapName);

		foreach (Trail trail in osmData.trails) {
			display.trailColor = colorHandler.SelectColor(trail.colorName);
			display.DisplayNodes(TranslateTrail (trail));
		}
	}

	public List<DisplayNode> TranslateTrail(Trail trail) {
		displayNodes = new List<DisplayNode> ();

		List<TrailNode> nodes = trail.GetNodeList();

		for (int i = 0; i < nodes.Count - 1 ; i++) {
			TrailNode node = nodes[i];
			TrailNode nextNode = nodes[i + 1];			
			AddDisplayNode(node);				
			AddDisplayNode(node, nextNode);
		}
		TrailNode lastNode = nodes[nodes.Count  - 1];		
		AddDisplayNode(lastNode);

		return displayNodes;
	}

	public void AddDisplayNode(TrailNode node) {
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
        displayNodes.Add(new DisplayNode((int) mapRelativePoint.x, (int) mapRelativePoint.y));
	}

	public void AddDisplayNode(TrailNode node, TrailNode nextNode) {
		if (nodeGenerationRate < 1) {
			return;
		}

		for (int i = 1; i <= this.nodeGenerationRate; i++) {		
			Vector2 point = mapData.GetMapSpecificCoordinatesRelativeToTopLeftFromLatLon(
					new MapPoint(
						x: (double) (i * (nextNode.lon - node.lon) / (nodeGenerationRate + 1) + node.lon),
                        y: (double) (i * (nextNode.lat - node.lat) / (nodeGenerationRate + 1) + node.lat)
					)					
			);
			displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
		}
	}


}

