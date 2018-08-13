using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	public Vector3 inputDirection;

	private Image joystickContainer;

	private Image joystick;

	void Start() {
		joystickContainer = GetComponent<Image>();
		joystick = transform.GetChild(0).GetComponent<Image>();
		inputDirection = Vector3.zero;
	}

	public bool IsPositionInContainer(Vector2 position) {
		return RectTransformUtility.RectangleContainsScreenPoint(
			joystickContainer.rectTransform, position);
	}

	public void OnDrag(PointerEventData eventData) {
		Vector2 position = Vector2.zero;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			joystickContainer.rectTransform,
			eventData.position,
			eventData.pressEventCamera,
			out position
		);

		position.x = (position.x/joystickContainer.rectTransform.sizeDelta.x);
		position.y = (position.y/joystickContainer.rectTransform.sizeDelta.y);

		float x = position.x * 2;
		float y = position.y * 2;

		inputDirection = new Vector3(x,y,0);
		if (inputDirection.magnitude > 1) {
			inputDirection = inputDirection.normalized;
		}

		joystick.rectTransform.anchoredPosition = new Vector3 (
			inputDirection.x * joystickContainer.rectTransform.sizeDelta.x / 3,
			inputDirection.y * joystickContainer.rectTransform.sizeDelta.y / 3
		);
	}

	public void OnPointerDown(PointerEventData eventData) {
		OnDrag(eventData);
	}

	public void OnPointerUp(PointerEventData eventData) {
		inputDirection = Vector3.zero;
		joystick.rectTransform.anchoredPosition = Vector3.zero;
	}

	/*
	[SerializeField]
	private GameObject knob;

	private Vector3 knobStartingPosition;

	// Use this for initialization
	void Start () {
		knobStartingPosition = knob.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		int touchCount = Input.touchCount;
		bool isOverPanel = false;
		Debug.Log(touchCount);
		if (EventSystem.current.IsPointerOverGameObject()) {
			Debug.Log("Pointer is over game object");
			knob.transform.position = Input.mousePosition;
			isOverPanel = true;
		}
		for (int i = 0; i < touchCount; ++i) {
			if (EventSystem.current.IsPointerOverGameObject(i)) {
				Debug.Log("Pointer is over game object");
				knob.transform.position = Input.GetTouch(i).position;
				isOverPanel = true;
			}
		}

		if (!isOverPanel) {
			knob.transform.position = knobStartingPosition;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		knob.transform.position = knobStartingPosition;
	}
	*/
}
