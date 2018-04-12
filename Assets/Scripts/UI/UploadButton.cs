using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadButton : MonoBehaviour {
    public GameObject StuIDPanel;
    public GameObject BackButton;
    public Text Hint;

    string StuID = "";
    string OriginalText = "";
    private string LogDirectory = "Assets/Logs/";
    private string uploadurl = "http://127.0.0.1:8000/UploadGameLog/";

	// Use this for initialization
	void Start () {
        OriginalText = Hint.text;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivatePanel()
    {
        Hint.text = OriginalText;
        BackButton.SetActive(false);
        StuIDPanel.SetActive(true);
    }

    /// <summary>
    /// Interface for the Upload Button.
    /// </summary>
	public void UploadLog() {
        StartCoroutine("UploadLogFiles");
	}

    /// <summary>
    /// Do the upload operation.
    /// </summary>
    /// <returns></returns>
    IEnumerator UploadLogFiles()
    {
        WWWForm data = new WWWForm();

        var DInfo = new DirectoryInfo(LogDirectory);
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
                    byte[] LevelData = File.ReadAllBytes(f.FullName);
                    data.AddBinaryData(StuID + "_" + f.Name, LevelData);
                }
            }
        }
       
        UnityWebRequest www = UnityWebRequest.Post(uploadurl, data);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Hint.text = www.error;
        }
        else if (www.isHttpError)
        {
            Hint.text = "Server internel error! \n Please try again or contact the administrator.";
        }
        else
        {
            Hint.text = "File upload completed!";
        }

        BackButton.SetActive(true);
    }

    /// <summary>
    /// Set Student ID.
    /// </summary>
    /// <param name="_StuID"></param>
    public void SetStuID(string _StuID)
    {
        StuID = _StuID;
    }
}
