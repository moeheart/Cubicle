using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveObject {
	//constant
	int panelIndex;
	public Image image;

	//alterable
	public bool isKilled;
	public int polygonIndex;
	public GameObject gameObject;
	public float alphaScale;

	Vector3 initialPos;
	public float speed;
	float refreshTime;
	float endingTime;
	public float reactionTime;


	public ActiveObject(int newPanelIndex,int newPolygonIndex){
		
		panelIndex = newPanelIndex;

		image =  GameObject.Find ("Section_"+panelIndex.ToString()).GetComponent<Image>();
		image.gameObject.SetActive (false);
		polygonIndex = newPolygonIndex;
		isKilled = true;//false;
		gameObject = ActiveObjControl.revSolids [polygonIndex].gameObject;

		alphaScale = 1.0f;

		//SetPosAndSpeed ();
	}



	public void Refresh(){
		image.gameObject.SetActive (true);
		ChangeSpriteAccordingToSolid ();

		isKilled = false;
		gameObject = ActiveObjControl.revSolids [polygonIndex].gameObject;

		SetPosRotAndSpeed ();

		alphaScale = 1.0f;
		GetRefreshTime ();
		
	}

	void GetRefreshTime (){
		refreshTime = Time.realtimeSinceStartup;
	}

	public void GetReactionTime(){
		if (isKilled) {
			endingTime = Time.realtimeSinceStartup;
			reactionTime = endingTime - refreshTime;
			ActiveObjControl.RecordReactionTimeWhenObjectKilled (reactionTime);

		} else {
			reactionTime = Time.realtimeSinceStartup - refreshTime;

			if (RevSolidGameInfo.GetLODByInt() == 1) {
				RespondToReactionTime ();
			}
		}
	}


	public void RespondToReactionTime(){
		if (reactionTime >= RevSolidGameInfo.MaxReactionTime) {
			isKilled = true;
			RevSolidGameInfo.Add2FalseStrokeCount (1);
			RevSolidUIControl.RefreshBroadcasts ();
		} else if (reactionTime >= 3.0f) {
			Tutorial.IndicateKeyUsage ();
		}
	}

	void SetPosRotAndSpeed(){
		if (RevSolidGameInfo.GetLODByInt() == 1) {
			this.SetFixedPosition ();
		} else if (RevSolidGameInfo.GetLODByInt() == 2) {
			this.RegeneratePositionAndSpeed();
		}
		SetRotation ();
	}

	void SetFixedPosition (){
		initialPos = new Vector3(3.0f,0,0);
		speed = 0;
		gameObject.transform.position = initialPos;
	}

	void RegeneratePositionAndSpeed(){
		//initialPos = GenRandomPos ();
		initialPos = Hard.GenPosAccordingToPanelIndex(panelIndex);
		speed = Hard.GenRandomSpeed ();
		gameObject.transform.position = initialPos;
	}

	void SetRotation(){
		gameObject.transform.rotation = Quaternion.Euler(-45.0f,0,0);
	}

	public void ChangeSpriteAccordingToSolid(){
		image.sprite = AxisDrawing.sections [polygonIndex].imgSprite;
	}

	public void UseTutorialSpriteMatchingSolid(int frameIdx){
		image.sprite = AxisDrawing.sections [polygonIndex].tutorialSprite[frameIdx];
	}

}
