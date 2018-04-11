using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadButton : MonoBehaviour {
    public GameObject UploadPanel;
    public GameObject BackButton;

    private string path = "Assets/Logs";
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
        //byte[] LevelData = File.ReadAllBytes(path);

        var DInfo = new DirectoryInfo(path);
        var SubDInfos = DInfo.GetDirectories();
        foreach (DirectoryInfo SubDInfo in SubDInfos)
        {
            var Files = SubDInfo.GetFiles();
            foreach (FileInfo f in Files)
            {
                // Use bit operator to check the flag of Hidden in FileAttributes is set.
                bool IsHidden = Convert.ToBoolean(f.Attributes.GetHashCode() & FileAttributes.Hidden.GetHashCode());
                if (!IsHidden)
                {
                    Debug.Log(f.FullName);
                }
            }
        }

        yield return 0;
        /*WWWForm data = new WWWForm();
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
        }*/
    }
}
