using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles full Trail pipeline from loading XML file to converting coordinates
/// to displaying trails in Unity.  
/// </summary>

public class TrailGenerator : MonoBehaviour {

	private MapData mapData;
	private TrailDisplay display;
	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data
	List<DisplayNode> displayNodes;


	public void GenerateTrails(MapGenerator mapGenerator) {
		mapData = mapGenerator.mapData;
		display = this.GetComponent<TrailDisplay> ();
		display.mapData = mapData;

		ColorHandler colorHandler = new ColorHandler ();

		TrailData trailData = TrailDataImporter.ReadTrailData (GetDataPath("SampleTrailDataCanyon.xml"));

		foreach (Trail trail in trailData.trails) {
			display.trailColor = colorHandler.SelectColor(trail.colorName);
			display.DisplayNodes(TranslateTrail (trail));
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
		Vector2 point = mapData.GetRawCoordinatesFromLatLon (new MapPoint((double) node.GetLon(), (double) node.GetLat ()));
		displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
	}

	public void AddDisplayNode(TrailNode node, TrailNode nextNode) {
		if (nodeGenerationRate < 1) {
			return;
		}

		for (int i = 1; i <= this.nodeGenerationRate; i++) {		
			Vector2 point = mapData.GetRawCoordinatesFromLatLon (
					new MapPoint(
						x: (double) (i * (nextNode.GetLon() - node.GetLon()) / (nodeGenerationRate + 1) + node.GetLon()),
                        y: (double) (i * (nextNode.GetLat() - node.GetLat()) / (nodeGenerationRate + 1) + node.GetLat())
					)					
			);
			displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
		}
	}


}

