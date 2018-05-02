using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Detects click when clicking poi ico
/// </summary>
public class POIClickDetector : MonoBehaviour {

	public POIDisplay poiDisplay;

	private void OnMouseDown() {
		HandleClick ();
	}

	public void HandleClick() {
		GameObject previousLabel = poiDisplay.currentVisibleLabel;
		if (previousLabel != null && previousLabel == this.gameObject) {
			this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			poiDisplay.currentVisibleLabel = null;
		} else {
			if (previousLabel != null) {
				previousLabel.GetComponentInChildren<MeshRenderer> ().enabled = false;
			}
			poiDisplay.currentVisibleLabel = this.gameObject;
			this.GetComponentInChildren<MeshRenderer> ().enabled = true;
		}
	}
}
