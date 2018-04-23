using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing information about trail labels. Contains a list of possible label 
/// positions in Unity's coordinates as well as the label GameObject and name. 
/// </summary>

public class TrailLabel {
	public List<Vector3> unityPositions;
	public GameObject labelObject;
	public string name;

	public TrailLabel() {
		unityPositions = new List<Vector3> ();
	}

	public TrailLabel(List<Vector3> unityPositions, GameObject labelObject, string name) {
		this.unityPositions = unityPositions;
		this.labelObject = labelObject;
		this.name = name;
	}
}
