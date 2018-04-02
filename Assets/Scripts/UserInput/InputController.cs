﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public Transform target;
    public Rotate rotate;
    public Zoom zoom;
    public Transform cameraTrans;

    void Start() {
        SwipeManager.OnSwipeDetected   += OnSwipeDetected;
        SwipeManager.OnGestureDetected += OnGestureDetected;
        SwipeManager.OnTapDetected     += OnTapDetected;
    }

    void OnTapDetected(Vector3 tapPosition) {
        Debug.Log("OnTapDetected");
        // Move map center here
        throw new NotImplementedException();
    }

    void OnGestureDetected(Gesture gestureType, float value) {
        // Rotate / Zoom here
        if(gestureType == Gesture.Pinch) {
            Debug.Log("Pinch");
            int zoomValue = (int)Mathf.Clamp(value, -1, 1);
            Debug.Log(zoomValue);
            zoom.ZoomTarget(zoomValue);
        }

       if(gestureType == Gesture.Rotate) {
            Debug.Log("Rotate");
            rotate.RotateTarget(value * Time.deltaTime);
        }
    }

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity) {
        Debug.Log("OnSwipeDetected");
        // Tilt Camera here
        Vector3 currentRotation = cameraTrans.eulerAngles;
        switch (direction) {
            case Swipe.Down:
                cameraTrans.Rotate(Vector3.left * Mathf.Abs(swipeVelocity.y) * Time.deltaTime);
                //cameraTrans.eulerAngles = new Vector3(currentRotation.x + swipeVelocity.y, currentRotation.y, currentRotation.z);
                break;
            case Swipe.Up:
                cameraTrans.Rotate(Vector3.right * Mathf.Abs(swipeVelocity.y) * Time.deltaTime);
                //cameraTrans.eulerAngles = new Vector3(currentRotation.x - swipeVelocity.y, currentRotation.y, currentRotation.z);
                break;
            case Swipe.Left:
                cameraTrans.Rotate(Vector3.up * Mathf.Abs(swipeVelocity.x) * Time.deltaTime);
                //cameraTrans.eulerAngles = new Vector3(currentRotation.x, currentRotation.y + swipeVelocity.x, currentRotation.z);
                break;
            case Swipe.Right:
                cameraTrans.Rotate(Vector3.down * Mathf.Abs(swipeVelocity.x) * Time.deltaTime);
                //cameraTrans.eulerAngles = new Vector3(currentRotation.x, currentRotation.y - swipeVelocity.x, currentRotation.z);
                break;
            default:
                break;
        }
        
    }
}
