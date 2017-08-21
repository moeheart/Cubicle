using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Retry5 : MonoBehaviour {

	public GameObject canvas, instructionTextObject, resultTextObject, player, Top1, Top2, Front1, Curve1, isoCamera, model;
	public Text instructionText, resultText;

	public void Click () {
		Transform transform = canvas.transform;

		foreach(Transform child in transform){
			child.gameObject.SetActive (false);
		}

		instructionTextObject.SetActive (true);
		resultTextObject.SetActive (true);

		player.GetComponent<Player5> ().top1 = false;
		player.GetComponent<Player5> ().top2 = false;
		player.GetComponent<Player5> ().front1 = false;
		player.GetComponent<Player5> ().curve1 = false;

		Top1.SetActive (true);
		Top1.GetComponent<M5Top1> ().collide = false;
		Top2.SetActive (true);
		Top2.GetComponent<M5Top2> ().collide = false;
		Front1.SetActive (true);
		Front1.GetComponent<M5Front1> ().collide = false;
		Curve1.SetActive (true);
		Curve1.GetComponent<M5Curve1> ().collide = false;

		instructionText.text = "Please use direction keys to explore every " +
			"plane you can access. and press enter to confirm your exploration. " +
			"Dropping is not allowed.";
		resultText.text = "";

		player.SetActive (true);
		player.transform.position = new Vector3 (0.5f,2.04f, 0.5f);

		isoCamera.SetActive (false);

		model.GetComponent<Model5> ().InitializeLog ();
	}
}
