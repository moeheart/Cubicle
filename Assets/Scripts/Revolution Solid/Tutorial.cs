using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public static bool isTutorialModeOn; 

	public static GameObject axisPrefab;

	void Awake(){
		isTutorialModeOn = true;

		axisPrefab = GameObject.Find ("axisPrefab");
		//GameObject.Destroy(GameObject.Find ("axis"));
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
				isTutorialModeOn = false;
			}
			yield return new WaitForSeconds (10);
		}
	}

	public static void EnableTutorial (){
		isTutorialModeOn = true;
	}

	public static void DisableTutorial(){
		isTutorialModeOn = false;
	}

	public static void IndicateAnApproachingObject (){
		if(isTutorialModeOn){
			Debug.Log ("IndicateAnApproachingObject");
			Time.timeScale = 0;
			RevSolidUIControl.SetTutorialMessage("AS YOU MAY NOTICE \n\n" +
				"All solids approaching your window center, are formed by revolution of the 4 shapes here.\n\n"+
				"1. Find for each solid the shape it revolutes by,\n2. Draw on the shape the corresponding revolution axis in your mind");
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
		}
	}

	public static void CancelAnsIndication(){
		Destroy(GameObject.Find ("axis"));
	}

	public static void IndicateAxisAndStroke (int panel){
		GameObject tempAxis;
		tempAxis = Instantiate (axisPrefab, ActiveObjControl.activeObjects [panel].gameObject.transform);
		tempAxis.name = "axis";
		//specially
		if (ActiveObjControl.activeObjects [panel].polygonIndex == 2) {
			tempAxis.transform.localPosition= new Vector3(0,0,0.035f);
		}
	}

	void IndicatePitfalls (){
		if (ActiveObjControl.Distance2Center (0) < 2.1f && ActiveObjControl.activeObjects [0].isKilled == false) {
			Time.timeScale = 0;
			//display tutorial img3
		}
	}
}
