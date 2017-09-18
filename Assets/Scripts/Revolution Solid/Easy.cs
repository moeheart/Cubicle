using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easy : ActiveObjControl {

	bool isRaycastHitOn;

	void Awake(){
		RevSolidGameInfo.MaxPanelNum=1;
		RevSolidGameInfo.InitializeHit ();
		RevSolidUIControl.FindStartGamePanel ();
		isRaycastHitOn = true;
	}
	// Use this for initialization
	void Start () {
		objectBehaviour += Rotate;
	}
	
	// Update is called once per frame
	void Update () {
		SwitchOffRotationByKey ();
		for (int i = 0; i < activeObjects.Count; i++) {
			if (isRaycastHitOn) {
				RaycastHit (i);
			}
			if (objectBehaviour != null) {
				objectBehaviour (i);
			}
			FadeInOrOut (i);
		}

		if(gameObjectJustHit != null){
			isRaycastHitOn = true;
			SwitchOnRotation ();
		}
	}

	void SwitchOffRotationByKey(){
		if (Input.GetKeyDown ("space")) {
			if (objectBehaviour != null) {
				objectBehaviour -= Rotate;
				gameObjectJustHit = null;
			} else {
				objectBehaviour += Rotate;
			}
		}

		//StartCoroutine(DisableRaycastHitForaWhile ());
	}

	IEnumerator DisableRaycastHitForaWhile (){
		isRaycastHitOn=false;
		yield return new WaitForSeconds (1);
		isRaycastHitOn=true;
	}

	void SwitchOnRotation(){
		if (objectBehaviour == null) {
			objectBehaviour += Rotate;
		}
	}


}
