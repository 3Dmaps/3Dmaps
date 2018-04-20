using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIClickDetector : MonoBehaviour {
	private void OnMouseDown() {
		this.GetComponentInChildren<MeshRenderer>().enabled = !this.GetComponentInChildren<MeshRenderer>().enabled;
	}

}
