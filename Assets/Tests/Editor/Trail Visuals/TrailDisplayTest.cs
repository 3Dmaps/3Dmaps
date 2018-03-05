using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class TrailDisplayTest {

	public MapData mapdata;
	public TrailDisplay display;

	[SetUp]
	public void Setup() {
		GameObject nodeGameObject = new GameObject ();

		GameObject trailDisplayObject = new GameObject ();

		display = trailDisplayObject.AddComponent<TrailDisplay> ();
		display.nodeGameObject = nodeGameObject;

		MapMetadata metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
		display.mapData = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
	}

	[Test]
	public void TrailObjectIsCreated() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (-1, -1));
	 	display.DisplayNodes (nodeList);
	 	Assert.True (display.transform.childCount == 1, "Didn't create one trail.");		 
	}

	[Test]
	public void TwoTrailObjectsAreCreated() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (1, 1));
	 	display.DisplayNodes (nodeList);
		nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (1, 1));
	 	display.DisplayNodes (nodeList);		
	 	Assert.True (display.transform.childCount == 2, "Didn't create two trails.");		 
	}


	[Test]
	public void TrailPartlyOutsideOfMapBoundsIsCreated() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 100));
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 100));
		display.DisplayNodes (nodeList);
		Assert.True (display.transform.childCount == 1, "Trail with a node outside of map boundaries wasn't created.");
	}

	[Test]
	public void TrailDisplayDoesNotCreateTrailOutsideMapBounds() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (2, 0));
		nodeList.Add(new DisplayNode (-2, 0));
		nodeList.Add(new DisplayNode (0, 2));
		nodeList.Add(new DisplayNode (0, -2));

		display.DisplayNodes (nodeList);
		Assert.True (display.transform.childCount == 0, "Trail created out of bounds!");
	}





	// Tests for individual node game objects

	// [Test]
	// public void TrailDisplayCanCreateNewNodeInCenter() {
	// 	List<DisplayNode> nodeList = new List<DisplayNode> ();
	// 	nodeList.Add(new DisplayNode (0, 0));

	// 	display.DisplayNodes (nodeList);		
	// 	Assert.True (display.transform.GetChild(0).position.x == 0, "Incorrect x-coordinate!");
	// 	Assert.True (display.transform.GetChild(0).position.z == 0, "Incorrect y-coordinate!");

	// 	Assert.True (display.transform.childCount == 1, "No node was created!");
		
	// }

	// [Test]
	// public void TrailDisplayCanCreateNewNodesInCorners() {
	// 	List<DisplayNode> nodeList = new List<DisplayNode> ();
	// 	nodeList.Add(new DisplayNode (-1, 1));
	// 	nodeList.Add(new DisplayNode (1, -1));
	// 	nodeList.Add(new DisplayNode (1, 1));
	// 	nodeList.Add(new DisplayNode (-1, -1));

	// 	display.DisplayNodes (nodeList);
	// 	Debug.Log(display.transform.childCount);

	// 	Assert.True (display.transform.childCount == 5, "Wrong number of nodes created!");
	// }

	// [Test]
	// public void TrailDisplayCannotCreateNodeOutsideMapBounds() {
	// 	List<DisplayNode> nodeList = new List<DisplayNode> ();
	// 	nodeList.Add(new DisplayNode (2, 0));
	// 	nodeList.Add(new DisplayNode (-2, 0));
	// 	nodeList.Add(new DisplayNode (0, 2));
	// 	nodeList.Add(new DisplayNode (0, -2));


	// 	display.DisplayNodes (nodeList);
	// 	Assert.True (display.transform.childCount == 1, "Node created out of bounds!");
	// }
}
