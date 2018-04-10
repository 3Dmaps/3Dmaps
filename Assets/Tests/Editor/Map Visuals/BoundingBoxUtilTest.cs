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
		expected.Add(new DisplayNode (1, 1));
		expected.Add(new DisplayNode (4, 4));        
        CollectionAssert.AreEqual(BoundingBoxUtil.BoundingBox(expected), BoundingBoxUtil.BoundingBox(nodeList));
    }
}    