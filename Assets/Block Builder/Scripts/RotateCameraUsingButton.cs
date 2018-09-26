using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraUsingButton : MonoBehaviour {

	[SerializeField]
	public ControlButton xUpButton, xDownButton, clockwiseYButton, counterClockwiseYButton;

	private float xRotation, yRotation;

	// Use this for initialization
	void Start () {
		xRotation = 2;
		yRotation = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (xUpButton.IsPressed()) {
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, xRotation);
			}
		}
		if (xDownButton.IsPressed()) {
			if (ViewUtil.canRotateAroundXAxis(transform)) {
				this.transform.Rotate(Vector3.right, -xRotation);
			}
		}
		if (clockwiseYButton.IsPressed()) {
			if (ViewUtil.canRotateAroundYAxis(transform)) {
				this.transform.Rotate(Vector3.up, yRotation);
			}
			if (ViewUtil.canRotateAroundZAxis(transform)) {
				this.transform.Rotate(Vector3.forward, -yRotation);
			}
		}
		if (counterClockwiseYButton.IsPressed()) {
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
