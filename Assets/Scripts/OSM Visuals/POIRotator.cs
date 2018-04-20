using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime script for turning POI icons towards currently active camera.
/// </summary>

public class POIRotator : MonoBehaviour {

	void Update () {
		transform.LookAt (2 * transform.position - Camera.main.transform.position, Camera.main.transform.rotation * Vector3.up);
	}
}
