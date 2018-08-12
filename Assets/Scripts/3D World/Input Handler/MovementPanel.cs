using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementPanel : MonoBehaviour, IPointerExitHandler {

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
}
