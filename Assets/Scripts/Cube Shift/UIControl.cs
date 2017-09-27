using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	public GameInfo gameInfo=GameInfo.getInstance();
	static Button button;
	static Button restartBtn;
	static GameObject restartCanvas;
	static Text btnText;

	static Text msgText;

	static Slider difficultySlider;
	static Text difficulty;

	static Text reactTime;

	static Slider cubeNumberSlider;
	static Text cubeNum;

	static Text score;

	bool isParameterAlterable=true;

	static Text popUpMessage;

	static Button instruction;
	static GameObject instructionImg;

	// Use this for initialization
	void Awake () {
		button = GameObject.Find ("Button").GetComponent<Button> ();
		button.onClick.AddListener(OnClick);
		restartBtn=GameObject.Find ("restartBtn").GetComponent<Button> ();
		restartBtn.onClick.AddListener(ClickToRestart);
		restartCanvas = GameObject.Find ("restartCanvas");
		popUpMessage = GameObject.Find ("popUpMessage").GetComponent<Text> ();


		btnText = GameObject.Find ("btnText").GetComponent<Text> ();
		msgText = GameObject.Find ("msgText").GetComponent<Text> ();

		difficultySlider = GameObject.Find ("difficultySlider").GetComponent<Slider> ();
		difficultySlider.onValueChanged.AddListener (delegate{DifficultyChangeCheck();});
		difficulty = difficultySlider.transform.Find ("difficulty").gameObject.GetComponent<Text> ();

		reactTime=GameObject.Find ("reactTime").GetComponent<Text> ();

		cubeNumberSlider = GameObject.Find ("cubeNumberSlider").GetComponent<Slider> ();
		cubeNumberSlider.onValueChanged.AddListener (delegate{CubeNumChangeCheck();});
		cubeNum = cubeNumberSlider.transform.Find ("cubeNum").gameObject.GetComponent<Text> ();

		score=GameObject.Find ("score").gameObject.GetComponent<Text> ();

		instruction = GameObject.Find ("instruction").GetComponent<Button>();
		instruction.onClick.AddListener (switchOnInstruction);
		if (GameInfo.levelNum == 1) {
			instruction.gameObject.SetActive (false);
		}

		instructionImg = GameObject.Find ("instructionImage");
		instructionImg.SetActive (false);
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
			if (GameInfo.levelNum == 1) {
				msgText.text = "Welcome! Your goal is to find which cube has the emoji face, after the cubes shift their location for 3 times.";
			}else if(GameInfo.levelNum >=2){
				msgText.text = "Welcome! Your goal is to find which cube has the emoji face, after the cubes shift their location for " + (GameInfo.MaxTravelPeriodNo * 3 + GameInfo.stoppingMove -1).ToString () + " times and return to "+(GameInfo.stoppingMove -1).ToString()+"-step earlier configuration.";
			}
			restartCanvas.SetActive (false);
		}
		else if(gameInfo.phaseNo == 1){
			
			button.gameObject.SetActive (true);
			btnText.text = "Play";
			msgText.text = "After you press [Play], cubes below (one topped with an emoji) begin to shift. Emoji transits once two cubes are connected by a face.";
			restartCanvas.SetActive (false);
		}
		else if(gameInfo.phaseNo == 2){
			
			if (gameInfo.moves == 1) {
				button.gameObject.SetActive (false);
			}
			else if (gameInfo.moves == 2) {
				//GameInfo.SetTargetInvisible ();
			}
				
			restartCanvas.SetActive (false);
		}
		else if (gameInfo.phaseNo == 3) {
			if (gameInfo.moves == GameInfo.stoppingMove) {
				gameInfo.PauseAtFirstMove ();

				button.gameObject.SetActive (true);
				btnText.text = "PLAY AGAIN";

				if (!GameInfo.isTargetFound) {
					GameInfo.SetTargetInvisible ();
					gameInfo.isChooseEnabled = true;
					if (GameInfo.levelNum >= 2) {
						msgText.text = "Where is the emoji " + (GameInfo.stoppingMove - 1).ToString () + "-step earlier? \nIndicate by ONE CLICK at a cube.";
					} else {
						msgText.text = "Where is the emoji? \nIndicate by ONE CLICK at a cube.";
					}
				}
			}
			if (GameInfo.isTargetFound) {
				gameInfo.isChooseEnabled = false;
				msgText.text = "You've found it!";
				restartCanvas.SetActive (true);

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
		if (GameInfo.retryNum >= GameInfo.leastRetryNum) {
			EventManager.TriggerEvent ("OnGeneratingNewGame");
		} else {
			EventManager.TriggerEvent ("OnRetry");
		}
	}

	void Restart(){
		if (gameInfo.phaseNo == 3) {//restart
			gameInfo.Restart ();
		}
	}

	void DifficultyChangeCheck(){
		GameInfo.MaxTravelPeriodNo=(int)((difficultySlider.value)*5+1);
		difficulty.text = ((GameInfo.MaxTravelPeriodNo+1)*3-1).ToString()+ " shifting";
	}

	void CubeNumChangeCheck(){
		GameInfo.CubeNumber=(int)((cubeNumberSlider.value)*2+4);
		cubeNum.text = (GameInfo.CubeNumber).ToString()+ " cubes";
	}

	public static void RefreshScore(){
		score.text = "SCORE "+GameInfo.score.ToString()+"/"+GameInfo.WinningCriterion.ToString();
		if (GameInfo.CheckIfWinningCriterionMet ()) {
			popUpMessage.text = "You win! The next level is unlocked now. Press Q to go on with world exploration";
		} else {
			popUpMessage.text="You win!\nStart a new game or [PLAY AGAIN].";
		}
	}

	public static void ErrorMessage(){
		msgText.text = "Not here!";
	}

	void switchOnInstruction(){
		if (instructionImg.activeSelf) {
			instructionImg.SetActive (false);
		} else {
			instructionImg.SetActive (true);
		}
	}

}
