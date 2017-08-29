using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;

public class DrawingHandler : MonoBehaviour {

	public GameObject targetTopViewPanel;
	public GameObject currentTopViewPanel;
	public GameObject targetFrontViewPanel;
	public GameObject currentFrontViewPanel;
	public GameObject targetRightViewPanel;
	public GameObject currentRightViewPanel;

	private Dictionary<Segment,LineType> targetTopView;
	private Dictionary<Segment,LineType> targetFrontView;
	private Dictionary<Segment,LineType> targetRightView;

	private string jsonFilePath;

	private string saveFilePath;

	private int id;

	private Dictionary<string, object> gameState;

	public int[,] height {get; private set;}

	// Use this for initialization
	void Start () {
		jsonFilePath = Application.streamingAssetsPath + "/Puzzles.json";
		/*
		saveFilePath = Path.Combine(Application.persistentDataPath, "game.dat");

		//Get the id
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open(saveFilePath, FileMode.Open);
		gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
		stream.Close();
		id = (int)gameState["current room id"];
		*/

		id = DataUtil.GetCurrentRoomId();

		height = new int[BlockBuilderConfigs.gridSize.x, BlockBuilderConfigs.gridSize.z];
		ParseJson(jsonFilePath, height, id);
		Dictionary<IntVector3, bool> targetBlock = To3DMapping(height);
		
		targetTopView = ThreeView.GetTopView(targetBlock);
		targetFrontView = ThreeView.GetFrontView(targetBlock);
		targetRightView = ThreeView.GetRightView(targetBlock);

		targetTopViewPanel.GetComponent<ViewPanel>().DrawView(targetTopView);
		targetFrontViewPanel.GetComponent<ViewPanel>().DrawView(targetFrontView);
		targetRightViewPanel.GetComponent<ViewPanel>().DrawView(targetRightView);
	}
	
	private void ParseJson(string jsonFilePath, int[,] height, int roomId) {
		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		List<object> _2DList = ((List<object>) dict["height"]);
		for (int i = 0; i < BlockBuilderConfigs.gridSize.x; ++i) {
			List<object> _list = ((List<object>) _2DList[i]);
			for (int j = 0; j < BlockBuilderConfigs.gridSize.z; ++j) {
				height[i,j] = System.Convert.ToInt32(_list[j]);
			}
		}
	}

	public void DrawMultiView(BaseGridCell[,] cells) {
		Dictionary<IntVector3, bool> cubes = To3DMapping(To2DMapping(cells));
		bool isTopViewCorrect, isFrontViewCorrect, isRightViewCorrect;

		Dictionary<Segment, LineType> topView = ThreeView.GetTopView(cubes);
		currentTopViewPanel.GetComponent<ViewPanel>().DrawView(topView);
		isTopViewCorrect = CompareCurrentAndTargetView(topView, targetTopView);
		targetTopViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(isTopViewCorrect);

		Dictionary<Segment, LineType> frontView = ThreeView.GetFrontView(cubes);
		currentFrontViewPanel.GetComponent<ViewPanel>().DrawView(frontView);
		isFrontViewCorrect = CompareCurrentAndTargetView(frontView, targetFrontView);
		targetFrontViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(isFrontViewCorrect);

		Dictionary<Segment, LineType> rightView = ThreeView.GetRightView(cubes);
		currentRightViewPanel.GetComponent<ViewPanel>().DrawView(rightView);
		isRightViewCorrect = CompareCurrentAndTargetView(rightView, targetRightView);
		targetRightViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(isRightViewCorrect);

		if (isTopViewCorrect && isFrontViewCorrect && isRightViewCorrect) {
			BlockBuilderManager.OnComplete();
			//TODO
			//Unlock this room!
			//To unlock this room, we only need to store this id in the savefile
			DataUtil.UnlockCurrentRoom();
			/*((List<int>) gameState["unlocked rooms"]).Add(id);
			FileStream stream = File.Create(saveFilePath);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, gameState);
			stream.Close();*/
		}
	}

	private bool CompareCurrentAndTargetView(Dictionary<Segment, LineType> currentView, Dictionary<Segment,LineType> targetView) {
		foreach (KeyValuePair<Segment, LineType> entry in currentView) {
			if (targetView[entry.Key] != entry.Value) {
				return false;
			}
		}
		return true;
	}

	private int[,] To2DMapping(BaseGridCell[,] cells) {
		int[,] mapping = new int[BlockBuilderConfigs.gridSize.x, BlockBuilderConfigs.gridSize.z];
		for (int x = 0; x < BlockBuilderConfigs.gridSize.x; ++x) {
			for (int z = 0; z < BlockBuilderConfigs.gridSize.z; ++z) {
				int height = cells[x,z].height;
				mapping[x,z] = height;
			}
		}
		return mapping;
	}

	private Dictionary<IntVector3, bool> To3DMapping(int[,] heightArray) {
		Dictionary<IntVector3, bool> cubes = new Dictionary<IntVector3, bool>();
		for (int x = 0; x < BlockBuilderConfigs.gridSize.x; ++x) {
			for (int z = 0; z < BlockBuilderConfigs.gridSize.z; ++z) {
				int height = heightArray[x,z];
				for (int h = 0; h < height; ++h) {
					IntVector3 coords = new IntVector3(x,z,h);
					cubes[coords] = true;
				}
			}
		}
		return cubes;
	}
	
}
