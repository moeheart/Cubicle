using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointCameraController : MonoBehaviour {

	public GameObject controller;
	public int camDir;

	void Awake () {

//		camDir = 0;
		camDir = Random.Range (0, 8);
		controller.transform.eulerAngles = new Vector3 (0, camDir * 45, 0);
//		print (camDir);

	}

}
