using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class ConfirmButton : MonoBehaviour {
    public GameObject UploadPanel;
    public GameObject StuIDPanel;
    public Text StuID;
    public UploadButton Uploader;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConfirmStuID()
    {
        Uploader.SetStuID(StuID.text);
        UploadPanel.SetActive(true);
        StuIDPanel.SetActive(false);
    }
}
