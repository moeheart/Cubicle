using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RevSolidUIControl : RevSolidGameInfo {
	
	private static Text broadcast;
	private static Text totalHit;
	private static Text falseCount;
	static Text tutorialText;
	private static Button retryBtn;
	private static Button responseBtn;
	private static Button showAllBtn;
	private static Text showAllSwitch;
	private static Button freeStrokeTutBtn;
	private static GameObject freeStrokeTut;
	private static bool isCandidateAxesShown = false;
	public static string defaultString = "Indicate the axis\nby a free stroke";
	// Use this for initialization
	void Awake() {
		broadcast= GameObject.Find ("Text").GetComponent<Text> ();
		totalHit=GameObject.Find ("hit").GetComponent<Text> ();
		falseCount=GameObject.Find ("miss").GetComponent<Text> ();

		retryBtn = GameObject.Find ("retryBtn").GetComponent<Button> ();
		HideButton (retryBtn);
		retryBtn.onClick.AddListener (TriggerRetry);

		responseBtn = GameObject.Find ("responseBtn").GetComponent<Button> ();
		HideButton (responseBtn);
		responseBtn.onClick.AddListener (Respond);

		tutorialText = GameObject.Find ("tutorialText").GetComponent<Text>();

		showAllBtn = GameObject.Find ("showAllBtn").GetComponent<Button> ();
		showAllBtn.onClick.AddListener (CandidateAxesSwitch);
		showAllSwitch=GameObject.Find ("showAllSwitch").GetComponent<Text> ();

		freeStrokeTutBtn = GameObject.Find ("freeStrokeExp").GetComponent<Button> ();
		freeStrokeTutBtn.onClick.AddListener (ShowFreeStrokeTutorial);
		freeStrokeTut = GameObject.Find ("freeStrokeTutorial");
		freeStrokeTut.SetActive (false);


	}

	void OnEnable(){
		EventManager.StartListening ("Retry",Retry);
	}

	void OnDisable(){
		EventManager.StopListening ("Retry",Retry);
	}

	void Start(){
		StartCoroutine ("ShowDefaultMsg");
	}
		
	IEnumerator ShowDefaultMsg(){
		while (true) {
			BroadcastMsg (defaultString);
			yield return new WaitForSeconds (1.0f);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public static void BroadcastMsg (string message){
		broadcast.text = message;
	}

	public static void BroadcastHits (){
		totalHit.text = "Hits: "+hit.ToString()+"/"+RevSolidGameInfo.WinningCriterion.ToString();
	}

	public static void BroadcastFalseStrokeCount (){
		falseCount.text = "Miss: "+falseStrokeCount.ToString ()+"/"+RevSolidGameInfo.MaxFalseCount.ToString();
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
		
	void TriggerRetry(){
		EventManager.TriggerEvent ("Retry");
	}

	public override void Retry(){
		base.Retry ();
		HideRetryButton ();
		BroadcastMsg (defaultString);
		RefreshBroadcasts ();
		Time.timeScale = 1;

	}

	public void Respond(){
		if (Tutorial.isTutorialModeOn) {
			ConfirmTutorialAndResume ();
		} else {
			RequireTutorial ();
			TutorialTrigger ();
		}
	}

	public void TutorialTrigger(){
		EventManager.TriggerEvent ("EnableTutorial");
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

	static void CandidateAxesSwitch(){
		isCandidateAxesShown = !isCandidateAxesShown;
		if (isCandidateAxesShown) {
			showAllSwitch.text = "Hide candidate axes";
			ShowCandidateAxes ();
		} else {
			showAllSwitch.text = "Show candidate axes";
			HideCandidateAxes ();
		}
	}

	static void ShowCandidateAxes(){
		RevSolidGameInfo.levelOfDifficulty -= 0.5f;
		AxisDrawing.ReloadSectionsWithCandidateAxes ();
	}

	static void HideCandidateAxes(){
		RevSolidGameInfo.levelOfDifficulty += 0.5f;
		AxisDrawing.RecoverOriginalSections ();
	}

	void ShowFreeStrokeTutorial(){
		StartCoroutine ("PlayFreeStrokeVideo");
	}

	IEnumerator PlayFreeStrokeVideo(){
		freeStrokeTut.SetActive (true);
		yield return new WaitForSeconds (5);
		freeStrokeTut.SetActive (false);
	}

}
