using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBuilderUI : MonoBehaviour {

	public InputField inputField;
	public Button resetLevelButton;

	// Use this for initialization
	void Start () {
		inputField.onEndEdit.AddListener(delegate {SetParticipantName(inputField); });
		resetLevelButton.onClick.AddListener(ResetLevel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetParticipantName(InputField inputField) {
		if (inputField.text != null && inputField.text != "") {
			BlockBuilderConfigs.participantName = inputField.text;
			Debug.Log("input field name:" + inputField.text);
		}

	}

	private void ResetLevel() {
		BlockBuilderConfigs.id = 1;
	}
}
