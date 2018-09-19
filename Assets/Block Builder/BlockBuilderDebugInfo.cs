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
		float X = Input.gyro.rotationRateUnbiased.x * sensitivityGyroX;
		float Y = Input.gyro.rotationRateUnbiased.y * sensitivityGyroY;
		float Z = Input.gyro.rotationRateUnbiased.z * sensitivityGyroZ;
		GUILayout.Label("Input.gyro.rotationRateUnbiased: " + new Vector3(X,Y,Z));

	}
}
