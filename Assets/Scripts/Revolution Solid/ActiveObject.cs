using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveObject : MonoBehaviour {
	//constant
	public int panelIndex;
	public Image image;

	//alterable
	public bool isKilled;
	public int polygonIndex;
	public GameObject gameObject;
	public float alphaScale;

	public Vector3 initialPos;
	public float speed;
	float refreshTime;
	float endingTime;
	float reactionTime;

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
		RecordReactionTime ();
	}

	public void RecordReactionTime(){
		if (isKilled) {
			endingTime = Time.time;
			reactionTime = endingTime - refreshTime;
		} else {
			reactionTime = Time.time - refreshTime;

			if (RevSolidGameInfo.levelOfDifficulty == 0) {
				if (reactionTime >= 5.0f) {
					this.isKilled = true;
				}
			}
		}
	}

	void SetPosAndSpeed(){
		if (RevSolidGameInfo.levelOfDifficulty == 0) {
			this.SetFixedPosition ();
		} else if (RevSolidGameInfo.levelOfDifficulty == 1) {
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
