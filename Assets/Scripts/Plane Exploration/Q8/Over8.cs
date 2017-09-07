using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Over8 : MonoBehaviour {

	public Text overText;
	public Text instructionText;
	public GameObject panel;
	public GameObject retryButton;
	public GameObject isometricButton;

	public GameObject logObject;
	public GameObject target;

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
			target.SetActive (false);
			instructionText.text = "";
			retryButton.SetActive (true);
			isometricButton.SetActive (true);
			logObject.GetComponent<PlaneExplorationLog> ().RecordResult (-1);
		}
	}
}
