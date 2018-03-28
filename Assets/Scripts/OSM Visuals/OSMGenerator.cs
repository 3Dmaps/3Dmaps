﻿using System.Collections;
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
    private AreaDisplay areaDisplay;

	private ColorHandler colorHandler;

	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data

	List<DisplayNode> displayNodes;
	IconHandler iconHandler;


	public void GenerateOSMObjects(MapGenerator mapGenerator, string mapName) {
		mapData         = mapGenerator.mapData;
		lineDisplay 	= this.GetComponent<LineDisplay> ();
		poiDisplay      = this.GetComponent<POIDisplay> ();
        iconHandler     = this.GetComponent<IconHandler>();
        areaDisplay     = this.GetComponent<AreaDisplay>();
        OSMData osmData = DataImporter.GetOSMData(mapName);

		colorHandler = new ColorHandler ();

		lineDisplay.mapData	 = mapData;
		poiDisplay.mapData   = mapData;

		iconHandler.generateIconDictionary();
		GenerateTrails(osmData);
		GenerateRivers(osmData);
		GeneratePoiNodes(osmData);
		GenerateAreas (osmData);
	}

	private void GenerateTrails(OSMData osmData) {
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
			DisplayNode displayNode = ChangeLatLonToDisplayNode (poiNode.lon, poiNode.lat, mapData);
			Icon icon = iconHandler.SelectIcon(poiNode.icon);
			poiDisplay.DisplayPOINode(displayNode, icon);
		}
	}

	private void GenerateAreas(OSMData osmData) {
		foreach (Area a in osmData.areas) {
			List<DisplayNode> areaBounds = new List<DisplayNode>();
			for (int i = 0; i < a.nodeList.Count; i++) {
				areaBounds.Add(ChangeLatLonToDisplayNode(a.nodeList[i].lon, a.nodeList[i].lat, mapData));
			}
			areaDisplay.AddArea (colorHandler.SelectAreaColor (a.type), areaBounds);
		}
		areaDisplay.displayAreas ();
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
		DisplayNode displayNode = ChangeLatLonToDisplayNode (node.lon, node.lat, mapData);
        displayNodes.Add(displayNode);
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

	public DisplayNode ChangeLatLonToDisplayNode(float lon, float lat, MapData mapData) {
		MapPoint nodeInLatLon = new MapPoint((double) lon, (double) lat);
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
}

