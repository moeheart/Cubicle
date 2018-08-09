using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {

	public float walkingSpeed = 6.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -20.0f;
	public float minFall = -1.5f;

	private float _vertSpeed;
	private ControllerColliderHit _contact;

	private CharacterController _charController;
	private Animator _animator;

	// Use this for initialization
	void Start () {
		_vertSpeed = minFall;
		_charController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}

	private float GetHorizontalMovement(){
		return Input.GetAxis("Horizontal");
	}

	private float GetVerticalMovement() {
		return Input.GetAxis("Vertical");
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 movement = Vector3.zero;

		float deltaX = GetHorizontalMovement() * walkingSpeed;
		float deltaZ = GetVerticalMovement() * walkingSpeed;
		movement.x = deltaX;
		movement.z = deltaZ;
		movement = Vector3.ClampMagnitude(movement, walkingSpeed);

		_animator.SetFloat("Speed", movement.sqrMagnitude);


		bool hitGround = false;
		RaycastHit hit;
		if (_vertSpeed < 0 &&
			Physics.Raycast(transform.position, Vector3.down, out hit)) {
				float check = (_charController.height + _charController.radius) / 1.9f;
				hitGround = hit.distance <= check;
			}

		if (hitGround) {
			if (Input.GetButtonDown("Jump")) {
				_vertSpeed = jumpSpeed;
			}
			else {
				_vertSpeed = minFall;
				_animator.SetBool("Jumping", false);
			}
		}
		else {
			_vertSpeed += gravity * 5 * Time.deltaTime;
			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}

			if (_contact != null) {
				_animator.SetBool("Jumping", true);
			}

			if (_charController.isGrounded) {
				if (Vector3.Dot(movement, _contact.normal) < 0) {
					movement = _contact.normal * walkingSpeed;
				} else {
					movement += _contact.normal * walkingSpeed;
				}
			}
		}

		//TODO
		if (Input.GetButton("Jump")) {
			_vertSpeed = jumpSpeed;
		}

		movement.y = _vertSpeed;
		
		
		/*if (Input.GetKey(KeyCode.Space))
			movement.y = flyingSpeed;
		else
			movement.y = gravity;*/
		
		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		_contact = hit;
	}
}
