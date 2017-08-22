using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easy : ActiveObjControl {

	void Awake(){
		
	}
	// Use this for initialization
	void Start () {
		objectBehaviour += Rotate;

	}
	
	// Update is called once per frame
	void Update () {
		RotateSwitchByClick ();
		if (objectBehaviour != null) {
			for (int i = 0; i < activeObjects.Count; i++) {
				objectBehaviour (i);
				FadeInOrOut (i);
			}
		}
	}

	void RotateSwitchByClick(){
		if (Input.GetKeyDown("space")) {
			if (objectBehaviour != null) {
				objectBehaviour -= Rotate;
			} else {
				objectBehaviour += Rotate;
			}
		}
	}


}
