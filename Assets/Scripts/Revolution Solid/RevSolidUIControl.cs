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
	private static GameObject[] checkMarks;
	private static GameObject gameStartPanel;
	private static bool isCandidateAxesShown = false;
	public static string defaultString = "";

	public static GameObject initialInstructionPanel;
	public static Button continueBtn;

	public static GameObject losingPanel;
	// Use this for initialization
	void Awake() {
		broadcast= GameObject.Find ("Text").GetComponent<Text> ();
		totalHit=GameObject.Find ("hit").GetComponent<Text> ();
		falseCount=GameObject.Find ("miss").GetComponent<Text> ();

		retryBtn = GameObject.Find ("retryBtn").GetComponent<Button> ();
		retryBtn.onClick.AddListener (TriggerRetry);

		responseBtn = GameObject.Find ("responseBtn").GetComponent<Button> ();
		HideButton (responseBtn);
		responseBtn.onClick.AddListener (Respond);

		tutorialText = GameObject.Find ("tutorialText").GetComponent<Text>();

		showAllBtn = GameObject.Find ("showAllBtn").GetComponent<Button> ();
		showAllBtn.onClick.AddListener (CandidateAxesSwitch);
		showAllSwitch=GameObject.Find ("showAllSwitch").GetComponent<Text> ();

		initialInstructionPanel = GameObject.Find ("initialInstructionPanel");
		continueBtn = GameObject.Find ("continue").GetComponent<Button> ();
		continueBtn.onClick.AddListener (DisableInitialInstructionPanel);

		losingPanel = GameObject.Find ("losingPanel");
		losingPanel.SetActive (false);
	}

	void OnEnable(){
		EventManager.StartListening ("Retry",Retry);
	}

	void OnDisable(){
		EventManager.StopListening ("Retry",Retry);
	}

	void Start(){
		checkMarks=new GameObject[4];
		for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {//for MaxPanelNum defined in Awake()
			checkMarks[i] = GameObject.Find ("checkMark_"+i.ToString());
			checkMarks[i].SetActive (false);
		}
		StartCoroutine ("ShowDefaultMsg");
	}
		
	IEnumerator ShowDefaultMsg(){
		while (true) {
			BroadcastMsg (defaultString);
			yield return new WaitForSeconds (3.0f);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public static void EnableInitialInstructionPanel(){
		initialInstructionPanel.SetActive (true);
	}
	public static void DisableInitialInstructionPanel(){
		Time.timeScale = 1;
		initialInstructionPanel.SetActive (false);

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

	public static void ShowRetry(){
		losingPanel.SetActive (true);
	}

	public static void HideRetry(){
		losingPanel.SetActive (false);
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
		HideRetry ();
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
		//Time.timeScale = 1;
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

	public void ShowCheckMark (int panelIndex){
		StartCoroutine (this.DisplayCheckMarkFor2Scs(panelIndex));
	}

	IEnumerator DisplayCheckMarkFor2Scs(int panelIndex){
		checkMarks[panelIndex].SetActive (true);
		//ActiveObjControl.activeObjects [panelIndex].image.gameObject.GetComponent<MeshRenderer> ().material.SetFloat ("_AlphaScale",0.0f);
		yield return new WaitForSeconds (2.0f);
		checkMarks[panelIndex].SetActive (false);
		//ActiveObjControl.activeObjects [panelIndex].image.gameObject.GetComponent<MeshRenderer> ().material.SetFloat ("_AlphaScale",1.0f);
	}

	public static void FindStartGamePanel(){
		gameStartPanel = GameObject.Find ("gameStartPanel");
	}

	public IEnumerator ShowStartGamePanel (){
		if (GameObject.Find ("gameStartPanel")) {
			float rate = 0.01f;
			for (int i = 0; i < 10; i++) {
				if (GameObject.Find ("gameStartPanel"))
					gameStartPanel.transform.Translate (new Vector3 (0, -10.0f, 0));
				else
					yield return null;
				rate -= 0.005f;
				yield return new WaitForSeconds (rate);
			}
			yield return new WaitForSeconds (2.0f);
			for (int i = 0; i < 10; i++) {
				gameStartPanel.transform.Translate (new Vector3 (0, 10.0f, 0));
				rate += 0.005f;
				yield return new WaitForSeconds (rate);
			}
			GameObject.Destroy (gameStartPanel);
		} else {
			yield return null;
		}
	}

}
