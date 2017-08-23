using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public static bool isTutorialModeOn; 
	static bool isPitfallWarningDone;//done once

	public static GameObject axisPrefab;

	void Awake(){
		isTutorialModeOn = true;
		isPitfallWarningDone = false;

		axisPrefab =Resources.Load ("axisPrefab") as GameObject;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine ("AutoDisableTutorial");
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator AutoDisableTutorial(){
		while (true) {
			if (RevSolidGameInfo.CheckIfPlayerLearned () == true) {
				DisableTutorial ();
			}
			yield return new WaitForSeconds (10);
		}
	}

	public static void EnableTutorial (){
		isTutorialModeOn = true;
		isPitfallWarningDone=false;
	}

	public static void DisableTutorial(){
		isTutorialModeOn = false;
		isPitfallWarningDone = true;
	}

	public static void IndicateAnApproachingObject (){
		if(isTutorialModeOn){
			Time.timeScale = 0;
			RevSolidUIControl.SetTutorialMessage("AS YOU MAY NOTICE \n\n" +
				"Every solid approaching your window center, are formed by revolution of a shape here.\n\n"+
				"1. Observe each solid & the shape it revolutes by,\n2. Draw on the shape the corresponding revolution axis in your mind");
			RevSolidUIControl.ShowResponseButton ();
		}
	}

	public static void IndicateCorrectAns (int panel){
		if(RevSolidGameInfo.CheckIfPlayerFailsMuch()==true){
			RevSolidUIControl.SetTutorialMessage ("Still need tutorial?");
			RevSolidUIControl.ShowResponseButton ();
		}
		if (isTutorialModeOn && ActiveObjControl.WithinViewport (panel)) {
			Time.timeScale = 0;
			RevSolidUIControl.SetTutorialMessage("Here we show the revolution axis and corresponding stroke. Try it yourself!");
			IndicateAxisAndStroke (panel);
			RevSolidUIControl.ShowResponseButton ();
			ActiveObjControl.activeObjects [panel].UseTutorialSpriteMatchingSolid ();
		}
	}

	public static void CancelAnsIndication(){
		if (GameObject.Find ("axis") != null) {
			Destroy (GameObject.Find ("axis"));
		}
		for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
			ActiveObjControl.activeObjects [i].ChangeSpriteAccordingToSolid ();
		}
	}

	public static void IndicateAxisAndStroke (int panel){
		GameObject tempAxis;
		tempAxis = Instantiate (axisPrefab, ActiveObjControl.activeObjects [panel].gameObject.transform);
		tempAxis.name = "axis";
		//exception
		if (ActiveObjControl.activeObjects [panel].polygonIndex == 2) {
			tempAxis.transform.localPosition= new Vector3(0,0,0.035f);
		}
	}
		
	public static void IndicatePitfalls (){
		if (isTutorialModeOn&&isPitfallWarningDone==false) {
			Time.timeScale = 0;
			RevSolidUIControl.SetTutorialMessage("Watch out. Once a solid enter the ring in the middle, you will lose your point.");
			RevSolidUIControl.ShowResponseButton ();
			isPitfallWarningDone = true;
		}
	}

	public static void IndicateKeyUsage(){
		if (isTutorialModeOn) {
			RevSolidUIControl.SetTutorialMessage ("Press SPACE key anytime to freeze solid.");
		}
	}
}
