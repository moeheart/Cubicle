using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCameraUsingGyro : MonoBehaviour {

	public float cosineSimilarityLowerBound = 0.94f;
	public ControlButton xPlus;
	public ControlButton xMinus;
	public ControlButton yPlus;
	public ControlButton yMinus;
	public ControlButton zPlus;
	public ControlButton zMinus;

	public float xRotation {get; private set;}
	public float yRotation {get; private set;}
	public float zRotation {get; private set;}

	private float sensitivityGyroX = 9.0f;
	private float sensitivityGyroY = 9.0f;
	private float sensitivityGyroZ = 9.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
		xRotation = - Input.gyro.rotationRateUnbiased.x * sensitivityGyroX;
		yRotation = Input.gyro.rotationRateUnbiased.y * sensitivityGyroY;
		zRotation = Input.gyro.rotationRateUnbiased.z * sensitivityGyroZ;

		if (Application.isEditor) {
			xRotation = GetRotationFromButton(xPlus, xMinus);
			yRotation = GetRotationFromButton(yPlus, yMinus);
			zRotation = GetRotationFromButton(zPlus, zMinus);
		}

		// Debug.Log(xRotation + " " + yRotation);

		// Determine which axis's rotation is most dominant
		float XX, YY, ZZ;
		XX = Mathf.Abs(xRotation);
		YY = Mathf.Abs(yRotation);
		ZZ = Mathf.Abs(zRotation);

		if (XX > YY && XX > ZZ) {
			if (canRotateAroundXAxis()) {
				//this.glueToYZAxis();
				this.transform.Rotate(Vector3.right, xRotation);
				PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
			}
		}
		else if (YY > XX && YY > ZZ) {
			if (canRotateAroundYAxis()) {
				//this.glueToZXAxis();
				this.transform.Rotate(Vector3.up, yRotation);
				PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
			}
		}
		else {
			if (canRotateAroundZAxis()) {
				//this.glueToXYAxis();
				this.transform.Rotate(Vector3.forward, zRotation);
				PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
			}
		}
	}

	private void PlaceCameraFromRotation(Transform transform, float dist) {
		Vector3 point = transform.position + transform.forward * dist;
		transform.position -= point;
	}

	public bool canRotateAroundXAxis() {
		float cos1 = Vector3.Dot(transform.right, Vector3.right);
		float cos2 = Vector3.Dot(transform.right, Vector3.forward);
		cos1 = Mathf.Abs(cos1);
		cos2 = Mathf.Abs(cos2);
		return (cos1 > cosineSimilarityLowerBound || cos2 > cosineSimilarityLowerBound);
	}

	public bool canRotateAroundYAxis() {
		float cos1 = Vector3.Dot(transform.up, Vector3.up);
		cos1 = Mathf.Abs(cos1);
		return (cos1 > cosineSimilarityLowerBound);
	}

	public bool canRotateAroundZAxis() {
		float cos1 = Vector3.Dot(transform.forward, Vector3.up);
		cos1 = Mathf.Abs(cos1);
		return (cos1 > cosineSimilarityLowerBound);
	}

	private void glueToYZAxis() {
		Vector3 newPosition = this.transform.position;
		newPosition.x = 0;
		newPosition = newPosition.normalized * 8;
		Vector3 right = transform.right;
		Vector3 forward = -transform.position;
		Vector3 up = -Vector3.Cross(right, forward);
		this.transform.LookAt(Vector3.zero, up);
		this.transform.position = newPosition;
	}

	private void glueToZXAxis() {
		Vector3 newPosition = this.transform.position;
		newPosition.y = 0;
		newPosition = newPosition.normalized * 8;
		this.transform.position = newPosition;
		this.transform.LookAt(BlockBuilderConfigs.baseGridWorldPosition);
	}

	private void glueToXYAxis() {
		Vector3 newPosition = this.transform.position;
		newPosition.z = 0;
		newPosition = newPosition.normalized * 8;
		this.transform.position = newPosition;
		this.transform.LookAt(BlockBuilderConfigs.baseGridWorldPosition);
	}

	private float GetRotationFromButton(ControlButton plus, ControlButton minus) {
		if (plus.IsPressed()) {
			return 2;
		}
		else if (minus.IsPressed()) {
			return -2;
		}
		return 0;
	}

}
