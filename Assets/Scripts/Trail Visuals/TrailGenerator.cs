using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles full Trail pipeline from loading XML file to converting coordinates
/// to displaying trails in Unity.  
/// </summary>

public class TrailGenerator : MonoBehaviour {

	public MapGenerator mapGenerator;
	private MapData mapData;
	private TrailDisplay display;

	// Use this for initialization
	void Start () {
		StartCoroutine(StartInit (60));
	}

	IEnumerator StartInit(int counter) {
		for (int val = 0; val <= counter; val += 1) {
			yield return null;
		}
		GenerateTrails ();
	}

	public void GenerateTrails() {
		mapGenerator = GameObject.Find ("MapGenerator").GetComponent<MapGenerator>();
		mapData = mapGenerator.mapData;

		display = this.GetComponent<TrailDisplay> ();
		display.mapData = mapData;

		ColorHandler colorHandler = new ColorHandler ();

		TrailData trailData = TrailDataImporter.ReadTrailData ("Assets/Resources/testData/testTrailData2.xml");

		foreach (Trail trail in trailData.trails) {
			display.trailColor = colorHandler.SelectColor(trail.colorName);
			display.DisplayNodes(TranslateTrail (trail));
		}
	}

	public List<DisplayNode> TranslateTrail(Trail trail) {
		List<DisplayNode> displayNodes = new List<DisplayNode> ();

		List<TrailNode> nodes = trail.GetNodeList();

		for (int i = 0; i < nodes.Count - 1 ; i++) {
			TrailNode node = nodes[i];
			TrailNode nextNode = nodes[i + 1];			
			displayNodes.Add (GenerateDisplayNode(node));				
			displayNodes.Add (GenerateDisplayNodeFromAverage(node, nextNode));;
		}
		TrailNode lastNode = nodes[nodes.Count  - 1];		
		displayNodes.Add (GenerateDisplayNode(lastNode));

		return displayNodes;
	}

	public DisplayNode GenerateDisplayNode(TrailNode node) {
		Vector2 point = mapData.GetMapSpecificCoordinatesFromLatLon (new MapPoint((double) node.GetLon(), (double) node.GetLat ()));
		return new DisplayNode((int) point.x, (int) point.y);
	}

	public DisplayNode GenerateDisplayNodeFromAverage(TrailNode node, TrailNode nextNode) {
		Vector2 nextPoint = mapData.GetMapSpecificCoordinatesFromLatLon (
				new MapPoint((double) ((node.GetLon() + nextNode.GetLon()) / 2),
				(double) ((node.GetLat() + nextNode.GetLat()) / 2))
		);
		return new DisplayNode((int) nextPoint.x, (int) nextPoint.y);
	}


}

