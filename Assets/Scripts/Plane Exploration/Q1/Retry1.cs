using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Retry1 : MonoBehaviour {

	public GameObject canvas, instructionTextObject, resultTextObject, player, Top1, Top2, Right2, isoCamera, model;
	public Text instructionText, resultText;

	public void Click () {
		
		Transform transform = canvas.transform;

		foreach(Transform child in transform){
			child.gameObject.SetActive (false);
		}

		instructionTextObject.SetActive (true);
		resultTextObject.SetActive (true);

		player.GetComponent<Player1> ().top1 = false;
		player.GetComponent<Player1> ().top2 = false;
		player.GetComponent<Player1> ().right2 = false;

		Top1.SetActive (true);
		Top1.GetComponent<M1Top1> ().collide = false;
		Top2.SetActive (true);
		Top2.GetComponent<M1Top2> ().collide = false;
		Right2.SetActive (true);
		Right2.GetComponent<M1Right2> ().collide = false;

		instructionText.text = "Please use direction keys to explore every " +
			"plane you can access. and press enter to confirm your exploration. " +
			"Dropping is not allowed.";
		resultText.text = "";

		player.SetActive (true);
		player.transform.position = new Vector3 (1.0f, 2.04f, 0.5f);

		isoCamera.SetActive (false);

		model.GetComponent<Model1> ().InitializeLog ();
	}
}
