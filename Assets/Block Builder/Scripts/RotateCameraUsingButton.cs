using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraUsingButton : MonoBehaviour {

	[SerializeField]
	public ControlButton xUpButton, xDownButton, clockwiseYButton, counterClockwiseYButton;

	private float xRotationPerFrame = 0.3f, yRotationPerFrame = 0.3f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float xRotation, yRotation;
		if (xUpButton.IsPressed()) {
			xRotation = xRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, xRotation);
			}
		}
		if (xDownButton.IsPressed()) {
			xRotation = xRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, -xRotation);
			}
		}
		if (clockwiseYButton.IsPressed()) {
			yRotation = yRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				this.transform.Rotate(Vector3.up, yRotation);
			}
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				this.transform.Rotate(Vector3.forward, -yRotation);
			}
		}
		if (counterClockwiseYButton.IsPressed()) {
			yRotation = yRotationPerFrame * BlockBuilderConfigs.sensitivityGyro;
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				this.transform.Rotate(Vector3.up, -yRotation);
			}
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				this.transform.Rotate(Vector3.forward, yRotation);
			}
		}
		ViewUtil.PlaceCameraFromRotation(this.transform, BlockBuilderConfigs.distanceToBaseGrid);
	}


}
