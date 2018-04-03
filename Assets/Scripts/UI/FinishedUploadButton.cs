using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedUploadButton : MonoBehaviour {
    public GameObject UploadPanel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FinishUpload() {
        UploadPanel.SetActive(false);
    }
}
