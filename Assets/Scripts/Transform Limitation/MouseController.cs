using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {


	public GameObject camController;

	private static float xSpeed = 20.0f;
	private static float ySpeed = 18.0f;

	private float x, y;
	private bool flag;

	Transform transform, tarTransform;

	void Start()
	{
		transform = camController.transform;
		y = transform.position.x;
		x = transform.position.y;
	}
	void Update()
	{
		if (Input.GetMouseButton (0)) {
			flag = true;
			x += Input.GetAxis ("Mouse X") * xSpeed;
			y += Input.GetAxis ("Mouse Y") * ySpeed;
		} else {
			flag = false;
			x = 0;
			y = 0;
		}
	}
	void LateUpdate()
	{
		if (flag) {
			Quaternion rotation = Quaternion.Euler (y, x, 0);
			transform.rotation = rotation;
		} else {
			Quaternion rotation = Quaternion.Euler (0, 0, 0);
			transform.rotation = rotation;
		}
	}

}
