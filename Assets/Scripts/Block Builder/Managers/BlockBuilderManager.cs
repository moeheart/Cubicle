using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlockBuilderManager : MonoBehaviour {

	public BaseGrid baseGridPrefab;
	public static BaseGrid baseGridInstance {get; private set;}

	// Use this for initialization
	void Start () {
		BeginGame();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}*/
		if (Input.GetKeyDown(KeyCode.Q)) {
			OnClickExit();
		}
	}

	private void BeginGame() {
		baseGridInstance = Instantiate (baseGridPrefab) as BaseGrid;
		// StartCoroutine(baseGridInstance.Generate());
		baseGridInstance.GenerateCells();
	}

	private void RestartGame(){
		StopAllCoroutines();
		Destroy(baseGridInstance.gameObject);
		BeginGame();
	}

	public static void OnComplete() {
		baseGridInstance.OnCompleteBlockBuilderPuzzle();
	}

	public void OnClickExit() {
		SceneManager.LoadScene("World Scene");
	}
}
