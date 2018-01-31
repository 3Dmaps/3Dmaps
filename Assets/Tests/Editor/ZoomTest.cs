using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ZoomTest {
	Zoom zoomer;
	[Test]
	public void ZoomTestChangesScaleOfTarget() {
		GameObject target = new GameObject();
		GameObject zoomerObject = new GameObject();
		zoomerObject.AddComponent<Zoom> ();
		zoomer = zoomerObject.GetComponent<Zoom> ();
		target.transform.localScale = new Vector3(10f,10f,10f);
		zoomer.target = target;
		zoomer.multiplier = 2f;
		zoomer.ZoomTarget ();
		Assert.True (Mathf.Approximately(target.transform.localScale.x, 20f));


	}

}
