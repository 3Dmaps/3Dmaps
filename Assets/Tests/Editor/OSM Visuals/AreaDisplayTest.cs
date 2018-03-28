using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class AreaDisplayTest {

	public AreaDisplay display;

	[SetUp]
	public void Setup() {
		
		GameObject areaDisplayObject = new GameObject ();
		display = areaDisplayObject.AddComponent<AreaDisplay> ();
	}

	[Test]
	public void AreaDisplayProvidesCorrectColorForPointIsInsideArea() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 2));
		nodeList.Add(new DisplayNode (2, 2));
		nodeList.Add(new DisplayNode (2, 0));

		display.AddArea (Color.blue, nodeList);

		Assert.True (display.GetAreaColor (1, 1) == Color.blue, "Wrong color found at 1,1!");
	}

	[Test]
	public void AreaDisplayReturnsBlackWhenPointIsOutsideArea() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 1));
		nodeList.Add(new DisplayNode (1, 1));
		nodeList.Add(new DisplayNode (1, 0));

		display.AddArea (Color.blue, nodeList);

		Assert.True (display.GetAreaColor (2, 2) == Color.black, "A color was erroneously found!");
	}

	[Test]
	public void AreaDisplayWorksWithMoreThanFourPolygons() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 1));
		nodeList.Add(new DisplayNode (2, 1));
		nodeList.Add(new DisplayNode (2, 2));
		nodeList.Add(new DisplayNode (1, 2));
		nodeList.Add(new DisplayNode (0, 1));

		display.AddArea (Color.blue, nodeList);

		Assert.True (display.GetAreaColor (1, 1) == Color.blue, "Wrong color found at 1,1!");
	}

	[Test]
	public void AreaDisplayCanHaveMultipleAreas() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (0, 0));
		nodeList.Add(new DisplayNode (0, 2));
		nodeList.Add(new DisplayNode (2, 2));
		nodeList.Add(new DisplayNode (2, 0));

		List<DisplayNode> nodeList2 = new List<DisplayNode> ();
		nodeList2.Add(new DisplayNode (3, 3));
		nodeList2.Add(new DisplayNode (3, 5));
		nodeList2.Add(new DisplayNode (5, 5));
		nodeList2.Add(new DisplayNode (5, 3));

		display.AddArea (Color.blue, nodeList);
		display.AddArea (Color.red, nodeList2);

		Assert.True (display.GetAreaColor (1, 1) == Color.blue, "Wrong color found at 1,1!");
		Assert.True (display.GetAreaColor (4, 4) == Color.red, "Wrong color found at 4,4!");
	}


}