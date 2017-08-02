using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {


	public float rotateSpeed = 200;

	private Quaternion originalRotation;

	// Use this for initialization
	void Start () {
		originalRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(0) == false)
			SnapBack();

		if (Input.GetMouseButton(0)) {
			//Debug.Log("Dragging");
			float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
			float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;

			transform.Rotate(Vector3.up, -rotX);
			transform.Rotate(Vector3.right, rotY);
		}
	
	}

	void SnapBack() {
		
		this.transform.localRotation = Quaternion.Slerp(transform.rotation, originalRotation, 15 * Time.deltaTime);
		//this.transform.localRotation = originalRotation;
	}
}
