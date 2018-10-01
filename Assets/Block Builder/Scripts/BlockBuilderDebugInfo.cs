using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilderDebugInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected void OnGUI() {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);

        GUILayout.Label("Device width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);

		float sensitivityGyroX, sensitivityGyroY, sensitivityGyroZ;
		sensitivityGyroX = sensitivityGyroY = sensitivityGyroZ = BlockBuilderConfigs.sensitivityGyro;
		var rotationScript = Camera.main.GetComponent<RotateCameraUsingGyro>();
		float X = rotationScript.xRotation;
		float Y = rotationScript.yRotation;
		float Z = rotationScript.zRotation;

		GUILayout.Label("Input.gyro.rotationRateUnbiased: " + new Vector3(X,Y,Z));

		GUILayout.Label("cameraLocalXAxis: " + Camera.main.transform.right);
		GUILayout.Label("cameraLocalYAxis: " + Camera.main.transform.up);
		GUILayout.Label("cameraLocalZAxis: " + Camera.main.transform.forward);

		GUILayout.Label("canRotateAroundXAxis: " + ViewUtil.canRotateAroundXAxis(Camera.main.transform));
		GUILayout.Label("canRotateAroundYAxis: " + ViewUtil.canRotateAroundYAxis(Camera.main.transform));
		GUILayout.Label("canRotateAroundZAxis: " + ViewUtil.canRotateAroundZAxis(Camera.main.transform));
	}
}
