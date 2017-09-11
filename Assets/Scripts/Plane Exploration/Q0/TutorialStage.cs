using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStage : MonoBehaviour {

	public int tutStage, modelStage;
	public Text tutText;
	public Camera hintCamera;
	public GameObject tutPanel, okButton, player;

	void Start () {
		tutStage = 0;
	}
	
	
	void Update () {
		
		switch(tutStage){
			case 0:
				tutText.text = "Welcome! In this module, your goal" + 
					" is to orient the yellow square to the orange one.";
				break;
			case 1:
				tutText.text = "You can see the three views of your map.";
				break;
			case 2:
				tutText.text = "You have to visualize its 3d model" + 
					" to direct your square correctly and safely.";
				hintCamera.gameObject.SetActive(true);
				break;
			case 3:
				tutText.text = "Now, try to use WSAD or direction keys " + 
					"to move the yellow square to the orange one!";
					player.GetComponent<Player0>().enabled = true;
					okButton.SetActive(false);
				break;
			case 4:
				tutText.text = "Good job! Sometimes, you have to go down a slope" + 
					" to access the orange square. Have a go! ";
				break;
			case 5:
				tutText.text = "Great! You can also try going up a slope.";
				break;
			case 6:
				tutText.text = "However, going across the fault is fatal.";
				break;
			case 7:
				tutText.text = "You cannot see the perspective view of the map in a real time. " + 
					"Yet every time you win or fail the game, you can " +
					"click on “perspective” button to have a perspective view.";
				break;
			case 8:
				tutText.text = "Now you can start your first level!";
				break;
		}
	}

	public void TutNext(){
		if(tutStage < 8)
			tutStage ++;
		UpdateModelStage();
	}

	public void TutBefore(){
		if(tutStage >= 0)
			tutStage --;
		UpdateModelStage();
	}

	void UpdateModelStage(){
		switch(tutStage){
			case 0 :
				modelStage = 1;
				break;
			case 1 :
				modelStage = 1;
				break;
			case 2 :
				modelStage = 1;
				break;
			case 3 :
				modelStage = 1;
				break;
			case 4 :
				modelStage = 2;
				break;
			case 5 :
				modelStage = 3;
				break;
			case 6 :
				modelStage = 4;
				break;
			case 7 :
				modelStage = 4;
				break;
			case 8 :
				modelStage = 4;
				break;
		}
	}

}
