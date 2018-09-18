﻿// Move by tapping on the ground in the scenery or with the joystick in the lower left corner.
// Look around by dragging the screen.

// These controls are a simple version of the navigation controls in Epic's Epic Citadel demo.

// The ground object  must be on the layer 8 (I call it Ground layer).

// Attach this script to a character controller game object. That game object must
// also have a child object with the main camera attached to it.


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (CharacterController))]
public class iOSPlayerTouchInput : MonoBehaviour {

	public Joystick joystick;
	public bool kInverse = false;
	public bool pressedRightFinger = false;
	public Vector2 inputDirection;
	public static float sensitivityHor = 9.0f;
	public static float sensitivityVert = 9.0f;
	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;

	public static float touchSensitivityHor = 0.04f;
	public static float touchSensitivityVert = 0.04f;
	
	private Transform cameraTransform;
	private Camera mainCamera;
	private CharacterController charController;
	private int leftFingerId = -1;
	private int rightFingerId = -1;

	private Vector2 leftFingerStartPoint;
	private Vector2 leftFingerCurrentPoint;
	private Vector2 rightFingerStartPoint;
	public Vector2 rightFingerCurrentPoint {get; private set;}
	private Vector2 rightFingerLastPoint;
	private bool isRotating;

	void Start() {
		mainCamera = Camera.main;
		cameraTransform = Camera.main.transform;
		charController = this.GetComponent<CharacterController>();
		pressedRightFinger = false;
	}

	void Update() {
	}

	public void TrackChanges() {
		pressedRightFinger = false;
		if (Application.isEditor)
		{
			if (Input.GetMouseButtonDown(0))
				OnTouchBegan(0, Input.mousePosition);
			else if (Input.GetMouseButtonUp(0))
				OnTouchEnded(0);
			else if (leftFingerId != -1 || rightFingerId != -1)
				OnTouchMoved(0, Input.mousePosition);
		}
		else
		{
			int count = Input.touchCount;
 
			for (int i = 0;  i < count;  i++) 
			{	
				Touch touch = Input.GetTouch (i);
 
				if (touch.phase == TouchPhase.Began)
					OnTouchBegan(touch.fingerId, touch.position);
				else if (touch.phase == TouchPhase.Moved)
					OnTouchMoved(touch.fingerId, touch.position);
				else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
					OnTouchEnded(touch.fingerId);
			}
		}
		inputDirection = joystick.inputDirection;
		/*
		if (leftFingerId != -1)
			MoveFromJoystick();
		else if (isMovingToTarget)
			MoveToTarget();
		*/
 
		if (rightFingerId != -1 && isRotating)
			Rotate();

		RotateFromAccelerometer();
	}

	private void OnTouchBegan(int fingerId, Vector2 pos) {
		if (leftFingerId == -1 && joystick.IsPositionInContainer(pos)) {
			leftFingerId = fingerId;
			leftFingerStartPoint = leftFingerCurrentPoint = pos;
		}
		else if (rightFingerId == -1) {
			rightFingerStartPoint = rightFingerCurrentPoint = rightFingerLastPoint = pos;
			rightFingerId = fingerId;
			isRotating = false;
		}
	}

	private void OnTouchEnded(int fingerId) {
		if (fingerId == leftFingerId) {
			leftFingerId = -1;
		}
		else if (fingerId == rightFingerId) {
			rightFingerId = -1;
			if (isRotating == false) {
				pressedRightFinger = true;
			}
		}
	}

	private void OnTouchMoved(int fingerId, Vector2 pos) {
		if (fingerId == leftFingerId) {
			leftFingerCurrentPoint = pos;
		}
		else if (fingerId == rightFingerId) {
			rightFingerCurrentPoint = pos;
			if ((pos - rightFingerStartPoint).magnitude > 2) {
				isRotating = true;
			}
		}
	}

	private void RotateFromVector2D(Vector2 vec) {
		if (WorldCanvas.isMenuActive) {
			return;
		}
		float X = vec.x;
		float Y = vec.y;
      	transform.Rotate(0, X, 0);

		//Debug.Log(Input.acceleration);
		
		float rotationX;
		rotationX = cameraTransform.localEulerAngles.x - Y;
		//Debug.Log(rotationX);
		if (rotationX > 180) {
			rotationX -= 360;
		}
		rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

		cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
	}

	private void RotateFromAccelerometer() {
		float X = -Input.gyro.rotationRateUnbiased.y * sensitivityHor;
		float Y = Input.gyro.rotationRateUnbiased.x * sensitivityVert;
		float Z = Input.gyro.rotationRateUnbiased.z;
		RotateFromVector2D(new Vector2(X,Y));

		/* transform.Rotate(0, X * sensitivityHor, 0);

		//Debug.Log(Input.acceleration);
		
		float rotationX;
		rotationX = cameraTransform.localEulerAngles.x - Y * sensitivityVert;
		//Debug.Log(rotationX);
		if (rotationX > 180) {
			rotationX -= 360;
		}
		rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

		cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0); */
	}

	void Rotate()
	{
		Vector2 vec = rightFingerLastPoint - rightFingerCurrentPoint;
		vec.x *= touchSensitivityHor;
		vec.y *= touchSensitivityVert;
		RotateFromVector2D(vec);
		/*
		Vector3 lastDirectionInGlobal = mainCamera.ScreenPointToRay(rightFingerLastPoint).direction;
		Vector3 currentDirectionInGlobal = mainCamera.ScreenPointToRay(rightFingerCurrentPoint).direction;

		Quaternion rotation = new Quaternion();
		rotation.SetFromToRotation(lastDirectionInGlobal, currentDirectionInGlobal);

		transform.rotation = transform.rotation * Quaternion.Euler(0, kInverse ? rotation.eulerAngles.y : -rotation.eulerAngles.y, 0);

		// and now the rotation in the camera's local space
		rotation.SetFromToRotation(cameraTransform.InverseTransformDirection(lastDirectionInGlobal),
			cameraTransform.InverseTransformDirection(currentDirectionInGlobal));
		cameraTransform.localRotation = Quaternion.Euler(kInverse ? rotation.eulerAngles.x : -rotation.eulerAngles.x, 0, 0) * cameraTransform.localRotation;
		*/
		rightFingerLastPoint = rightFingerCurrentPoint;
	}
}