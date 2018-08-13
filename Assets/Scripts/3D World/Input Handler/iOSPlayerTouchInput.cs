// Move by tapping on the ground in the scenery or with the joystick in the lower left corner.
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
	public bool instructedJump = false;
	public Vector2 inputDirection;
	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;
	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;

	private float _rotationX = 0;
	
	private Transform cameraTransform;
	private Camera mainCamera;
	private CharacterController charController;
	private int leftFingerId = -1;
	private int rightFingerId = -1;

	private Vector2 leftFingerStartPoint;
	private Vector2 leftFingerCurrentPoint;
	private Vector2 rightFingerStartPoint;
	private Vector2 rightFingerCurrentPoint;
	private Vector2 rightFingerLastPoint;
	private bool isRotating;

	void Start() {
		mainCamera = Camera.main;
		cameraTransform = Camera.main.transform;
		charController = this.GetComponent<CharacterController>();
		instructedJump = false;
	}

	void Update() {
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
				instructedJump = true;
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

	private void RotateFromAccelerometer() {
		float X = Input.acceleration.x;
		float Y = Input.acceleration.y;
		float Z = Input.acceleration.z;

		X = 0.01f;

      	transform.Rotate(0, X * sensitivityHor, 0);
		
		_rotationX -= Y * sensitivityVert;
		_rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

		float rotationY = transform.localEulerAngles.y;

		cameraTransform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
	}

	void Rotate()
	{
		Vector3 lastDirectionInGlobal = mainCamera.ScreenPointToRay(rightFingerLastPoint).direction;
		Vector3 currentDirectionInGlobal = mainCamera.ScreenPointToRay(rightFingerCurrentPoint).direction;

		Quaternion rotation = new Quaternion();
		rotation.SetFromToRotation(lastDirectionInGlobal, currentDirectionInGlobal);

		transform.rotation = transform.rotation * Quaternion.Euler(0, kInverse ? rotation.eulerAngles.y : -rotation.eulerAngles.y, 0);

		// and now the rotation in the camera's local space
		rotation.SetFromToRotation(cameraTransform.InverseTransformDirection(lastDirectionInGlobal),
			cameraTransform.InverseTransformDirection(currentDirectionInGlobal));
		cameraTransform.localRotation = Quaternion.Euler(kInverse ? rotation.eulerAngles.x : -rotation.eulerAngles.x, 0, 0) * cameraTransform.localRotation;

		rightFingerLastPoint = rightFingerCurrentPoint;
	}
}
