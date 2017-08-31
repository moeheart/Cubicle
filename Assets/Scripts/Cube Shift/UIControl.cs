using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	public GameInfo gameInfo=GameInfo.getInstance();
	static Button button;
	static Button restartBtn;
	static Text btnText;

	static Text cubeHit;

	static Slider difficultySlider;
	static Text difficulty;

	static Text reactTime;

	static Slider cubeNumberSlider;
	static Text cubeNum;

	static Text score;

	bool isParameterAlterable=true;



	// Use this for initialization
	void Awake () {
		button = GameObject.Find ("Button").GetComponent<Button> ();
		button.onClick.AddListener(OnClick);
		restartBtn=GameObject.Find ("restartBtn").GetComponent<Button> ();
		restartBtn.onClick.AddListener(ClickToRestart);
		btnText = GameObject.Find ("btnText").GetComponent<Text> ();
		cubeHit = GameObject.Find ("cubeHit").GetComponent<Text> ();

		difficultySlider = GameObject.Find ("difficultySlider").GetComponent<Slider> ();
		difficultySlider.onValueChanged.AddListener (delegate{DifficultyChangeCheck();});
		difficulty = difficultySlider.transform.Find ("difficulty").gameObject.GetComponent<Text> ();

		reactTime=GameObject.Find ("reactTime").GetComponent<Text> ();

		cubeNumberSlider = GameObject.Find ("cubeNumberSlider").GetComponent<Slider> ();
		cubeNumberSlider.onValueChanged.AddListener (delegate{CubeNumChangeCheck();});
		cubeNum = cubeNumberSlider.transform.Find ("cubeNum").gameObject.GetComponent<Text> ();

		score=GameObject.Find ("score").gameObject.GetComponent<Text> ();
	}

	void OnEnable(){
		EventManager.StartListening ("OnProceedingTutorial",Proceed);
		EventManager.StartListening ("OnShiftingStart",Play);
		EventManager.StartListening ("OnRetry",Retry);
		EventManager.StartListening ("OnGeneratingNewGame",Restart);
	}
	void OnDisable(){
		EventManager.StopListening ("OnProceedingTutorial",Proceed);
		EventManager.StopListening ("OnShiftingStart",Play);
		EventManager.StopListening ("OnRetry",Retry);
		EventManager.StopListening ("OnGeneratingNewGame",Restart);
	}

	void Start(){
		if (GameInfo.CubeNumber == 3) {
			isParameterAlterable = false;
			difficultySlider.gameObject.SetActive (false);
			difficulty.gameObject.SetActive (false);
			cubeNumberSlider.gameObject.SetActive (false);
			cubeNum.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//button & text region
		if(gameInfo.phaseNo == 0){
			gameInfo.PauseGame ();
			button.gameObject.SetActive (true);
			btnText.text = "Proceed";
			cubeHit.text = "Given several cubes, organized in 3*3 space, with a {candy cane} in one of them\n\n"+"Now please REMEMBER the position of each cube and the candy for later manipulation & recall";
			restartBtn.gameObject.SetActive (false);
		}
		else if(gameInfo.phaseNo == 1){
			
			button.gameObject.SetActive (true);
			btnText.text = "Play";
			cubeHit.text = "After you press [Play], cubes will begin shifting\n\n"+"Candy in one cube will travel to the nearest one with an adjoining face, "+"both of which are later marked [GREEN]";
			restartBtn.gameObject.SetActive (false);
		}
		else if(gameInfo.phaseNo == 2){
			
			if (gameInfo.moves == 1) {
				button.gameObject.SetActive (false);
			}
			else if (gameInfo.moves == 2) {
				gameInfo.target.SetActive (false);
			}

			cubeHit.text = "Now the candy is travelling between cubes";
			restartBtn.gameObject.SetActive (false);
		}
		else if (gameInfo.phaseNo == 3) {
			if (gameInfo.moves == 3) {
				gameInfo.PauseAtFirstMove ();

				button.gameObject.SetActive (true);
				btnText.text = "Retry";

				if (!GameInfo.isTargetFound) {
					gameInfo.isChooseEnabled = true;
					cubeHit.text = "Now this is the same view as shown at beginning. Indicate the cube with candy by ONE CLICK on it";
				}
			}
			if (GameInfo.isTargetFound) {
				gameInfo.isChooseEnabled = false;
				cubeHit.text = "You've found the candy!";
				restartBtn.gameObject.SetActive (true);
			}
		}
		reactTime.text = GameInfo.reactTime.ToString ("##.000");
	}

	void OnClick(){
		if (gameInfo.phaseNo == 0) {//Proceed
			EventManager.TriggerEvent("OnProceedingTutorial");
		}
		else if (gameInfo.phaseNo == 1) {//play
			EventManager.TriggerEvent("OnShiftingStart");
		}
		else if (gameInfo.phaseNo == 3) {//retry
			EventManager.TriggerEvent("OnRetry");
		}
	}

	void Proceed(){
		gameInfo.phaseNo++;
	}

	void Play(){
		gameInfo.ResumeGame ();
		gameInfo.Play ();
	}

	void Retry(){
		gameInfo.Retry ();
	}

	void ClickToRestart(){
		EventManager.TriggerEvent("OnGeneratingNewGame");
	}

	void Restart(){
		if (gameInfo.phaseNo == 3) {//restart
			gameInfo.Restart ();
		}
	}

	void DifficultyChangeCheck(){
		GameInfo.MaxTravelPeriodNo=(int)((difficultySlider.value)*5+1);
		difficulty.text = (GameInfo.MaxTravelPeriodNo).ToString()+ " shifting / trial";
	}

	void CubeNumChangeCheck(){
		GameInfo.CubeNumber=(int)((cubeNumberSlider.value)*2+4);
		cubeNum.text = (GameInfo.CubeNumber).ToString()+ " cubes";
	}

	public static void RefreshScore(){
		score.text = "SCORE "+GameInfo.score.ToString();
		if (GameInfo.CheckIfWinningCriterionMet ()) {
			score.text+="\nCongratulations! The next level is unlocked now. \nPress Q to go on with world exploration";
		}
	}

}
