using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevSolidUIControl : RevSolidGameInfo {
	
	private static Text broadcast;
	private static Text totalHit;
	private static Text falseCount;
	static Text tutorialText;
	private static Button retryBtn;
	private static Button responseBtn;
	// Use this for initialization
	void Awake() {
		broadcast= GameObject.Find ("Text").GetComponent<Text> ();
		totalHit=GameObject.Find ("hit").GetComponent<Text> ();
		falseCount=GameObject.Find ("miss").GetComponent<Text> ();

		retryBtn = GameObject.Find ("retryBtn").GetComponent<Button> ();
		HideButton (retryBtn);
		retryBtn.onClick.AddListener (Retry);

		responseBtn = GameObject.Find ("responseBtn").GetComponent<Button> ();
		HideButton (responseBtn);
		responseBtn.onClick.AddListener (Respond);

		tutorialText = GameObject.Find ("tutorialText").GetComponent<Text>();

	}

	// Update is called once per frame
	void Update () {
		
	}

	public static void BroadcastMsg (string message){
		broadcast.text = message;
	}

	public static void BroadcastHits (){
		totalHit.text = "Hits: "+hit.ToString();
	}

	public static void BroadcastFalseStrokeCount (){
		falseCount.text = "Miss: "+falseStrokeCount.ToString ();
	}

	public static void ShowRetryButton(){
		ShowButton (retryBtn);
	}

	public static void HideRetryButton(){
		HideButton (retryBtn);
	}

	public static void ShowResponseButton(){
		ShowButton (responseBtn);
	}

	public static void HideResponseButton(){
		HideButton (responseBtn);
	}

	public override void Retry(){
		base.Retry ();
		HideRetryButton ();
		RefreshBroadcasts ();
		Time.timeScale = 1;

	}

	public void Respond(){
		if (Tutorial.isTutorialModeOn) {
			ConfirmTutorialAndResume ();
		} else {
			RequireTutorial ();
			Tutorial.EnableTutorial ();
		}
	}

	void ConfirmTutorialAndResume(){
		Time.timeScale = 1;
		HideResponseButton ();
		SetTutorialMessage ("");
		Tutorial.CancelAnsIndication ();
	}

	void RequireTutorial(){
		ConfirmTutorialAndResume ();
	}

	public static void RefreshBroadcasts(){
		BroadcastHits ();
		BroadcastFalseStrokeCount ();
	}

	static void ShowButton(Button btn){
		btn.gameObject.SetActive (true);
	}

	static void HideButton(Button btn){
		btn.gameObject.SetActive (false);
	}

	public static void SetTutorialMessage(string message){
		tutorialText.text = message;
	}

}
