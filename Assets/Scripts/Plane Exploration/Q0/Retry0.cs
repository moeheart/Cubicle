using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Retry0 : MonoBehaviour {

	public GameObject canvas, player, isoCamera,tutPanel, tutObject, target;
	public Text resultText;


	public void Click () {
		
		Transform transform = canvas.transform;

		foreach(Transform child in transform){
			child.gameObject.SetActive (false);
		}

		resultText.gameObject.SetActive(true);
		resultText.text = "";
		
		tutPanel.SetActive (true);
		target.SetActive(true);
		player.SetActive (true);
		player.transform.position = new Vector3 (0.5f, 2.04f, 0.5f);

		tutObject.GetComponent<TutorialStage>().TutBefore();

		isoCamera.SetActive (false);
	}
}
