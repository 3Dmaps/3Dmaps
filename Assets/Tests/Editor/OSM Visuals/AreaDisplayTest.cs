using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class AreaDisplayTest {

	public MapData mapData;
	public AreaDisplay display;



	[SetUp]
	public void Setup() {
		
		GameObject areaDisplay = new GameObject ();
		

		display = areaDisplay.AddComponent<AreaDisplay> ();
        List<Area> areas = new List<Area>();
        List<OSMNode> osmNode = new List<OSMNode>();

        // areas.Add(new Area(osmNode, ));
		// display.SetAreas();
		
		
        
		MapMetadata metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
		mapData = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
	}
	// Tests for individual node game objects
}