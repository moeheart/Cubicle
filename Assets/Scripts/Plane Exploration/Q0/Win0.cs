using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win0 : MonoBehaviour {

	public GameObject tutObject, player, target;
	private Transform playerTransform, targetTransform;
	public GameObject panel, isometricButton, retryButton;
	public Text resultText;

	void Start(){
		playerTransform = player.gameObject.transform;
		targetTransform = target.gameObject.transform;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			int modelStage = tutObject.GetComponent<TutorialStage>().modelStage;
			if(modelStage == 1){
				tutObject.GetComponent<TutorialStage>().TutNext();
				playerTransform.position = new Vector3(0.5f, 2.04f, 0.5f);
			}else if(modelStage == 2){
				tutObject.GetComponent<TutorialStage>().TutNext();
				playerTransform.position = new Vector3(1.5f, 1.04f, 2.5f);
				targetTransform.position = new Vector3(0.5f, 2.04f, 0.5f);
			}else if(modelStage == 3){
				tutObject.GetComponent<TutorialStage>().TutNext();
				playerTransform.position = new Vector3(0.5f, 2.04f, 0.5f);
				targetTransform.position = new Vector3(1.5f, 1.04f, 2.5f);
			}else{ // 4
				tutObject.GetComponent<TutorialStage>().TutNext();
				panel.SetActive (true);
				resultText.text = "You Win!";
				player.SetActive (false);
				isometricButton.SetActive (true);
				retryButton.SetActive(true);
				target.SetActive (false);
			}
		}
	}


}
