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
		SwitchRotationByKey ();
		for (int i = 0; i < activeObjects.Count; i++) {
			if (objectBehaviour != null) {
				objectBehaviour (i);
			}
			FadeInOrOut (i);
		}
	}

	void SwitchRotationByKey(){
		if (Input.GetKeyDown("space")) {
			if (objectBehaviour != null) {
				objectBehaviour -= Rotate;
			} else {
				objectBehaviour += Rotate;
			}
		}
	}


}
