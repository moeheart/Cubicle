using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

	public GameObject menuPanel;

	public static bool isActive = false;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ToggleMenuPanel();
		}
	}

	public void ToggleMenuPanel() {
		isActive = !isActive;
		menuPanel.SetActive(isActive);
	}
}
