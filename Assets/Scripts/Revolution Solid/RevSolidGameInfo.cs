using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevSolidGameInfo : MonoBehaviour {
	
	protected static int hit;
	protected static int falseStrokeCount;
	//private RevSolidUIControl uiController= new RevSolidUIControl();
	public const int MaxFalseCount=6;
	const int LearningThres = 3;
	public const int WinningCriterion = 6;

	public static float RecoverInterval=3.0f;
	public static float MaxReactionTime=9999.0f;
	public static int MaxPolygonNum=12;
	public static int MaxPanelNum;

	public static float levelOfDifficulty;//(0.5,1)(1.5,2)

	// Use this for initialization
	void Awake () {
		hit = 0;
		falseStrokeCount = 0;
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
			RevSolidUIControl.BroadcastMsg ("CONGRATULATIONS you have unlocked this room. Press Q to quit, or continue enjoying your play.");
		}
		if (hit < WinningCriterion && falseStrokeCount>=MaxFalseCount) {
			RevSolidUIControl.BroadcastMsg ("You failed the game. Press RESTART to refill yourself with determination.");
			Time.timeScale = 0;
			RevSolidUIControl.ShowRetryButton ();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}

	public static bool CheckIfPlayerLearned(){
		return hit >= LearningThres;
	}

	public static bool CheckIfPlayerFailsMuch(){
		return falseStrokeCount >= MaxFalseCount-2 && Tutorial.isTutorialModeOn == false;
	}

	public virtual void Retry(){
		hit = 0;
		falseStrokeCount = 0;
	}
	public static int GetLODByInt(){
		return Mathf.CeilToInt(levelOfDifficulty);
	}
		
}
