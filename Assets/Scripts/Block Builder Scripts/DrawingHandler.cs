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

	private string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";

	private string saveFilePath;

	// Use this for initialization
	void Start () {

		saveFilePath = Path.Combine(Application.persistentDataPath, "game.dat");

		Dictionary<string, object> gameState;

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open(saveFilePath, FileMode.Open);
		gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
		stream.Close();

		int id = (int)gameState["current room id"];

		int[,] height = new int[BlockBuilderConfigs.gridSize.x, BlockBuilderConfigs.gridSize.z];
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
		bool flag;

		Dictionary<Segment, LineType> topView = ThreeView.GetTopView(cubes);
		currentTopViewPanel.GetComponent<ViewPanel>().DrawView(topView);
		flag = CompareCurrentAndTargetView(topView, targetTopView);
		targetTopViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(flag);

		Dictionary<Segment, LineType> frontView = ThreeView.GetFrontView(cubes);
		currentFrontViewPanel.GetComponent<ViewPanel>().DrawView(frontView);
		flag = CompareCurrentAndTargetView(frontView, targetFrontView);
		targetFrontViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(flag);

		Dictionary<Segment, LineType> rightView = ThreeView.GetRightView(cubes);
		currentRightViewPanel.GetComponent<ViewPanel>().DrawView(rightView);
		flag = CompareCurrentAndTargetView(rightView, targetRightView);
		targetRightViewPanel.GetComponent<ViewPanel>().ChangeColorOnCompare(flag);
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
