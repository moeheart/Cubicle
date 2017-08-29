using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Retry5 : MonoBehaviour {

	public GameObject canvas, instructionTextObject, resultTextObject, player, Top1, Top2, Front1, Curve1, isoCamera, model;
	public Text instructionText, resultText;
	public GameObject target;

	public void Click () {
		Transform transform = canvas.transform;

		foreach(Transform child in transform){
			child.gameObject.SetActive (false);
		}

		instructionTextObject.SetActive (true);
		resultTextObject.SetActive (true);

		target.SetActive (true);

		instructionText.text = "Please use direction keys to orient the yellow " +
			"square to the orange one. Dropping is not allowed.";
		resultText.text = "";

		player.SetActive (true);
		player.transform.position = new Vector3 (0.5f,2.04f, 0.5f);

		isoCamera.SetActive (false);

		model.GetComponent<Model5> ().InitializeLog ();
	}
}
