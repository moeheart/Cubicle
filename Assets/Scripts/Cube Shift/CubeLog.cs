using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class CubeLog : MonoBehaviour {

	private static string logFilePath = @"Assets/Logs/Cube Shift/_CubeShiftLogs.txt";

	private static int recordNo;
	private static int trialNum;
	StreamWriter writer;

	void Awake(){
		recordNo = 0;
		trialNum = 0;
	}

	void OnEnable(){

		//action: 
		//  start shifting(observing time);
		//  choose(reaction time, right/wrong, number of chosen cube); 
		//  new game(cube count, period count); 
		//  retry

		EventManager.StartListening ("OnShiftingStart",RecordShiftingStart);
		EventManager.StartListening ("OnProceedingTutorial",RecordProceedingTutorial);
		EventManager.StartListening ("OnChoosing",RecordChoosing);
		EventManager.StartListening ("OnGeneratingNewGame",RecordGeneratingNewGame);
		EventManager.StartListening ("OnRetry",RecordRetry);
	}

	void OnDisable(){

		EventManager.StopListening ("OnShiftingStart",RecordShiftingStart);
		EventManager.StopListening ("OnProceedingTutorial",RecordProceedingTutorial);
		EventManager.StopListening ("OnChoosing",RecordChoosing);
		EventManager.StopListening ("OnGeneratingNewGame",RecordGeneratingNewGame);
		EventManager.StopListening ("OnRetry",RecordRetry);

	}

	void Start(){
		RecordInitialization ();
		SceneManager.activeSceneChanged += CommitResult;
	}

	void Update(){

	}

	void RecordInitialization(){
		writer = new StreamWriter (logFilePath, true);
		writer.WriteLine ("\n\n{0}\n",System.DateTime.Now.ToString());
		writer.WriteLine ("recordNo\ttimeStamp\ttrialNum\tlevel\taction\tdetail\t\n");
	}

	void RecordChoosing(){
		FormulateResult ("choose", GameInfo.reactTime.ToString()+", "+(GameInfo.isTargetFound?"correct":"wrong")+", "+MouseAndKeyboard.hitCubeNum);
	} 


	void RecordGeneratingNewGame(){
		FormulateResult ("newGameGenerated", GameInfo.CubeNumber.ToString()+", " + GameInfo.MaxTravelPeriodNo.ToString());
		AddToTrialNum ();
	}

	void RecordRetry(){
		FormulateResult ("retry", "");
		AddToTrialNum ();
	}

	void RecordShiftingStart(){
		FormulateResult ("startShifting","");
	}

	void RecordProceedingTutorial(){
		FormulateResult ("click'Proceed'Btn","");
	}

	void FormulateResult(string action,string detail){ 
		recordNo++;
		writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t\n",recordNo,Time.realtimeSinceStartup,trialNum,action,detail);
	}

	void AddToTrialNum(){
		trialNum++;
	}

	void CommitResult(Scene scene1,Scene scene2){
		writer.WriteLine("shift from {0} to {1}",scene1.name, scene2.name);
		writer.Close ();
	}
}
