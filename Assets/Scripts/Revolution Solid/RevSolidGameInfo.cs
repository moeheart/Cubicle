using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevSolidGameInfo : MonoBehaviour {
	
	protected static int hit;
	protected static int falseStrokeCount;
	//private RevSolidUIControl uiController= new RevSolidUIControl();
	public const int MaxFalseCount=8;
	public static int GuidingTrialNum = 1;
	public const int WinningCriterion = 8;

	public static float RecoverInterval=5.0f;
	public static float MaxReactionTime=9999.0f;
	public static int MaxPolygonNum=12;
	public static int MaxPanelNum;

	public static int polygonGenerationCount;
	public static int polygonGenerationCountSinceLastTutorial;

	public static float levelOfDifficulty;//(0.5,1)(1.5,2)

	// Use this for initialization
	void Awake () {
		InitializeHit ();
		falseStrokeCount = 0;
		polygonGenerationCount = 0;
		polygonGenerationCountSinceLastTutorial = 0;
	}

	void OnEnable(){
		EventManager.StartListening ("Qdown",GoBackToWorld);
	}

	void OnDisable(){
		EventManager.StopListening ("Qdown",GoBackToWorld);
	}

	public static void InitializeHit(){
		hit = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static int Add2TotalHit (int scoreAdded){
		hit+=scoreAdded;
		return hit;
	}

	public static void Add2FalseStrokeCount (int scoreAdded){
		falseStrokeCount+=scoreAdded;
	}
		
	public static void CheckEndOfGame(){
		
		if (hit >= WinningCriterion) {
			DataUtil.UnlockCurrentRoom();
			RevSolidUIControl.defaultString = "You have unlocked this room! Press Q to quit.";
		}

		if (hit < WinningCriterion) {
			RevSolidUIControl.defaultString = "";
			if (falseStrokeCount >= MaxFalseCount) {
				Time.timeScale = 0;
				RevSolidUIControl.ShowRetry ();
			}
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			EventManager.TriggerEvent ("Qdown");

		}
	}

	void GoBackToWorld(){
		Time.timeScale = 1;
		SceneManager.LoadScene("World Scene");
	}
	 
	public static bool IfNoviceGuideEnds(){
		return polygonGenerationCountSinceLastTutorial >= GuidingTrialNum;
	}

	public static bool WhenNoviceGuideEnds(){
		return polygonGenerationCount >= GuidingTrialNum && hit==GuidingTrialNum;
	}

	public virtual void Retry(){
		hit = 0;
		falseStrokeCount = 0;
	}
	public static int GetLODByInt(){
		return Mathf.CeilToInt(levelOfDifficulty);
	}
		
}
