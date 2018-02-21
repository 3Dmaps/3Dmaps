using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			display.trailColor = colorHandler.selectColor(trail.colorName);
			display.DisplayNodes(TranslateTrail (trail));
		}
	}

	public List<DisplayNode> TranslateTrail(Trail trail) {
		List<DisplayNode> displayNodes = new List<DisplayNode> ();

		foreach (TrailNode node in trail.GetNodeList()) {
			Vector2 point = mapData.GetMapSpecificCoordinatesFromLatLon (new MapPoint((double) node.GetLon(), (double) node.GetLat ()));

			DisplayNode displayNode = new DisplayNode((int) point.x, (int) point.y);
			displayNodes.Add (displayNode);
		}

		return displayNodes;
	}
}
