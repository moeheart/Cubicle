using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Over3 : MonoBehaviour {

	public Text overText;
	public Text instructionText;
	public GameObject panel;
	public GameObject retryButton;
	public GameObject isometricButton;

	void Start () {
		overText.text = "";
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			other.gameObject.SetActive (false);
			panel.SetActive (true);
			overText.text = "Game Over!";
			instructionText.text = "";
			retryButton.SetActive (true);
			isometricButton.SetActive (true);
		}
	}
}
