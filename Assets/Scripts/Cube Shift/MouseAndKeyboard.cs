﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseAndKeyboard : MonoBehaviour {

	//Text cubeHit;
	private GameInfo gameInfo=GameInfo.getInstance();
	public static string hitCubeNum;

	void OnEnable(){
		EventManager.StartListening ("OnChoosing",OnChoosing);
	}

	void OnDisable(){
		EventManager.StopListening ("OnChoosing",OnChoosing);
	}
	// Use this for initialization
	void Start () {
		//cubeHit = GameObject.Find ("cubeHit").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)&&Input.GetMouseButtonDown(0)) {
			if (gameInfo.isChooseEnabled) {
				/*
				if (hit.collider != null) {
					cubeHit.text="Cube #" + hit.collider.name;
				}
				*/
				hitCubeNum = hit.collider.name;
				EventManager.TriggerEvent ("OnChoosing");

			}
		}
		CheckEndOfGame (GameInfo.reactTime);
	}

	void OnChoosing(){
		if (hitCubeNum==gameInfo.targetIdx.ToString()) {
			//cubeHit.text+=" Correct";
			GameInfo.SetTargetVisible();
			GameInfo.isTargetFound = true;
			//gameInfo.isChooseEnabled = false;
			GameInfo.Add2Score();
			UIControl.RefreshScore ();
		}
	}

	void CheckEndOfGame(float reactTime){
		if (GameInfo.CheckIfWinningCriterionMet()) {
			DataUtil.UnlockCurrentRoom();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			Time.timeScale = 1;
			SceneManager.LoadScene("World Scene");

		}
	}
}
