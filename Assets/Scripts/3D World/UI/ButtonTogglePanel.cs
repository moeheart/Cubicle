using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTogglePanel : MonoBehaviour {

	public GameObject menuPanel;

	public bool isActive = false;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void DisablePanel() {
		menuPanel.SetActive(false);
	}

	public void EnablePanel() {
		menuPanel.SetActive(true);
	}

	public void TogglePanel() {
		isActive = !isActive;
		menuPanel.SetActive(isActive);
	}
}
