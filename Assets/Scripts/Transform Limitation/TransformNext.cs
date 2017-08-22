using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformNext : MonoBehaviour {
	

	public GameObject controllerObject;
	public GameObject targetModelObject;
	public GameObject startModelObject;
	public GameObject notationObject;

	public Text text;

	public GameObject nextButton;

	public void Click(){
		controllerObject.SetActive (true);
		notationObject.SetActive (true);
		nextButton.SetActive (false);

		startModelObject.GetComponent<ModelGeneration> ().level += 1;
		startModelObject.GetComponent<ModelGeneration> ().Initialize ();
		targetModelObject.GetComponent<TransformGeneration> ().Initialize ();

		controllerObject.GetComponent<Controller> ().Initialize ();




	}
}
