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
	public GameObject hintPanel;

	private iOSPlayerTouchInput touchInput;
	private float _vertSpeed;
	private ControllerColliderHit _contact;
	
	private float radius = 2f;

	private CharacterController _charController;
	private Animator _animator;

	// Use this for initialization
	void Start () {
		_vertSpeed = minFall;
		_charController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
		touchInput = GetComponent<iOSPlayerTouchInput>();

		hintPanel = GameObject.Find("Hint Panel");

//		movementPanel = GameObject.Find("Movement Panel").GetComponent<MovementPanel>();
	}

	private float GetHorizontalMovement(){
		return touchInput.inputDirection.x;
	}

	private float GetVerticalMovement() {
		return touchInput.inputDirection.y;
	}
	
	// Update is called once per frame
	void Update () {
		touchInput.TrackChanges();
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
		
		CheckOperableDevice();
		// The right Finger is pressed, if it is a 
		if (touchInput.pressedRightFinger) {
			TryOperateDevice();
		}

		if (hitGround) {
			if (Input.GetButtonDown("Jump") || touchInput.pressedRightFinger) {
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

		movement.y = _vertSpeed;
		
		
		/*if (Input.GetKey(KeyCode.Space))
			movement.y = flyingSpeed;
		else
			movement.y = gravity;*/
		
		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);


	}

	private void CheckOperableDevice() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		bool canOperate = false;
		foreach (Collider hitCollider in hitColliders) {
			Vector3 direction = hitCollider.transform.position - this.transform.position;
			if (Vector3.Dot(transform.forward, direction) > .5f) {
				// Debug.Log("HIt collider: " + hitCollider.transform.name);
				if (hitCollider.gameObject.GetComponent<TriggerDevice>() != null) {
					canOperate = true;
					hintPanel.SetActive(true);
				}
			}
		}
		if (canOperate == false) {
			hintPanel.SetActive(false);
		}
	}

	private void TryOperateDevice() {
		RaycastHit hit;
		Vector2 pos;
		if (Application.isEditor) {
			pos = Input.mousePosition;
		}
		else {
			pos = touchInput.rightFingerCurrentPoint;
		}
		Ray ray = Camera.main.ScreenPointToRay(pos);
		if (Physics.Raycast(ray, out hit)) {
			var device = hit.transform.GetComponent<TriggerDevice>();
			if (device != null)
				device.Operate();
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		_contact = hit;
	}
}
