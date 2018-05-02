using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for storing information about trail labels. Contains a list of possible label 
/// positions in Unity's coordinates as well as the label GameObject and name. LabelState
/// is used to store information about if and how the label is currently being displayed.
/// </summary>

public class TrailLabel {
	public List<Vector3> unityPositions;
	public GameObject labelObject;
	public string name;
	public LabelState state;

	public TrailLabel() {
		unityPositions = new List<Vector3> ();
	}

	public TrailLabel(List<Vector3> unityPositions, GameObject labelObject, string name) {
		this.unityPositions = unityPositions;
		this.labelObject = labelObject;
		this.name = name;
		this.state = LabelState.outOfSight;
	}
}

public enum LabelState {visible, fadingOut, outOfSight, fadingIn};