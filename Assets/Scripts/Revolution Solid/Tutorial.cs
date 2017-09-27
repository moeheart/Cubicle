using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour {

	static bool isPitfallWarningDone;//done once

	public static GameObject axisPrefab;
	public static bool isInstructionPanelEnabled;

	void Awake(){
		isPitfallWarningDone = false;

		isInstructionPanelEnabled = true;

		axisPrefab =Resources.Load ("axisPrefab") as GameObject;
	}

	void OnEnable(){
		EventManager.StartListening ("EnableTutorial",EnableTutorial);
		EventManager.StartListening ("DisableTutorial",DisableTutorial);
	}

	void OnDisable(){
		EventManager.StopListening ("EnableTutorial",EnableTutorial);
		EventManager.StopListening ("DisableTutorial",DisableTutorial);
	}

	// Use this for initialization
	void Start () {
		DisplayInitialInstructions ();
		Tutorial.IndicateKeyUsage ();
	}
	
	// Update is called once per frame
	void Update () {


	}

	static void DisplayInitialInstructions(){
		Time.timeScale = 0;
		RevSolidUIControl.EnableInitialInstructionPanel ();
	}

	public static void EnableTutorial (){
		RevSolidGameInfo.indicationCountSinceLastTutorial = 0;
		isPitfallWarningDone=false;
		for(int i=0;i<RevSolidGameInfo.MaxPanelNum;i++){
			Tutorial.IndicateCorrectAns (i);
		}
	}

	public static void DisableTutorial(){
		isPitfallWarningDone = true;

	}

	public static void IndicateCorrectAns (int panel){
		if (!RevSolidGameInfo.IfNoviceGuideEnds ()) {
			//RevSolidUIControl.SetTutorialMessage("Draw the revolution axis with your mouse");
			RevSolidGameInfo.indicationCountSinceLastTutorial++;
			GameObject.Find("Canvas1").GetComponent<Tutorial>().IndicateAxisAndStroke (panel);
		}
			
	}

	public static void CancelAnsIndication(){
		/*
if (GameObject.Find ("axis") != null) {
			Destroy (GameObject.Find ("axis"));
		}
		*/

		for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
			ActiveObjControl.activeObjects [i].ChangeSpriteAccordingToSolid ();
		}
	}

	public IEnumerator tutorialAnimationCoroutine;
	public void IndicateAxisAndStroke (int panel){
		/*
		GameObject tempAxis;
		tempAxis = Instantiate (axisPrefab, ActiveObjControl.activeObjects [panel].gameObject.transform);
		tempAxis.name = "axis";
		//exception
		if (ActiveObjControl.activeObjects [panel].polygonIndex == 2) {
			tempAxis.transform.localPosition= new Vector3(0,0,0.035f);
		}
		*/
			
		tutorialAnimationCoroutine = FreeStrokeAnimation (panel);
		StartCoroutine(tutorialAnimationCoroutine);
	}

	IEnumerator FreeStrokeAnimation(int panel){
		for (int i = 0; i < 1; i++) {
			yield return new WaitForSeconds (2.0f);
			for (int j = 0; j < 8; j++) {
				ActiveObjControl.activeObjects [panel].UseTutorialSpriteMatchingSolid (j);
				yield return new WaitForSeconds (0.15f);
			}
		}
	}
		
	public static void IndicatePitfalls (){
		if (isPitfallWarningDone==false) {
			//Time.timeScale = 0;
			RevSolidUIControl.SetTutorialMessage("Watch out. Once a solid enter the ring in the middle, you will lose your point.");
			//RevSolidUIControl.ShowResponseButton ();
			isPitfallWarningDone = true;
		}
	}

	public static void IndicateKeyUsage(){
		if (RevSolidGameInfo.MaxPanelNum == 1) {
			RevSolidUIControl.SetTutorialMessage ("[Hover cursor on solid once & WASD] - rotate solid | [SPACE] - freeze rotation");
		}
		else {//4
			RevSolidUIControl.SetTutorialMessage ("[Hover cursor on solid once & WASD] - rotate solid");
		}
	}
}
