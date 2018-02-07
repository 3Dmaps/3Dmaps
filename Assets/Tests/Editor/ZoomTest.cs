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
        zoomer = zoomerObject.AddComponent<Zoom>();
        target.transform.localScale = new Vector3(1f, 1f, 1f);
        zoomer.target = target;
        zoomer.zoomInmultiplier = 0.5F;
        zoomer.zoomOutmultiplier = 2.0F;
    }


	[Test]
	public void ZoomTestChangesScaleOfTarget() {		
		zoomer.ZoomTarget (1);
        Assert.True (Mathf.Approximately(target.transform.localScale.x, 2.0f));
        zoomer.ZoomTarget(-1);
        Assert.True(Mathf.Approximately(target.transform.localScale.x, 1.0f));
    }

    [Test]
    public void NoZoomsPastZoomLimit() {
        for (int i = 0; i < 10; i++)
        {
            zoomer.ZoomTarget(1);
        }
        Assert.True(Mathf.Approximately(target.transform.localScale.x, 32.0F));

        for (int i = 0; i < 10; i++)
        {
            zoomer.ZoomTarget(-1);
        }
        Assert.True(Mathf.Approximately(target.transform.localScale.x, 0.03125F));
    }
}
