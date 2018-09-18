using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using MiniJSON;

public class BlockBuilderManager : MonoBehaviour {

	public BaseGrid baseGridPrefab;
	public static BaseGrid baseGridInstance {get; private set;}

	public static int [,] height {get; private set;}

	private static bool isTutorialLevel;
	
	private int currentLevelId;

	// Use this for initialization
	void Awake () {
		BeginGame();
		if (SceneManager.GetActiveScene().name == BlockBuilderConfigs.blockBuilderTutorialSceneName) {
			isTutorialLevel = true;
			currentLevelId = 0;
		}
		else {
			isTutorialLevel = false;
			currentLevelId = BlockBuilderConfigs.id;
		}

		//TODO load the height array, which represents the 3D model
		height = new int[BlockBuilderConfigs.gridSize.x, BlockBuilderConfigs.gridSize.z];
		string jsonFilePath = Path.Combine(Application.streamingAssetsPath, BlockBuilderConfigs.jsonFilename);
		ParseJson(jsonFilePath, height, currentLevelId);
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}*/
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("Block Builder Home");
		}
	}

	private void BeginGame() {
		baseGridInstance = Instantiate (baseGridPrefab, Camera.main.transform) as BaseGrid;
		baseGridInstance.transform.eulerAngles = new Vector3(0, 0 ,0);
		baseGridInstance.transform.position = new Vector3(0, 0, 0);
		// Camera.main.transform.LookAt(baseGridInstance.transform);
		baseGridInstance.Generate();
		// baseGridInstance.transform.localEulerAngles = new Vector3(-90, 90, -90);
	}

	private void RestartGame(){
		StopAllCoroutines();
		Destroy(baseGridInstance.gameObject);
		BeginGame();
	}

	public static void OnComplete() {
		//TODO 
		if (isTutorialLevel) {
			
		}
		else {
			BlockBuilderConfigs.id ++;
		}
		baseGridInstance.OnCompleteBlockBuilderPuzzle();
	}

	public void OnClickExit() {
		SceneManager.LoadScene("Block Builder Home");
	}

	private void ParseJson(string jsonFilePath, int[,] height, int roomId) {
		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		if (dict.ContainsKey("isTutorial")) {
			if (SceneManager.GetActiveScene().name != "Block Builder Tutorial Scene") {
				SceneManager.LoadScene("Block Builder Tutorial Scene", LoadSceneMode.Single);
			}
		}

		List<object> _2DList = ((List<object>) dict["height"]);
		for (int i = 0; i < BlockBuilderConfigs.gridSize.x; ++i) {
			List<object> _list = ((List<object>) _2DList[i]);
			for (int j = 0; j < BlockBuilderConfigs.gridSize.z; ++j) {
				height[i,j] = System.Convert.ToInt32(_list[j]);
			}
		}
	}
}
