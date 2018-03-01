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
	public int nodeGenerationRate = 1; // number of new nodes created between adjacent nodes in data
	List<DisplayNode> displayNodes;

	
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
		Vector2 point = mapData.GetMapSpecificCoordinatesFromLatLon (new MapPoint((double) node.GetLon(), (double) node.GetLat ()));
		displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));		
	}

	public void AddDisplayNode(TrailNode node, TrailNode nextNode) {
		if (nodeGenerationRate < 1) {
			return;
		}

		for (int i = 1; i <= this.nodeGenerationRate; i++) {		
			Vector2 point = mapData.GetMapSpecificCoordinatesFromLatLon (
					new MapPoint(
						x: (double) (i * (nextNode.GetLon() - node.GetLon()) / (nodeGenerationRate + 1) + node.GetLon()),
                        y: (double) (i * (nextNode.GetLat() - node.GetLat()) / (nodeGenerationRate + 1) + node.GetLat())
					)					
			);
			displayNodes.Add(new DisplayNode((int) point.x, (int) point.y));
		}
	}


}

