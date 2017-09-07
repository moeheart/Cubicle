using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Isometric8 : MonoBehaviour {

	public GameObject isoCamera;

	public Text resultText;
	public Text instructionText;
	public GameObject panel;
	public GameObject retryButton;
	public GameObject isometricButton;
	public GameObject isoRetryButton;
	public GameObject projectionButton;
	public GameObject nextButton;


	public void Click () {
		

		isoCamera.SetActive(true);
		nextButton.SetActive (false);

		panel.SetActive (false);
		resultText.text = "";
		retryButton.SetActive (false);
		isometricButton.SetActive (false);

		instructionText.text = "Please use direction keys for rotation and mouse for zoom in and out.";
		isoRetryButton.SetActive (true);
		projectionButton.SetActive (true);

	}
}
