using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public Transform target;
    public Rotate rotate;
    public Zoom zoom;
    public Transform cameraTrans;
    public MapGenerator mapGenerator;
    private int lodUpdateCounter = 0;
    public int lodUpdateInterval = 5;

    void Start() {
        InputController.OnSwipeDetected   += OnSwipeDetected;
        InputController.OnGestureDetected += OnGestureDetected;
        InputController.OnTapDetected     += OnTapDetected;
    }

    void OnTapDetected(Vector3 tapPosition) {
        Debug.Log("OnTapDetected");
        throw new NotImplementedException();
        // Move map center here
    }

    void OnGestureDetected(Gesture gestureType, float value) {
        // Rotate / Zoom here
        if(gestureType == Gesture.Pinch) {
            Debug.Log("Pinch");
            // DOo zoom here
            /*
            int zoomValue = (int)Mathf.Clamp(value, -1, 1);
            Debug.Log(zoomValue);
            zoom.ZoomTarget(zoomValue);
            */
        }

       if(gestureType == Gesture.Rotate) {
            Debug.Log("Rotate");
            // Do Rotate here
            //rotate.RotateTarget(value * Time.deltaTime);
        }
    }

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity) {
        Debug.Log("OnSwipeDetected");
        float speed = 0.01F;
        float max_x = 0.5F;
        float min_x = -0.5F;
        float max_y = 0.5F;
        float min_y = -0.5F;
        Vector2 newPos = mapGenerator.mapViewerPosition;
        newPos.x = Mathf.Clamp(newPos.x + (swipeVelocity.x * Time.deltaTime * speed), min_x, max_x);
        newPos.y = Mathf.Clamp(newPos.y + (swipeVelocity.y * Time.deltaTime * speed), min_y, max_y);
        mapGenerator.mapViewerPosition = newPos;
        mapGenerator.gameObject.transform.position = new Vector3(mapGenerator.mapViewerPosition.x, 0, mapGenerator.mapViewerPosition.y);
        //Lisää joku checki et aina lopuks updatee lodin!
        if (mapGenerator != null && ++lodUpdateCounter > lodUpdateInterval) {
            mapGenerator.UpdateLOD();
            lodUpdateCounter = 0;
        }
    }
}
