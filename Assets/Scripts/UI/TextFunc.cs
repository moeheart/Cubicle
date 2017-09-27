using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFunc : MonoBehaviour {

	public GameObject objectsManager;

	public enum FunctionToCall {
		ToggleTarget,
		ToggleSceneObjects
	}

	public FunctionToCall function;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnActive() {
		switch (function) {
			case FunctionToCall.ToggleTarget:
				objectsManager.GetComponent<ObjectsManager>().ToggleTarget();
				break;
			case FunctionToCall.ToggleSceneObjects:
				objectsManager.GetComponent<ObjectsManager>().ToggleSceneObjects();
				break;
		}
	}
}
