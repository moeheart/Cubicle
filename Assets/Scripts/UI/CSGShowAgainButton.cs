using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSGShowAgainButton : MonoBehaviour {

	public CSGTutorialButton okButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick() {
		this.gameObject.SetActive(false);
		okButton.gameObject.SetActive(true);
		okButton.LoadFirst();
	}
}
