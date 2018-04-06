using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class BoundingBoxUtilTest {
    
 

    [Test]
    public void TestBoundingBoxWithValues() {
		List<DisplayNode> nodeList = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (1, 1));
		nodeList.Add(new DisplayNode (2, 2));
        nodeList.Add(new DisplayNode (3, 3));
        nodeList.Add(new DisplayNode (4, 4));
        BoundingBoxUtil.BoundingBox(nodeList);
		List<DisplayNode> expected = new List<DisplayNode> ();
		nodeList.Add(new DisplayNode (1, 1));
		nodeList.Add(new DisplayNode (4, 4));
        // Assert.True (BoundingBoxUtil.BoundingBox(nodeList), "No node was created!");        
        CollectionAssert.AreEqual(expected, BoundingBoxUtil.BoundingBox(nodeList));
    }
}    