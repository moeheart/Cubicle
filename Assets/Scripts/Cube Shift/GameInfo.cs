//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MiniJSON;

public class GameInfo
{

	public Cube cube=Cube.getInstance();

	public int targetIdx;
	public static GameObject target;
	public static bool isTargetFound;
	public bool isChooseEnabled;
	public int moves;//1~3
	public int phaseNo;//{Instructions,Travelling,Clicking};//0-"proceed",1-"play", 2-travelling, >=3-clicking
	public static int travelPeriodNo;

	//difficulty
	public static int MaxTravelPeriodNo;
	public static int CubeNumber;

	public float lastUpdateTime,currTime;
	public bool isMoving;
	public const float stillDuration = 1.0f;
	public const float shiftDuration = 2.0f;

	public bool[] isShiftDone=new bool[Cube.MaxCubeNumber*3];

	public bool needDestroyCubes;
	public bool needReInstantiate;

	public bool hasPhase3Begun;
	public float beginTime;
	public static float reactTime;
	public static float score;

	static float WinningCriterion=1.0f;

	public static bool isTutorialModeOn;
	//单例模式
	private static GameInfo instance = new GameInfo ();
	private GameInfo(){
		score = 0;
		isTutorialModeOn = true;
	}
	public static GameInfo getInstance(){
		return instance;
	}

	public void Init(){
		//initialize GameInfo records
		CheckLevel();

		isTargetFound = false;
		isChooseEnabled = false;

		lastUpdateTime = Time.time;
		isMoving = true;
		moves = 1;
		phaseNo = 0;
		hasPhase3Begun = false;
		reactTime = 0.0f;
		travelPeriodNo = 1;
		UndoneShifts();

		needDestroyCubes=true;
		needReInstantiate = true;
	
		SetTargetVisible ();
	}

	public void Play(){

		isTargetFound = false;
		isChooseEnabled = false;
		lastUpdateTime = Time.time;
		isMoving = true;
		moves = 1;

		phaseNo = 2;
	}

	public void Retry(){
		Init ();
		//no need to destroy old cubes
		needDestroyCubes=false;
		//no need to instantiate new cubes
		needReInstantiate=false;
	}

	public void Restart(){//Only referenced when 3 cubes already exist

		cube.CubeNumber = CubeNumber;
		cube.InitializePos ();
		cube.FindAdjoiningCubes ();
		Init ();
	}
		
	public void PauseGame(){
		if(!Input.GetKey(KeyCode.Q))
			Time.timeScale = 0;
	}

	public void PauseAtFirstMove(){
		moves = 1;
		PauseGame ();
	}
	public void ResumeGame(){
		Time.timeScale = 1;
	}

	public void UpdateTarget(int targetIdx){
		//SetAllTreeVisible ();
		target = cube.trees [targetIdx];
		SetAllTreeInvisible ();
		if (isTutorialModeOn) {
			SetTargetVisible ();
		}
	}

	void SetAllTreeInvisible(){
		for (int i = 0; i < cube.CubeNumber; i++) {
			cube.trees[i].SetActive(false);
		}
	}

	public static void SetTargetVisible(){
		target.SetActive(true);
	}

	public static void SetTargetInvisible(){
		target.SetActive(false);
	}

	public void UndoneShifts(){
		for (int i = 0; i < cube.CubeNumber; i++) {
			for (int mvs = 1; mvs <= 3; mvs++) {
				isShiftDone [i * 3 + mvs - 1] = false;
			}
		}
	}

	void CheckLevel(){
		MaxTravelPeriodNo = ParseJson("shiftNumPerTrial");
		CubeNumber = ParseJson ("cubeNum");
	}

	public static int ParseJson(string lineTitle){
		int roomId=DataUtil.GetCurrentRoomId();
		string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Puzzles.json");

		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		return System.Convert.ToInt32 (dict [lineTitle]);
	}

	public static void Add2Score(){
		score+=1.0f;
	}

	public static bool CheckIfWinningCriterionMet(){
		if (score > WinningCriterion) {
			return true;
		} else {
			return false;
		}
	}
		
}