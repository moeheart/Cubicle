using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {


	public float rotateSpeed = 200;

	public float cosineSimilarityThreshold = 0.2f;

	private Quaternion originalRotation;

	private Button snapButton;

	private float sensitivityGyroX = 9.0f;

	private float sensitivityGyroY = 9.0f;

	private float sensitivityGyroZ = 9.0f;
	private Vector3 localXAxis, localYAxis, localZAxis;

	// Use this for initialization
	void Start () {
		originalRotation = transform.localRotation;
		snapButton = GameObject.Find("Snap Button").GetComponent<Button>();
		snapButton.onClick.AddListener(SnapBack);
		sensitivityGyroX = sensitivityGyroY = sensitivityGyroZ = BlockBuilderConfigs.sensitivityGyro;
	}
	
	// Update is called once per frame
	void Update () {

		sensitivityGyroX = sensitivityGyroY = sensitivityGyroZ = BlockBuilderConfigs.sensitivityGyro;

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
		// Quaternion difQuaternion = Input.gyro.attitude * Quaternion.Inverse(startingQuaternion);
		// Quaternion rotation = Quaternion.Euler(difQuaternion.eulerAngles.x, difQuaternion.eulerAngles.y, difQuaternion.eulerAngles.z);
		// this.transform.localRotation = difQuaternion * this.originalRotation;

		Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
		float X = Input.gyro.rotationRateUnbiased.x * sensitivityGyroX;
		float Y = Input.gyro.rotationRateUnbiased.z * sensitivityGyroY;
		float Z = Input.gyro.rotationRateUnbiased.y * sensitivityGyroZ;

		float XX, YY, ZZ;
		XX = Mathf.Abs(X);
		YY = Mathf.Abs(Y);
		ZZ = Mathf.Abs(Z);

		localZAxis = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
		localXAxis = transform.worldToLocalMatrix.MultiplyVector(transform.right);
		localYAxis = transform.worldToLocalMatrix.MultiplyVector(transform.up);

		// Y: Rotate around Local Axis
		// Z: Rotate around Local Axis
		// X: Rotate around World Axis??????
		if (XX > YY && XX > ZZ) {
			if (canRotateAroundXAxis()) {

			}
		}
		else if (YY > XX && YY > ZZ) {
			if (canRotateAroundYAxis()) {

			}
		}
		else {
			if (canRotateAroundZAxis()) {

			}
		}

		/*float XX = Mathf.Abs(X);
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
		}*/

		// rotateSpeed = new Vector3(0, 1, 0);

		// transform.Rotate(rotateSpeed, Space.Self);

		// transform.rotation = Input.gyro.attitude;
		/* Vector3 xAxis = new Vector3 (0, 1, 0);
		Vector3 yAxis = new Vector3 (1, 0, 0);
		Vector3 zAxis = new Vector3 (0, 0, 1);
		xAxis = Input.gyro.attitude * xAxis;
		yAxis = Input.gyro.attitude * yAxis;
		zAxis = Input.gyro.attitude * zAxis;

		transform.Rotate(zAxis, X, Space.Self);
		transform.Rotate(yAxis, Y, Space.Self);
		transform.Rotate(xAxis, Z, Space.Self); */
	}

	private bool canRotateAroundXAxis() {
		float cos1 = Vector3.Dot(localZAxis, Vector3.right);
		float cos2 = Vector3.Dot(localYAxis, Vector3.right);
		return (cos1 < cosineSimilarityThreshold && cos2 < cosineSimilarityThreshold);
	}

	private bool canRotateAroundYAxis() {
		float cos1 = Vector3.Dot(localZAxis, Vector3.up);
		float cos2 = Vector3.Dot(localXAxis, Vector3.up);
		return (cos1 < cosineSimilarityThreshold && cos2 < cosineSimilarityThreshold);
	}

	private bool canRotateAroundZAxis() {
		float cos1 = Vector3.Dot(localXAxis, Vector3.forward);
		float cos2 = Vector3.Dot(localYAxis, Vector3.forward);
		return (cos1 < cosineSimilarityThreshold && cos2 < cosineSimilarityThreshold);
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
