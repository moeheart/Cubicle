using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverButtonText : MonoBehaviour {

	public GameObject instructionPanel;

	private bool isActive = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleInstructions() {
		isActive = !isActive;
		instructionPanel.SetActive(isActive);
	}

}
