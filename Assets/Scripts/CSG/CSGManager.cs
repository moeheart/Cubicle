using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(ObjectsManager))]
public class CSGManager : MonoBehaviour {

	public static ObjectsManager objectsManager {get; private set;}

	// Use this for initialization
	void Start () {
		objectsManager = this.GetComponent<ObjectsManager>();
		objectsManager.LoadGameObjects();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}
}
