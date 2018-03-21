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
	private POIDisplay poiDisplay;
	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data
	List<DisplayNode> displayNodes;
	IconHandler iconHandler;


	public void GenerateTrails(MapGenerator mapGenerator) {
		mapData = mapGenerator.mapData;
		display = this.GetComponent<TrailDisplay> ();
		poiDisplay = this.GetComponent<POIDisplay> ();
		display.mapData = mapData;
		poiDisplay.mapData = mapData;

		iconHandler = this.GetComponent<IconHandler> ();
		iconHandler.generateIconDictionary();
		
		ColorHandler colorHandler = new ColorHandler ();

		OSMData osmData = OSMDataImporter.ReadOSMData (GetDataPath("SampleTrailDataCanyon.xml"));

		foreach (Trail trail in osmData.trails) {
			display.trailColor = colorHandler.SelectColor(trail.colorName);
			display.DisplayNodes(TranslateTrail (trail));
		}
		foreach (POINode poiNode in osmData.poiNodes) {
			Vector2 point = mapData.GetRawCoordinatesFromLatLon(new MapPoint((double) poiNode.lon, (double) poiNode.lat));
			Icon icon = iconHandler.SelectIcon(poiNode.icon);
			poiDisplay.DisplayPOINode(new DisplayNode((int) point.x, (int) point.y), icon);
		}
	}


    private string GetDataPath(string filename)
    {
    #if UNITY_EDITOR
        return Application.dataPath + "/StreamingAssets/" + filename;
    #endif

    #if UNITY_IPHONE
        return Application.dataPath + "/Raw/" + filename;
    #endif

    #if UNITY_ANDROID
        return "jar:file://" + Application.dataPath + "!/assets/" + filename;
    #endif
        return filename;
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
		Vector2 point = mapData.GetRawCoordinatesFromLatLon (new MapPoint((double) node.lon, (double) node.lat));
		displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
	}

	public void AddDisplayNode(TrailNode node, TrailNode nextNode) {
		if (nodeGenerationRate < 1) {
			return;
		}

		for (int i = 1; i <= this.nodeGenerationRate; i++) {		
			Vector2 point = mapData.GetRawCoordinatesFromLatLon (
					new MapPoint(
						x: (double) (i * (nextNode.lon - node.lon) / (nodeGenerationRate + 1) + node.lon),
                        y: (double) (i * (nextNode.lat - node.lat) / (nodeGenerationRate + 1) + node.lat)
					)					
			);
			displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
		}
	}


}

