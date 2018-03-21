using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
		Camera.main.transform.rotation * Vector3.up);
	}
}
