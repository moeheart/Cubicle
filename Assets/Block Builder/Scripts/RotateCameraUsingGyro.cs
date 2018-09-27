using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCameraUsingGyro : MonoBehaviour {

	public float xRotation {get; private set;}
	public float yRotation {get; private set;}
	public float zRotation {get; private set;}

	private float sensitivityGyroX = 9.0f;
	private float sensitivityGyroY = 9.0f;
	private float sensitivityGyroZ = 9.0f;

	private ViewType lastViewType = ViewType.None;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		sensitivityGyroX = sensitivityGyroY = sensitivityGyroZ = BlockBuilderConfigs.sensitivityGyro;
		
		Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;
		xRotation = - Input.gyro.rotationRateUnbiased.x * sensitivityGyroX;
		yRotation = - Input.gyro.rotationRateUnbiased.y * sensitivityGyroY;
		zRotation = - Input.gyro.rotationRateUnbiased.z * sensitivityGyroZ;

		// Debug.Log(xRotation + " " + yRotation);

		// Determine which axis's rotation is most dominant
		float XX, YY, ZZ;
		XX = Mathf.Abs(xRotation);
		YY = Mathf.Abs(yRotation);
		ZZ = Mathf.Abs(zRotation);

		if (XX > YY && XX > ZZ) {
			this.transform.Rotate(Vector3.right, xRotation);
			ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
		}
		else if (YY > XX && YY > ZZ) {
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				//this.glueToZXAxis();
				this.transform.Rotate(Vector3.up, yRotation);
				ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
			}
		}
		else {
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				//this.glueToXYAxis();
				this.transform.Rotate(Vector3.forward, zRotation);
				ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
			}
		}

		LogCurrentPosition();

	}





	/* private void glueToYZAxis() {
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
	} */

	private float GetRotationFromButton(ControlButton plus, ControlButton minus) {
		if (plus.IsPressed()) {
			return 2;
		}
		else if (minus.IsPressed()) {
			return -2;
		}
		return 0;
	}

	public void SwitchToView(ViewType viewType) {
		this.transform.position = new Vector3(0, 0, -8);
		this.transform.LookAt(Vector3.zero);
		switch (viewType) {
			case ViewType.TopView:
				this.transform.Rotate(Vector3.right, 90);
				ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Clicked to switch to Top View");
				break;
			case ViewType.FrontView:
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Clicked to switch to Front View");
				break;
			case ViewType.RightView:
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Clicked to switch to Right View");
				this.transform.Rotate(Vector3.up, -90);
				ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
				break;
		}
	}

	private void LogCurrentPosition() {
		if (ViewUtil.IsAlignedWithFrontView(transform)) {
			if (lastViewType != ViewType.FrontView) {
				lastViewType = ViewType.FrontView;
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Transition to Front View");
			}
		}
		if (ViewUtil.IsAlignedWithTopView(transform)) {
			if (lastViewType != ViewType.TopView) {
				lastViewType = ViewType.TopView;
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Transition to Top View");
			}
			
		}
		if (ViewUtil.IsAlignedWithRightView(transform)) {
			if (lastViewType != ViewType.RightView) {
				lastViewType = ViewType.RightView;
				BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Transition to Right View");
			}

		}
	}

}
