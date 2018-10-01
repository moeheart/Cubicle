using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraUsingButton : MonoBehaviour {

	[SerializeField]
	public ControlButton xUpButton, xDownButton, clockwiseYButton, counterClockwiseYButton;

	private float xRotationPerFrame = 0.5f, yRotationPerFrame = 0.5f;

	private ViewType lastViewType = ViewType.None;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float xRotation, yRotation;
		if (xUpButton.IsPressed()) {
			xRotation = xRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "xUp Button Pressed");
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, xRotation);
			}
		}
		if (xDownButton.IsPressed()) {
			xRotation = xRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "xDown Button Pressed");
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, -xRotation);
			}
		}
		if (clockwiseYButton.IsPressed()) {
			yRotation = yRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "Clockwise Button Pressed");
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				this.transform.Rotate(Vector3.up, yRotation);
			}
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				this.transform.Rotate(Vector3.forward, -yRotation);
			}
		}
		if (counterClockwiseYButton.IsPressed()) {
			yRotation = yRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			BlockBuilderLog.Log(BlockBuilderManager.currentLevelId, "counterClockwise Button Pressed");
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				this.transform.Rotate(Vector3.up, -yRotation);
			}
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				this.transform.Rotate(Vector3.forward, yRotation);
			}
		}
		ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
		LogCurrentPosition();
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
