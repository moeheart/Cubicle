using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projection1 : MonoBehaviour {

	public Text instructionText;

	public GameObject isoCamera;

	public GameObject projectionButton;
	public GameObject isoButton;

	public void Click () {

		isoCamera.SetActive(false);

		projectionButton.SetActive (false);
		isoButton.SetActive (true);

		instructionText.text = "";

	}
}
