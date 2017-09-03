using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {

	public float walkingSpeed = 6.0f;
	public float flyingSpeed = 12.0f;
	public float gravity = -9.8f;

	public float rotSpeed = 15.0f;

	private CharacterController _charController;

	// Use this for initialization
	void Start () {
		_charController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 movement = Vector3.zero;

		float deltaX = Input.GetAxis("Horizontal") * walkingSpeed;
		float deltaZ = Input.GetAxis("Vertical") * walkingSpeed;
		movement.x = deltaX;
		movement.z = deltaZ;
		movement = Vector3.ClampMagnitude(movement, walkingSpeed);
		if (Input.GetKey(KeyCode.Space))
			movement.y = flyingSpeed;
		else
			movement.y = gravity;
		
		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
	}
}
