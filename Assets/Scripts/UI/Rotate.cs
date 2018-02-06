using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
/// This class rotate target object by the turningSpeed  
/// </summary>

public class Rotate : MonoBehaviour {
	public GameObject target;
	public bool isPressed = false;
    public float turningSpeed = 30f;

	
	// Update is called once per frame
	void Update () {
		if (isPressed) {
			RotateTarget (turningSpeed * Time.deltaTime);
		}
	
	}

	public void RotateTarget(float amount) {
		target.transform.Rotate(Vector3.up, amount);
	}

	///<summary>
	/// EventTrigger calls onPointerDown and OnPointerUp  
	/// </summary>
	public void OnPointerDown() {
		isPressed = true;
	}
	public void OnPointerUp() {
		isPressed = false;
	}
}
