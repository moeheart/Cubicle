using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSGTutorialButton : MonoBehaviour {

	public Text[] tutorialTexts;

	public Button showAgainButton;

	private int index = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadFirst() {
		index = 0;
		tutorialTexts[index].enabled = true;
	}

	public void LoadNext() {
		tutorialTexts[index].enabled = false;
		++index;
		if (index < tutorialTexts.Length) {
			tutorialTexts[index].enabled = true;
		}
		else {
			this.gameObject.SetActive(false);
			showAgainButton.gameObject.SetActive(true);
		}
	}
}
