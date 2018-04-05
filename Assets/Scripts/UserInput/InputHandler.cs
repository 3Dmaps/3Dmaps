using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public Transform target;
    public Camera cam;
    public MapGenerator mapGenerator;

    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed       = 0.5f;
    private int lodUpdateCounter      = 0;
    public int lodUpdateInterval      = 5;

    private Vector2 combMem;
    private float origRot;

    void Start() {
        InputController.OnSwipeDetected   += OnSwipeDetected;
        InputController.OnInputStarted    += OnInputStarted;
        InputController.OnInput           += OnInput;
        InputController.OnInputEnded      += OnInputEnded;
    }

    private void OnInputEnded(List<InputData> inputs) {
        UpdateLod();
    }

    private void OnInput(List<InputData> inputs) {
        if(inputs.Count > 1) {
            if(IsRotate(inputs))
                RotateObject(inputs);
            else if (IsPinch(inputs))
                HandleZoom(inputs);
        }
    }

    private void OnInputStarted(List<InputData> inputs) {
        if (inputs.Count > 1) {
            combMem = CalculateTouchToTouchVec(inputs);
            origRot = target.rotation.eulerAngles.y;
        }
    }

    private void HandleZoom(List<InputData> inputs) {
        Vector2 touchZeroPrevPos = inputs[0].prevPosition;
        Vector2 touchOnePrevPos  = inputs[1].prevPosition;
        float prevTouchDeltaMag  = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag      = (inputs[0].currentPosition - inputs[1].currentPosition).magnitude;
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        if (cam.orthographic) {
            cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);
        } else {
            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
        }

        UpdateLod();
    }

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity) {
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
        UpdateLod();
    }

    private Vector2 CalculateTouchToTouchVec(List<InputData> inputs) {
        Vector2 screenPosFir = ToViewportPoint(inputs[0].currentPosition);
        Vector2 screenPosSec = ToViewportPoint(inputs[1].currentPosition);
        return screenPosSec - screenPosFir;
    }

    private bool IsPinch(List<InputData> inputs) {
        float pinchDistance = Vector2.Distance(inputs[0].currentPosition, inputs[1].currentPosition);
        float prevDistance  = Vector2.Distance(inputs[0].prevPosition, inputs[1].prevPosition);
        float pinchDistanceDelta = Mathf.Abs(pinchDistance - prevDistance);
        return pinchDistanceDelta > 0;
    }

    private bool IsRotate(List<InputData> inputs) {
        float turnAngle = Angle(inputs[0].currentPosition, inputs[1].currentPosition);
        float prevTurn  = Angle(inputs[0].prevPosition, inputs[1].prevPosition);
        float turnAngleDelta = Mathf.Abs(Mathf.DeltaAngle(prevTurn, turnAngle));

        return turnAngleDelta > 0;
    }

    private void RotateObject(List<InputData> inputs) {
        target.eulerAngles = new Vector3(
                                    target.eulerAngles.x,
                                    origRot + (Vector2.SignedAngle(CalculateTouchToTouchVec(inputs), combMem)),
                                    target.eulerAngles.z
                                 );
        UpdateLod();
    }

    private void UpdateLod() {
        if (mapGenerator != null && ++lodUpdateCounter > lodUpdateInterval) {
            mapGenerator.UpdateLOD();
            lodUpdateCounter = 0;
        }
    }

    private float Angle(Vector2 pos1, Vector2 pos2) {
        Vector2 from  = pos2 - pos1;
        Vector2 to    = new Vector2(1, 0);
        float result  = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);
        if (cross.z > 0) result = 360f - result;       
        return result;
    }

    private Vector2 ToViewportPoint (Vector2 worldPos) {
        Vector3 viewPort = Camera.main.ScreenToViewportPoint(new Vector3(worldPos.x, worldPos.y, 0));
        return new Vector2(viewPort.x, viewPort.y);
    }

}
