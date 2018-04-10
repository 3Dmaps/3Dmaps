using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class InputTest {
	
	[Test]
	public void InputHandlerTest() {
		GameObject obj = new GameObject ();
		GameObject child = new GameObject ();
		child.transform.parent = obj.transform;

		InputHandler handler = obj.AddComponent<InputHandler> ();
		MapGenerator mapGenerator = child.AddComponent<MapGenerator> ();
		handler.mapGenerator = mapGenerator;
		handler.target = obj.transform;
		Quaternion startRot = obj.transform.rotation;

		List<InputData> inputs = new List<InputData> ();
		InputData data = new InputData (1, Vector2.zero);
		inputs.Add (data);

		InputData data2 = new InputData (2, new Vector2(-10, -10));
		data2.phase = TouchPhase.Moved;
		data2.currentPosition = new Vector2 (5, -5);
		data2.prevPosition = new Vector2 (-5, -0);
		inputs.Add (data2);
		handler.OnInput (inputs);
		Assert.False (startRot == obj.transform.rotation);
	}

}
