using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {


	public float rotateSpeed = 200;

	private Quaternion originalRotation;

	private Button snapButton;

	private float sensitivityGyroX = 9.0f;

	private float sensitivityGyroY = 9.0f;

	private float sensitivityGyroZ = 9.0f;

	// Use this for initialization
	void Start () {
		originalRotation = transform.localRotation;
		snapButton = GameObject.Find("Snap Button").GetComponent<Button>();
		snapButton.onClick.AddListener(SnapBack);
	}
	
	// Update is called once per frame
	void Update () {

		if (Application.isEditor) {

			if (Input.GetMouseButton(0)) {
				//Debug.Log("Dragging");
				float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
				float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;

				transform.Rotate(Vector3.up, -rotX);
				transform.Rotate(Vector3.right, rotY);
			}

			RotateInTabletMode();
		}

		else {
			RotateInTabletMode();
		}
	}

	private void RotateInTabletMode() {
		RotateFromDrag();
		RotateFromGyro();
		RotateFromVector2D(new Vector2(0f, 0f));
	}

	private void RotateFromDrag() {
	}

	private void RotateFromGyro() {
		Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
		float X = Input.gyro.rotationRateUnbiased.x * sensitivityGyroX;
		float Y = Input.gyro.rotationRateUnbiased.y * sensitivityGyroY;
		float Z = Input.gyro.rotationRateUnbiased.z * sensitivityGyroZ;
		Vector3 rotateSpeed = new Vector3(-X, Z, Y);
		Debug.Log(rotateSpeed);

		float XX = Mathf.Abs(X);
		float YY = Mathf.Abs(Y);
		float ZZ = Mathf.Abs(Z);
		if (XX > YY && XX > ZZ) {
			Debug.Log("XX axis....!!!");
		}
		else if (YY>XX && YY > ZZ) {
			Debug.Log("YY axis....!!!");
		}
		else {
			Debug.Log("ZZ axis....!!!");
		}

		// rotateSpeed = new Vector3(0, 1, 0);

		transform.Rotate(rotateSpeed, Space.World);
		// RotateFromVector2D(new Vector2(X,Y));
	}

	private void RotateFromVector2D(Vector2 vec) {
		if (WorldCanvas.isMenuActive) {
			return;
		}
		transform.Rotate(Vector3.up, -vec.x);
		transform.Rotate(Vector3.right, vec.y);

		//Debug.Log(Input.acceleration);
		/*
		float rotationX;
		rotationX = transform.localEulerAngles.x - Y;
		//Debug.Log(rotationX);
		if (rotationX > 180) {
			rotationX -= 360;
		}

		transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		*/
	}

	public void SnapBack() {
		this.transform.localRotation = originalRotation;
	}
}
