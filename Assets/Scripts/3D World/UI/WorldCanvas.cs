using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldCanvas : MonoBehaviour {

	public Button[] buttons;

	private Button menuButton;
	private Button controlSettingsButton;

	// Use this for initialization
	void Start () {
		menuButton = buttons[0];
		controlSettingsButton = buttons[1];
		menuButton.onClick.AddListener(
			delegate {OnClickButton(menuButton); });
		controlSettingsButton.onClick.AddListener(
			delegate {OnClickButton(controlSettingsButton); } 
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickButton(Button clickedButton) {
		foreach (Button itButton in buttons) {
			if (itButton == clickedButton) {
				clickedButton.GetComponent<ButtonTogglePanel>().TogglePanel();
			}
			else {
				itButton.GetComponent<ButtonTogglePanel>().DisablePanel();
			}
		}
	}
}
