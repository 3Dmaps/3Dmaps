using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class PoiClickDetectorTest {

	public POIDisplay display;
	public Icon icon;
	public string name;

	public GameObject nodeGameObject;
	public GameObject nodeGameObject2;

	[SetUp]
	public void Setup() {
		GameObject POIDisplayObject = new GameObject ();
		display = POIDisplayObject.AddComponent<POIDisplay> ();

		nodeGameObject = new GameObject ();
		nodeGameObject.AddComponent<MeshRenderer> ().enabled = false;
		nodeGameObject.AddComponent<POIClickDetector> ().poiDisplay = display;

		nodeGameObject2 = new GameObject ();
		nodeGameObject2.AddComponent<MeshRenderer> ().enabled = false;
		nodeGameObject2.AddComponent<POIClickDetector> ().poiDisplay = display;

		display.nodeGameObject = nodeGameObject;
	}

	[Test]
	public void ClickTurnsPOILabelOn() {
		nodeGameObject.GetComponent<POIClickDetector> ().handleClick ();
		Assert.True (nodeGameObject.GetComponent<MeshRenderer> ().enabled == true, "The label was not turned on!");
	}

	[Test]
	public void ClickingSecondTimeTurnsPOILabelOff() {
		nodeGameObject.GetComponent<POIClickDetector> ().handleClick ();
		nodeGameObject.GetComponent<POIClickDetector> ().handleClick ();
		Assert.True (nodeGameObject.GetComponent<MeshRenderer> ().enabled == false, "The label was still on!");
	}


	[Test]
	public void OnlyOnePOILabelEnabledAtOnce() {
		nodeGameObject.GetComponent<POIClickDetector> ().handleClick ();
		nodeGameObject2.GetComponent<POIClickDetector> ().handleClick ();
		Assert.True (nodeGameObject.GetComponent<MeshRenderer> ().enabled == false, "The earlier label was not turned off!");
		Assert.True (nodeGameObject2.GetComponent<MeshRenderer> ().enabled == true, "The latter label was not turned on!");
	}
}