using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SelectWorld : MonoBehaviour {

	public string jsonFilename;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select() {
		Configurations.jsonFilename = this.jsonFilename;
		Configurations.saveFilename = Path.ChangeExtension(jsonFilename, ".dat");
		SceneManager.LoadScene("World Scene", LoadSceneMode.Single);
	}
}
