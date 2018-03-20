using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class POIDisplayTest {

	public MapData mapdata;
	public POIDisplay display;

	[SetUp]
	public void Setup() {
		GameObject nodeGameObject = new GameObject ();

		GameObject POIDisplayObject = new GameObject ();

		display = POIDisplayObject.AddComponent<POIDisplay> ();
		display.nodeGameObject = nodeGameObject;
        
		MapMetadata metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
		display.mapData = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
	}
	// Tests for individual node game objects

	[Test]
	public void POIDisplayCanCreateNewNodeInCenter() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));

		display.DisplayPOINode (new DisplayNode(1,1));		
		Assert.True (display.transform.GetChild(0).position.x == 0, "Incorrect x-coordinate!");
		Assert.True (display.transform.GetChild(0).position.z == 0, "Incorrect y-coordinate!");

		Assert.True (display.transform.childCount == 1, "No node was created!");
		
	}
	
}
