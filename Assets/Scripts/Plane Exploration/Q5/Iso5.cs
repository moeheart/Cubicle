using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iso5 : MonoBehaviour {

	public Text instructionText;

	public GameObject isoCamera;

	public GameObject projectionButton;
	public GameObject isoButton;

	public void Click () {

		isoCamera.SetActive(true);

		projectionButton.SetActive (true);
		isoButton.SetActive (false);

		instructionText.text = "Please use direction keys for rotation and mouse for zoom in and out.";

	}
}
