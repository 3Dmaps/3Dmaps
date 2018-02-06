using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ZoomTest {
	Zoom zoomer;
    GameObject target;
    GameObject zoomerObject;


    [SetUp]
    public void SetUp() {
        target = new GameObject();
        zoomerObject = new GameObject();        
        zoomerObject.AddComponent<Zoom>();
        zoomer = zoomerObject.GetComponent<Zoom>();
        target.transform.localScale = new Vector3(10f, 10f, 10f);
        zoomer.target = target;
        zoomer.multiplier = 2f;        
    }


	[Test]
	public void ZoomTestChangesScaleOfTarget() {		
		zoomer.ZoomTarget ();        
		Assert.True (Mathf.Approximately(target.transform.localScale.x, 20f));
    }

    [Test]
    public void NoZoomsPastZoomLimit() { 
        zoomer.zoomLimit = 1;
        zoomer.ZoomTarget();
        zoomer.ZoomTarget();        
        Assert.True(Mathf.Approximately(target.transform.localScale.x, 20f));
    }

    [TearDown]
    public void CleanUp() {
        Zoom.ZoomLevel = 0;
    }

    


}
