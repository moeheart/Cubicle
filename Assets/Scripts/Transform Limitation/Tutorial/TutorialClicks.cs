using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialClicks : MonoBehaviour {

	public GameObject welcomePanel, operationPanel, rotPanel, 
		symPanel, undoPanel, DragPanel, stepsPanel, startPanel;
	
	public GameObject rotController, symController, undoController;

	public GameObject stepText;

	public GameObject tarModel, tarText;

	public void Welcome2Oper(){
		welcomePanel.SetActive(false);
		operationPanel.SetActive(true);
	}

	public void Oper2Rot(){
		operationPanel.SetActive(false);
		rotPanel.SetActive(true);
		rotController.SetActive(true);
		tarModel.SetActive(false);
		tarText.SetActive(false);
	}

	public void Rot2Undo(){
		rotPanel.SetActive(false);
		undoPanel.SetActive(true);
		undoController.SetActive(true);
	}

	public void Undo2Sym(){
		symPanel.SetActive(true);
		undoPanel.SetActive(false);
		symController.SetActive(true);
	}

	public void Sym2Drag(){
		symPanel.SetActive(false);
		DragPanel.SetActive(true);
	}

	public void Drag2Steps(){
		DragPanel.SetActive(false);
		stepsPanel.SetActive(true);
		stepText.SetActive(true);
		tarModel.SetActive(true);
		tarText.SetActive(true);
	}

	public void Steps2Start(){
		DataUtil.UnlockCurrentRoom();
		stepsPanel.SetActive(false);
		startPanel.SetActive(true);
	}
}
