using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverButtonText : MonoBehaviour {

	public Text instructionText;

	// Use this for initialization
	void Start () {
		HideInstructions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseOver() {
		ShowInstructions();
	}

	public void OnMouseExit() {
		HideInstructions();
	}

	public void ShowInstructions() {
		instructionText.enabled = !instructionText.enabled;
	}

	public void HideInstructions() {
		instructionText.enabled = false;
	}
}
