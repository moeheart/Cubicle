using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hard : ActiveObjControl {
	
	static float forbiddenRadius=2.0f; 
	static float PitfallWarningRadius=3.0f;
	void Awake(){
		RevSolidGameInfo.MaxPanelNum=4;
		RevSolidGameInfo.InitializeHit ();
		RevSolidUIControl.FindStartGamePanel ();
	}
	// Use this for initialization
	void Start () {
		objectBehaviour += Rotate;
		objectBehaviour += Move;
		objectBehaviour += Pitfall;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < activeObjects.Count; i++) {
			if (objectBehaviour != null) {
				objectBehaviour (i);
			}
			FadeInOrOut (i);
		}
	}

	void Move(int objIndex){
		if (activeObjects [objIndex].isKilled == false && activeObjects [objIndex].gameObject.transform.position != midPos) {
			activeObjects [objIndex].gameObject.transform.position=Vector3.MoveTowards(activeObjects [objIndex].gameObject.transform.position,midPos,Time.deltaTime*activeObjects[objIndex].speed);
		}

	}

	void Pitfall(int objIndex){
		if (InCentralArea (objIndex)) {
			activeObjects [objIndex].isKilled = true;
		}
	}
	bool InCentralArea(int objIndex){
		if (Distance2Center(objIndex) < forbiddenRadius) {
			RevSolidGameInfo.Add2FalseStrokeCount (1);
			RevSolidUIControl.RefreshBroadcasts ();
			Tutorial.IndicateAnApproachingObject ();
			return true;
		}
		else if(Distance2Center(objIndex) < PitfallWarningRadius){
			Tutorial.IndicatePitfalls ();
			return false;
		}
		else {
			return false;
		}

	}
	public static float Distance2Center(int objIndex){
		return Vector3.Distance (activeObjects [objIndex].gameObject.transform.position, midPos);
	}

	public static Vector3 GenPosAccordingToPanelIndex(int i){
		Vector3 newPos = new Vector3 (10.0f, 5.0f, 0);
		switch (i) {
		case 0:
			newPos = new Vector3 (10.0f, 5.0f, 0);
			break;
		case 1:
			newPos = new Vector3 (-10.0f, 5.0f, 0);
			break;
		case 2:
			newPos = new Vector3 (-10.0f, -5.0f, 0);
			break;
		case 3:
			newPos = new Vector3 (10.0f, -5.0f, 0);
			break;
		}
		return newPos;
	}

	public static Vector3 GenRandomPos(){
		Vector3 newPos = new Vector3 (-10.0f, 0.0f, 0);
		int rand = Mathf.FloorToInt(Random.value*8);
		switch (rand) {
		case 0:
			newPos = new Vector3 (10.0f, -5.0f, 0);
			break;
		case 1:
			newPos = new Vector3 (-10.0f, -5.0f, 0);
			break;
		case 2:
			newPos = new Vector3 (10.0f, 5.0f, 0);
			break;
		case 3:
			newPos = new Vector3 (-10.0f, 5.0f, 0);
			break;
		case 4:
			newPos = new Vector3 (10.0f, 2.5f, 0);
			break;
		case 5:
			newPos = new Vector3 (-10.0f, 2.5f, 0);
			break;
		case 6:
			newPos = new Vector3 (10.0f, -2.5f, 0);
			break;
		case 7:
			newPos = new Vector3 (-10.0f, -2.5f, 0);
			break;
		}

		return newPos;
	}

	public static float GenRandomSpeed(){
		float spd;
		spd=Mathf.Clamp(Random.value*1.0f,0.1f,1.0f);
		return spd;
	}
}


