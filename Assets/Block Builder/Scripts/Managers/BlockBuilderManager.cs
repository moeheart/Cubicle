using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using MiniJSON;

public class BlockBuilderManager : MonoBehaviour {

	public BaseGrid baseGridPrefab;
	public static GameObject levelCompletePanel;
	public static GameObject gameCompletePanel;
	public static BaseGrid baseGridInstance {get; private set;}

	public static int [,] height {get; private set;}

	private static bool isTutorialLevel;
	
	public static int currentLevelId;

	public GameObject movementPanel;
    
    public CSGTutorialButton csg;


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

		BlockBuilderConfigs.thisLevelRotationMethod = BlockBuilderConfigs.rotationMethod;

		if (currentLevelId >= BlockBuilderConfigs.switchLevelId) {
			if (BlockBuilderConfigs.rotationMethod == BlockBuilderConfigs.RotationMethod.Gyro) {
				BlockBuilderConfigs.thisLevelRotationMethod = BlockBuilderConfigs.RotationMethod.Button;
			}
			else {
				BlockBuilderConfigs.thisLevelRotationMethod = BlockBuilderConfigs.RotationMethod.Gyro;
			}
		}

		//TODO load the height array, which represents the 3D model
		height = new int[BlockBuilderConfigs.gridSize.x, BlockBuilderConfigs.gridSize.z];
		string jsonFilePath = Path.Combine(Application.streamingAssetsPath, BlockBuilderConfigs.jsonFilename);
		ParseJson(jsonFilePath, height, currentLevelId);
		levelCompletePanel = GameObject.Find("Level Complete Panel");
		levelCompletePanel.SetActive(false);
		gameCompletePanel = GameObject.Find("Game Complete Panel");
		gameCompletePanel.SetActive(false);

		if (BlockBuilderConfigs.thisLevelRotationMethod == BlockBuilderConfigs.RotationMethod.Gyro) {
			Camera.main.GetComponent<RotateCameraUsingGyro>().enabled = true;
			Camera.main.GetComponent<RotateCameraUsingButton>().enabled = false;
			movementPanel.SetActive(false);
		}
		else if (BlockBuilderConfigs.thisLevelRotationMethod == BlockBuilderConfigs.RotationMethod.Button) {
			Camera.main.GetComponent<RotateCameraUsingGyro>().enabled = false;
			Camera.main.GetComponent<RotateCameraUsingButton>().enabled = true;
			movementPanel.SetActive(true);
		}


		if (isTutorialLevel) {
			Camera.main.GetComponent<RotateCameraUsingGyro>().enabled = true;
			Camera.main.GetComponent<RotateCameraUsingButton>().enabled = true;
            
			movementPanel.SetActive(true);
		}
		
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
		baseGridInstance = Instantiate (baseGridPrefab) as BaseGrid;

		baseGridInstance.Generate();
		baseGridInstance.transform.eulerAngles = new Vector3(0, 0, 0);
		baseGridInstance.transform.position = new Vector3(0, 0, 0);
		Camera.main.transform.LookAt(baseGridInstance.transform);
		Camera.main.transform.Rotate(Vector3.right, 45);
		ViewUtil.PlaceCameraFromRotation(Camera.main.transform, BlockBuilderConfigs.distanceToBaseGrid);
        
        baseGridInstance.csg = csg;
		// Camera.main.transform.RotateAround(baseGridInstance.transform.position, Vector3.right, 30);
		// baseGridInstance.transform.localEulerAngles = new Vector3(-20, 0 ,0);
		// baseGridInstance.transform.localEulerAngles = new Vector3(-90, 90, -90);
	}

	private void RestartGame(){
		StopAllCoroutines();
		Destroy(baseGridInstance.gameObject);
		BeginGame();
	}

	public static void OnComplete() {
		baseGridInstance.OnCompleteBlockBuilderPuzzle();
		if (currentLevelId == BlockBuilderConfigs.totalLevels) {
			gameCompletePanel.SetActive(true);
			return;
		}
		levelCompletePanel.SetActive(true);
		//TODO 
		if (isTutorialLevel) {
			
		}
		else {
			BlockBuilderConfigs.id ++;
		}
		
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
