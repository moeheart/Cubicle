using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private Button button;

	private bool isPressed;

	// Use this for initialization
	void Start () {
		isPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData eventData) {
		isPressed = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		isPressed = false;

	}

	public bool IsPressed() {
		return isPressed;
	}
}
