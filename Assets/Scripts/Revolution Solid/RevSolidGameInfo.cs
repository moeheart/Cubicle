using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevSolidGameInfo : MonoBehaviour {
	
	protected static int hit;
	protected static int falseStrokeCount;
	//private RevSolidUIControl uiController= new RevSolidUIControl();
	const int MaxFalseCount=4;
	const int LearningThres = 3;
	const int WinningCriterion = 12;

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

	public static int Add2FalseStrokeCount (int scoreAdded){
		falseStrokeCount+=scoreAdded;
		return falseStrokeCount;
	}
		
	public static void CheckEndOfGame(){
		
		if (hit >= WinningCriterion) {
			DataUtil.UnlockCurrentRoom();
			RevSolidUIControl.BroadcastMessage ("CONGRATULATIONS you have unlocked this room. Press Q to quit, or continue enjoying your play.");
		}
		if (hit < WinningCriterion && falseStrokeCount>=MaxFalseCount) {
			RevSolidUIControl.BroadcastMessage ("You failed the game. Press RESTART to refill yourself with determination.");
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
		
}
