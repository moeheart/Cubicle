using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadButton : MonoBehaviour {
    public GameObject UploadPanel;
    public GameObject BackButton;

    private string path = "Assets/Logs/Unfolding/user_Level1.txt";
    private string uploadurl = "http://127.0.0.1:8000/UploadGameLog/";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ActivatePanel()
    {
        BackButton.SetActive(false);
        UploadPanel.SetActive(true);
    }

    /// <summary>
    /// Interface for the Upload Button.
    /// </summary>
	public void UploadLog() {
        ActivatePanel();

        StartCoroutine("UploadLogFiles");
	}

    /// <summary>
    /// Do the upload operation.
    /// </summary>
    /// <returns></returns>
    IEnumerator UploadLogFiles()
    {
        byte[] LevelData = File.ReadAllBytes(path);

        WWWForm data = new WWWForm();
        data.AddBinaryData("LevelData", LevelData);

        UnityWebRequest www = UnityWebRequest.Post(uploadurl, data);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("File upload complete!");
        }
    }
}
