using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using System.IO;
using System.Text;

public class RevSolidLog : MonoBehaviour {

	private static string logFilePath;

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
		EventManager.StartListening ("Retry",RecordRetry);
		EventManager.StartListening ("EnableTutorial",RecordTutorialOn);
		EventManager.StartListening ("DisableTutorial",RecordTutorialOff);
		EventManager.StartListening ("Qdown",CommitResult);
	}

	void OnDisable(){

		EventManager.StopListening ("OnMouseDown",listener);
		EventManager.StopListening ("OnMouseUp",RecordMouseUp);
		EventManager.StopListening ("Grading",RecordGrading);
		EventManager.StopListening ("Retry",RecordRetry);
		EventManager.StopListening ("EnableTutorial",RecordTutorialOn);
		EventManager.StopListening ("DisableTutorial",RecordTutorialOff);
		EventManager.StopListening ("Qdown",CommitResult);
	}

	void Start(){
		GenerateLogFilePath ();
		RecordInitialization ();
	}

	void Update(){
		
	}

	void GenerateLogFilePath(){
		int id = 0;
		do {
			id++;
			logFilePath = @"Assets/Logs/Revolution Solid/_RevSolidLogs" + id + ".txt";
		} while(File.Exists (logFilePath));
	}

	void RecordInitialization(){
		writer = new StreamWriter (logFilePath, true);
		writer.WriteLine ("\n\n{0}\n",System.DateTime.Now.ToString());
		writer.WriteLine ("recordNo\ttimeStamp\ttrialNum\tlevel\taction\tdetail\t\n");
	}

	void RecordMouseDown(){
		FormulateResult ("mouseDown", "");
		RecordReactionTime ();
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
		writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t\n",recordNo,Time.realtimeSinceStartup,trialNum,RevSolidGameInfo.levelOfDifficulty,action,detail);
	}

	void AddToTrialNum(){
		trialNum++;
	}

	void CommitResult(){
		writer.WriteLine("press Q");
		writer.Close ();
	}
}
