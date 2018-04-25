using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public Camera cam;
    public MapGenerator mapGenerator;

    public float perspectiveZoomSpeed = 0.5f;
    public float zoomMaxValue         = 100.0F;
    public float zoomMinValue         = 5.0F;
    private int lodUpdateCounter      = 0;
    public int lodUpdateInterval      = 5;

    private Vector2 combMem;
    private float origRot;
	private bool isGesture = false;
	public Transform targetTrans;
	private int currentZoomLevel = 0;

	public bool isTiltMode = false;
	private Quaternion cameraOriginalRotation;
	private Vector3 cameraStartPosition;

    void Start() {
        InputController.OnSwipeDetected   += OnSwipeDetected;
        InputController.OnInputStarted    += OnInputStarted;
        InputController.OnInput           += OnInput;
        InputController.OnInputEnded      += OnInputEnded;

		targetTrans = mapGenerator.gameObject.transform.parent.transform;
		cameraOriginalRotation = cam.transform.rotation;
		cameraStartPosition = cam.transform.position;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ZoomCamera(Input.GetAxis("Mouse ScrollWheel") * 100);
        }

		isGesture = Input.touchCount == 0 ? false : isGesture;
    }

    public void OnInputEnded(List<InputData> inputs) {
        UpdateLod();
		if (inputs.Count == 0)
			isGesture = false;
    }

    public void OnInput(List<InputData> inputs) {
        if(inputs.Count > 1) {
            if(IsRotate(inputs))
                RotateObject(inputs);
            if (IsPinch(inputs))
                HandleZoom(inputs);
        }
    }

    public void OnInputStarted(List<InputData> inputs) {
		isGesture = false;
        if (inputs.Count > 1) {
            combMem = CalculateTouchToTouchVec(inputs);
			origRot = targetTrans.rotation.eulerAngles.y;
        }
    }

    private void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity) {
		if (isTiltMode) {
			swipeVelocity = new Vector2 (Mathf.Clamp (swipeVelocity.x , -50, 50), Mathf.Clamp (swipeVelocity.y , -50, 50));
			Vector3 originalEulers = cam.transform.eulerAngles;
			cam.transform.Rotate(Vector3.right, swipeVelocity.y * Time.deltaTime * Mathf.Max(1, currentZoomLevel - mapGenerator.maxZoomValue));
			cam.transform.Rotate(Vector3.down, swipeVelocity.x * Time.deltaTime * Mathf.Max(1, currentZoomLevel - mapGenerator.maxZoomValue));
			cam.transform.eulerAngles = new Vector3 (cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, originalEulers.z);
			UpdateLod();
			return;
		}

		if (isGesture)
			return;
		
		float speed = 0.05F * Math.Max(((cam.fieldOfView - zoomMinValue) / (zoomMaxValue - zoomMinValue)), 0.05F);
        float max_x = 0.5F;
        float min_x = -0.5F;
        float max_y = 0.5F;
        float min_y = -0.5F;
		Vector2 newPos = new Vector2 (mapGenerator.gameObject.transform.position.x, mapGenerator.gameObject.transform.position.z);
        newPos.x = Mathf.Clamp(newPos.x + (swipeVelocity.x * Time.deltaTime * speed), min_x, max_x);
        newPos.y = Mathf.Clamp(newPos.y + (swipeVelocity.y * Time.deltaTime * speed), min_y, max_y);
		mapGenerator.gameObject.transform.position = new Vector3(newPos.x, 0, newPos.y);
		mapGenerator.mapViewerPosition = new Vector2 (mapGenerator.gameObject.transform.position.x, mapGenerator.gameObject.transform.position.z);
        UpdateLod();
    }

    private void HandleZoom(List<InputData> inputs)
    {
		isGesture = true;
        Vector2 touchZeroPrevPos = inputs[0].prevPosition;
        Vector2 touchOnePrevPos = inputs[1].prevPosition;
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (inputs[0].currentPosition - inputs[1].currentPosition).magnitude;
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
        ZoomCamera(deltaMagnitudeDiff);
    }

    private void ZoomCamera(float deltaMagnitudeDiff)
    {
        cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomMinValue, zoomMaxValue);
		currentZoomLevel = (int) (mapGenerator.maxZoomValue - ((cam.fieldOfView - 5) / 95) * mapGenerator.maxZoomValue);
		float step = Vector3.Distance(cameraStartPosition, Vector3.zero) / mapGenerator.maxZoomValue;
		Vector3 targetPos = deltaMagnitudeDiff < 0 ? new Vector3(0,0.15F,-0.15F) : cameraStartPosition;
		cam.transform.position = Vector3.MoveTowards (cam.transform.position, targetPos, step);
		if(mapGenerator != null) mapGenerator.UpdateZoomLevel(currentZoomLevel);
        cam.transform.LookAt(targetTrans);
    }

    private Vector2 CalculateTouchToTouchVec(List<InputData> inputs) {
        Vector2 screenPosFir = ToViewportPoint(inputs[0].currentPosition);
        Vector2 screenPosSec = ToViewportPoint(inputs[1].currentPosition);
        return screenPosSec - screenPosFir;
    }

    private bool IsPinch(List<InputData> inputs) {
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
			return false;
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
		isGesture = true;
		targetTrans.eulerAngles = new Vector3(
									targetTrans.eulerAngles.x,
                                    origRot + (Vector2.SignedAngle(CalculateTouchToTouchVec(inputs), combMem)),
									targetTrans.eulerAngles.z
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
