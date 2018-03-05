using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewEditModeTest {
	Rotate rotator;
	[Test]
	public void NewEditModeTestSimplePasses() {
		
		GameObject target = new GameObject ();
		GameObject rotatorObject = new GameObject ();
		rotatorObject.AddComponent<Rotate> ();
		rotator = rotatorObject.GetComponent<Rotate> ();
		rotator.target = target;
		rotator.RotateTarget (30f);
		Assert.True (Mathf.Approximately(target.transform.eulerAngles.y, 30f));


	
	}

}
