using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using MiniJSON;

public class BlockBuilderManager : MonoBehaviour {

	public BaseGrid baseGridPrefab;
    public DrawingHandler drawinghandlersample;
	public static GameObject levelCompletePanel;
	public static BaseGrid baseGridInstance {get; private set;}

	public static int [,] height {get; private set;}

	private static bool isTutorialLevel;
    private static bool isCompetition;
	
	private int currentLevelId;
    
    public TcpComm tcp;
    public static TcpComm staticTcp;

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
		levelCompletePanel = GameObject.Find("Level Complete Panel");
		levelCompletePanel.SetActive(false);
        print("start setting tcp...");
        staticTcp = tcp;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}*/
		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("Block Builder Home");
		}
        if (tcp.receivedString.Length != 0 && tcp.receivedString[0] == 's') {
            print("Data received, ready to start");
            string s = tcp.receivedString;
            for (int x = 0; x < BlockBuilderConfigs.gridSize.x; ++x) {
                for (int z = 0; z < BlockBuilderConfigs.gridSize.z; ++z) {
                    height[x, z] = (int)(s[x*3+z+1] - '0');
                }
            }
            print(height);
            currentLevelId = -1;
            tcp.receivedString = "";
            drawinghandlersample.updateTarget();
            StopAllCoroutines();
            Destroy(baseGridInstance.gameObject);
            BeginGame();
            print("Start complete...?");
        }
        if (tcp.receivedString.Length != 0 && tcp.receivedString[0] == 'r') {
            StopAllCoroutines();
            Destroy(baseGridInstance.gameObject);
            BeginGame();
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
		levelCompletePanel.SetActive(true);
		//TODO 
		if (isTutorialLevel) {
			
		}
		else {
			BlockBuilderConfigs.id ++;
		}
		baseGridInstance.OnCompleteBlockBuilderPuzzle();
        staticTcp.SendMessageTcp("w");
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
    
    public void sendLevelInfo(){
        string s = baseGridInstance.getCells();
        staticTcp.SendMessageTcp('q'+s);
    }
    
}
