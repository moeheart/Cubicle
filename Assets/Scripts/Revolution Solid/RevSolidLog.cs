using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using System.IO;
using System.Text;

public class RevSolidLog : MonoBehaviour {

	private static string logFilePath = @"Assets/Logs/Revolution Solid/_RevSolidLogs.txt";

	private static int recordNo;
	private static int trialNum;
	private UnityAction listener;

	StreamWriter writer;

	void Awake(){
		recordNo = 0;
		trialNum = 0;
		listener = new UnityAction (RecordMouseDown);
	}

	void OnEnable(){

		//action: mouse button down,up; restart a game; tutorial on,off
		//detail: freeStrokePath, grading(hit/false/miss), raction time

		EventManager.StartListening ("OnMouseDown",listener);
		EventManager.StartListening ("OnMouseUp",RecordMouseUp);
		EventManager.StartListening ("Grading",RecordGrading);
		EventManager.StartListening ("RecordReactionTime",RecordReactionTime);
		EventManager.StartListening ("Retry",RecordRetry);
		EventManager.StartListening ("EnableTutorial",RecordTutorialOn);
		EventManager.StartListening ("DisableTutorial",RecordTutorialOff);

	}

	void OnDisable(){

		EventManager.StopListening ("OnMouseDown",listener);
		EventManager.StopListening ("OnMouseUp",RecordMouseUp);
		EventManager.StopListening ("Grading",RecordGrading);
		EventManager.StopListening ("RecordReactionTime",RecordReactionTime);
		EventManager.StopListening ("Retry",RecordRetry);
		EventManager.StopListening ("EnableTutorial",RecordTutorialOn);
		EventManager.StopListening ("DisableTutorial",RecordTutorialOff);

	}

	void Start(){
		RecordInitialization ();
		SceneManager.activeSceneChanged += CommitResult;
	}

	void Update(){
		
	}

	void RecordInitialization(){
		writer = new StreamWriter (logFilePath, true);
		writer.WriteLine ("recordNo\ttimeStamp\ttrialNum\tlevel\taction\tdetail\t\n");
	}

	void RecordMouseDown(){
		Debug.Log ("mousedown");
		FormulateResult ("mouseDown", "");
	} 

	void RecordMouseUp(){
		FormulateResult ("mouseRelease", AxisDrawing.pathStringToLog);
		AddToTrialNum ();
	}

	void RecordGrading(){
		FormulateResult ("grading", AxisDrawing.lastGradingResult);
	}

	void RecordReactionTime(){
		FormulateResult ("reactionTime", ActiveObjControl.reactionTimeToLog);
	}

	void RecordRetry(){
		FormulateResult ("retry", "");
	}

	void RecordTutorialOn(){
		FormulateResult ("tutorialEnabled", "");
	}

	void RecordTutorialOff(){
		FormulateResult ("tutorialDisabled", "");
	}

	void FormulateResult(string action,string detail){ 
		recordNo++;
		writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\n",recordNo,Time.time,trialNum,RevSolidGameInfo.levelOfDifficulty,action,detail);
	}

	public void AddToTrialNum(){
		trialNum++;
	}

	void CommitResult(Scene scene1,Scene scene2){
		writer.WriteLine("shift from {0} to {1}",scene1.name, scene2.name);
		writer.Close ();
	}
}
