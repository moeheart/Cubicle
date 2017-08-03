using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlockBuilderManager : MonoBehaviour {

	public BaseGrid baseGridPrefab;
	private BaseGrid baseGridInstance;

	// Use this for initialization
	void Start () {
		BeginGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}

	private void BeginGame() {
		baseGridInstance = Instantiate (baseGridPrefab) as BaseGrid;
		StartCoroutine(baseGridInstance.Generate());
	}

	private void RestartGame(){
		StopAllCoroutines();
		Destroy(baseGridInstance.gameObject);
		BeginGame();
	}
}
