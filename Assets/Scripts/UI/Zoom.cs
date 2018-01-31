using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
/// This class changes the scale of target object by the multiplier  
/// </summary>
public class Zoom : MonoBehaviour {
	public GameObject target;
	public float multiplier = 0.8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ZoomTarget() {
		Vector3 scale = target.transform.localScale;
		scale = scale * multiplier;
		target.transform.localScale = scale;


	}
}
