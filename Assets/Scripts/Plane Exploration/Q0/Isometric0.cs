using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Isometric0 : MonoBehaviour {

	public GameObject isoCamera;

	public Text resultText;
	public GameObject panel, retryButton, isometricButton, isoRetryButton, projectionButton;
	public void Click () {

		isoCamera.SetActive(true);

		panel.SetActive (false);
		resultText.text = "";
		retryButton.SetActive (false);
		isometricButton.SetActive (false);

		isoRetryButton.SetActive (true);
		projectionButton.SetActive (true);

	}
}
