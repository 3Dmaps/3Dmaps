using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime script for turning POI icons and labels towards currently active camera.
/// </summary>

public class ObjectRotator : MonoBehaviour {

	void Update () {

		/*Quaternion quaternion = new Quaternion ();
		quaternion.x = Camera.main.transform.rotation.x;
		quaternion.y = transform.position.x - transform.localPosition.x;

		transform.rotation = quaternion;*/

		transform.LookAt(transform.position - Camera.main.transform.rotation * Vector3.back,
		Camera.main.transform.rotation * Vector3.up);

		/*transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
			Camera.main.transform.rotation * Vector3.up);*/

		//transform.LookAt (Camera.main.transform.position - transform.position, Camera.main.transform.rotation * Vector3.up);

		/*transform.LookAt(2 * transform.position - Camera.main.transform.position + Camera.main.transform.rotation * Vector3.back,
			Camera.main.transform.rotation * Vector3.up);*/
		

		//transform.LookAt (2 * transform.position - Camera.main.transform.position, Camera.main.transform.rotation * new Vector3(0, 1, 0));
	}
}
