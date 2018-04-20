using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class POIDisplayTest {

	public MapData mapdata;
	public POIDisplay display;
	public Icon icon;
	public ColorHandler colorHandler;


	[SetUp]
	public void Setup() {
		GameObject nodeGameObject = new GameObject ();

		GameObject POIDisplayObject = new GameObject ();
		nodeGameObject.AddComponent<SpriteRenderer> ();

		display = POIDisplayObject.AddComponent<POIDisplay> ();
		display.nodeGameObject = nodeGameObject;
		
		icon = new Icon();
		icon.sprite = new Sprite ();

		colorHandler = new ColorHandler();		
        
		MapMetadata metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
		display.mapData = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
	}
	// Tests for individual node game objects

	[Test]
	public void POIDisplayCanCreateNewNodeInCenter() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));

		display.DisplayPOINode (new DisplayNode(1,1), icon, colorHandler);		
		Assert.True (display.transform.GetChild(0).position.x == 0, "Incorrect x-coordinate!");
		Assert.True (display.transform.GetChild(0).position.z == 0, "Incorrect y-coordinate!");
		Assert.True (display.transform.GetChild(1).position.x == 0, "Incorrect x-coordinate!");
		Assert.True (display.transform.GetChild(1).position.z == 0, "Incorrect y-coordinate!");

		Assert.True (display.transform.childCount == 2, "No node was created!");		
	}

	[Test]
	public void POIDisplayDoesNotCreatePOIOutsideMapBounds() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (3, 0));
		nodeList.Add(new DisplayNode (-1, 0));
		nodeList.Add(new DisplayNode (0, 3));
		nodeList.Add(new DisplayNode (0, -1));

		display.DisplayPOINode (new DisplayNode(3,0), icon, colorHandler);
		display.DisplayPOINode (new DisplayNode(-1,0), icon, colorHandler);
		display.DisplayPOINode (new DisplayNode(0,3), icon, colorHandler);
		display.DisplayPOINode (new DisplayNode(0,-1), icon, colorHandler);
		Assert.True (display.transform.childCount == 0, "created false nodes!");
	}
	
}
