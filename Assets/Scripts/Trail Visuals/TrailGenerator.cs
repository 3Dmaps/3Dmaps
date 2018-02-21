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

	// Update is called once per frame
	void Update () {

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

		print ("map data found, top left is " + mapData.GetTopLeftLatLonPoint ().x + "/" + mapData.GetTopLeftLatLonPoint ().y);
		print ("?" + mapData.GetMapSpecificCoordinatesFromLatLon(mapData.GetTopLeftLatLonPoint()).x);

		TrailData trailData = TrailDataImporter.ReadTrailData ("Assets/Resources/testData/testTrailData2.xml");

		foreach (Trail trail in trailData.trails) {
			display.DisplayNodes(TranslateTrail (trail));
		}
	}

	public List<DisplayNode> TranslateTrail(Trail trail) {
		List<DisplayNode> displayNodes = new List<DisplayNode> ();

		foreach (TrailNode node in trail.GetNodeList()) {
			Vector2 point = mapData.GetMapSpecificCoordinatesFromLatLon (new MapPoint((double) node.GetLon(), (double) node.GetLat ()));
			print ("node lat/lon: " + node.GetLat() + "/" + node.GetLon());
			print ("Point: " + point.x + "/" + point.y);
			DisplayNode displayNode = new DisplayNode((int) point.x, (int) point.y);
			displayNodes.Add (displayNode);
		}

		return displayNodes;
	}
}
