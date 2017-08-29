using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformRetry : MonoBehaviour {

	public Dictionary<Vector3, bool> startModel;

	public GameObject controllerObject;
	public GameObject targetModelObject;
	public GameObject startModelObject;
	public GameObject notationObject;

	public Text text;

	public GameObject retryButton;

	public void Click(){
		
		controllerObject.SetActive (true);
		notationObject.SetActive (true);
		retryButton.SetActive (false);

		startModel = startModelObject.GetComponent<ModelGeneration> ().model;

		controllerObject.GetComponent<Controller> ().restStep = 
			targetModelObject.GetComponent<TransformGeneration> ().transNum
		+ controllerObject.GetComponent<Controller> ().difficulty
		+ controllerObject.GetComponent<Controller> ().wrongTime / 3;
		controllerObject.GetComponent<Controller> ().curModel = startModel;
		controllerObject.GetComponent<Controller> ().lastModel = null;

		text.text = " Rest Steps: " + controllerObject.GetComponent<Controller> ().restStep;

		targetModelObject.GetComponent<TransformGeneration> ().InitializeRecord ();


	}
}
