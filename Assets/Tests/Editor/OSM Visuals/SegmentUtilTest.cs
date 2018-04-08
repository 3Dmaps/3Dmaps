using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SegmentUtilTest{

    [Test]
    public void FindDistanceToSegmentRiverTest() {
    DisplayNode dpnode = new DisplayNode(4,4);
    DisplayNode dpnode2 = new DisplayNode(4, 4);
    DisplayNode point = new DisplayNode(6,4);
    Assert.AreEqual(SegmentUtil.FindDistanceToSegmentRiver(point,dpnode,dpnode2),2);        
    dpnode = new DisplayNode(4,4);
    dpnode2 = new DisplayNode(6,6);
    point = new DisplayNode(6,6);
    Assert.AreEqual(SegmentUtil.FindDistanceToSegmentRiver(point,dpnode,dpnode2),(double) 0.0);
    dpnode = new DisplayNode(12,12);
    dpnode2 = new DisplayNode(18,18);
    point = new DisplayNode(6,16);
    Assert.AreEqual(SegmentUtil.FindDistanceToSegmentRiver(point,dpnode,dpnode2),7.211102550927978);
    dpnode = new DisplayNode(24,24);
    dpnode2 = new DisplayNode(18,18);
    point = new DisplayNode(6,16);
    Assert.AreEqual(SegmentUtil.FindDistanceToSegmentRiver(point,dpnode,dpnode2),12.165525060596439);             
    }
}