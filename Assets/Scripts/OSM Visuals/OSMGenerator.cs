using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles full Trail pipeline from loading XML file to converting coordinates
/// to displaying trails in Unity.  
/// </summary>

public class OSMGenerator : MonoBehaviour {

	private MapData mapData;

	private LineDisplay lineDisplay;
	private POIDisplay poiDisplay;
	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data

	List<DisplayNode> displayNodes;
	IconHandler iconHandler;


	public void GenerateOSMObjects(MapGenerator mapGenerator, string mapName) {
		mapData = mapGenerator.mapData;

		lineDisplay = this.GetComponent<LineDisplay> ();
		poiDisplay = this.GetComponent<POIDisplay> ();

		lineDisplay.mapData = mapData;
		poiDisplay.mapData = mapData;

		iconHandler = this.GetComponent<IconHandler> ();
		iconHandler.generateIconDictionary();

        OSMData osmData = DataImporter.GetOSMData(mapName);

		GenerateTrails(osmData);
		// GenerateRivers(osmData);
		GeneratePoiNodes(osmData);
	}

	private void GenerateTrails(OSMData osmData) {
		ColorHandler colorHandler = new ColorHandler ();
		foreach (Trail trail in osmData.trails) {
			lineDisplay.trailColor = colorHandler.SelectColor(trail.colorName);
			lineDisplay.DisplayNodes(TranslateLine (trail.GetNodeList()));
		}
	}

	private void GenerateRivers(OSMData osmData) {
		foreach (River river in osmData.rivers) {
			lineDisplay.trailColor = Color.blue;
			lineDisplay.DisplayNodes(TranslateLine (river.GetNodeList()));
		}
	}

	private void GeneratePoiNodes(OSMData osmData) {
		foreach (POINode poiNode in osmData.poiNodes) {
			Vector2 mapRelativePoint;
			MapPoint nodeInLatLon = new MapPoint((double)poiNode.lon, (double)poiNode.lat);

			switch (mapData.metadata.GetMapDataType ()) {
				case MapDataType.Binary:
						// Map coordinates in WM -> transform node coordinates to WM.
					CoordinateConverter converter = new CoordinateConverter ();
					MapPoint nodeInWebMercator = converter.ProjectPointToWebMercator (nodeInLatLon);
					mapRelativePoint = mapData.GetMapSpecificCoordinatesRelativeToTopLeftFromWebMercator (nodeInWebMercator);
					break;
				case MapDataType.ASCIIGrid:
						// Map and node coordinates in LatLon, no conversion needed.
					mapRelativePoint = mapData.GetMapSpecificCoordinatesRelativeToTopLeftFromLatLon (nodeInLatLon);
					break;
				default:
					throw new System.Exception ("Mapdata type not recognized.");    
			}
			Icon icon = iconHandler.SelectIcon(poiNode.icon);
			poiDisplay.DisplayPOINode(new DisplayNode((int) mapRelativePoint.x, (int) mapRelativePoint.y), icon);
		}
	}
		
	public List<DisplayNode> TranslateLine(List<OSMNode> nodes) {
		displayNodes = new List<DisplayNode> ();
		
		for (int i = 0; i < nodes.Count - 1 ; i++) {
			OSMNode node = nodes[i];
			OSMNode nextNode = nodes[i + 1];			
			AddDisplayNode(node);				
			AddDisplayNode(node, nextNode);
		}
		OSMNode lastNode = nodes[nodes.Count  - 1];		
		AddDisplayNode(lastNode);

		return displayNodes;
	}


	public void AddDisplayNode(OSMNode node) {
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

	public void AddDisplayNode(OSMNode node, OSMNode nextNode) {
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

