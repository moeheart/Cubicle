using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Retry4 : MonoBehaviour {

	public GameObject canvas, instructionTextObject, resultTextObject, player, Top1, Right1, isoCamera, model;
	public Text instructionText, resultText;

	public void Click () {
		Transform transform = canvas.transform;

		foreach(Transform child in transform){
			child.gameObject.SetActive (false);
		}

		instructionTextObject.SetActive (true);
		resultTextObject.SetActive (true);

		player.GetComponent<Player4> ().top1 = false;
		player.GetComponent<Player4> ().right1 = false;

		Top1.SetActive (true);
		Top1.GetComponent<M4Top1> ().collide = false;
		Right1.SetActive (true);
		Right1.GetComponent<M4Right1> ().collide = false;


		instructionText.text = "Please use direction keys to explore every " +
			"plane you can access. and press enter to confirm your exploration. " +
			"Dropping is not allowed.";
		resultText.text = "";

		player.SetActive (true);
		player.transform.position = new Vector3 (1f,2.04f, 0.5f);

		isoCamera.SetActive (false);

		model.GetComponent<Model4> ().InitializeLog ();
	}
}
