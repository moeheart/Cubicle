using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutNext : MonoBehaviour {

	public GameObject tutObject;
	public void NextTut(){
		tutObject.GetComponent<TutorialStage>().TutNext();
	}
}
