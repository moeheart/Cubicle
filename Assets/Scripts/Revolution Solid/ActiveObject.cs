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

		SetPosAndSpeed ();

		alphaScale = 1.0f;
		GetRefreshTime ();
	}

	void GetRefreshTime (){
		refreshTime = Time.time;
	}

	public void GetEndingTime(){
		endingTime = Time.time;
		GetReactionTime ();
	}

	public void GetReactionTime(){
		if (isKilled) {
			endingTime = Time.time;
			reactionTime = endingTime - refreshTime;
			ActiveObjControl.RecordReactionTimeWhenObjectKilled (reactionTime);

		} else {
			reactionTime = Time.time - refreshTime;

			if (RevSolidGameInfo.GetLODByInt() == 1) {
				RespondToReactionTime ();
			}
		}
	}


	public void RespondToReactionTime(){
		if (reactionTime >= 5.0f) {
			isKilled = true;
			RevSolidGameInfo.Add2FalseStrokeCount (1);
			RevSolidUIControl.RefreshBroadcasts ();
		} else if (reactionTime >= 3.0f) {
			Tutorial.IndicateKeyUsage ();
		}
	}

	void SetPosAndSpeed(){
		if (RevSolidGameInfo.GetLODByInt() == 1) {
			this.SetFixedPosition ();
		} else if (RevSolidGameInfo.GetLODByInt() == 2) {
			this.RegeneratePositionAndSpeed();
		}
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

	public void ChangeSpriteAccordingToSolid(){
		image.sprite = AxisDrawing.sections [polygonIndex].imgSprite;
	}

	public void UseTutorialSpriteMatchingSolid(){
		image.sprite = AxisDrawing.sections [polygonIndex].tutorialSprite;
	}

}
