using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Over0 : MonoBehaviour {

	public Text resultText;
	public GameObject panel, retryButton, isometricButton, target, tutObject;

	void Start () {
		resultText.text = "";
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			tutObject.GetComponent<TutorialStage>().TutNext();
			other.gameObject.SetActive (false);
			panel.SetActive (true);
			resultText.text = "Game Over!";
			isometricButton.SetActive (true);
			target.SetActive (false);
			retryButton.SetActive(true);
		}
	}


}
