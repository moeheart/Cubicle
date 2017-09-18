using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public World worldPrefab;

	public static World worldInstance {get; private set;}
	private string levelJsonFilePath;

	// Use this for initialization
	void Start () {
		Debug.Log(Application.streamingAssetsPath);
		levelJsonFilePath = Path.Combine(Application.streamingAssetsPath, Configurations.jsonFilename);
		worldInstance = Instantiate(worldPrefab) as World;
		worldInstance.GenerateWorld(levelJsonFilePath);
		worldInstance.LoadData();
		//worldInstance.SaveData();

		string logPath = Path.Combine(Application.persistentDataPath, "Logs");

		Directory.CreateDirectory(Path.Combine(logPath, "CSG"));
		Directory.CreateDirectory(Path.Combine(logPath, "Block Builder"));
		Directory.CreateDirectory(Path.Combine(logPath, "View Point"));
		Directory.CreateDirectory(Path.Combine(logPath, "Transform Limitation"));
		Directory.CreateDirectory(Path.Combine(logPath, "Plane Exploration"));
		Directory.CreateDirectory(Path.Combine(logPath, "Revolution Solid"));
		Directory.CreateDirectory(Path.Combine(logPath, "Cube Shift"));
		Directory.CreateDirectory(Path.Combine(logPath, "Unfolding"));

		/*Directory.CreateDirectory("Assets/Logs/CSG");
		Directory.CreateDirectory("Assets/Logs/Block Builder");
		Directory.CreateDirectory("Assets/Logs/View Point");
		Directory.CreateDirectory("Assets/Logs/Transform Limitation");
		Directory.CreateDirectory("Assets/Logs/Plane Exploration");
		Directory.CreateDirectory("Assets/Logs/Revolution Solid");
		Directory.CreateDirectory("Assets/Logs/Cube Shift");*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartGame() {
		worldInstance.RestartGame();
	}

	public void ExitToStartScene() {
		SceneManager.LoadScene("Start Scene", LoadSceneMode.Single);
	}

	/*public void SaveGameState() {
		//TODO
		Dictionary<string, object> gameState = new Dictionary<string, object>();
		//worldInstance.SaveData(gameState);

		FileStream stream = File.Create(saveFilePath);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, gameState);
		stream.Close();
	}

	public void LoadGameState() {
		if (!File.Exists(saveFilePath)) {
			Debug.Log("No saved game.");
			return;
		}

		Dictionary<string, object> gameState;

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open(saveFilePath, FileMode.Open);
		gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
		stream.Close();

		//worldInstance.LoadData(gameState);

		Debug.Log("Successfully Loaded save game...!!");
	}*/

	/*void ReadAndParse() {
		string jsonString = File.ReadAllText(levelJsonFilePath);
		ParseJsonString(jsonString);
	}

	void ParseJsonString(string data) {
		Dictionary<string, object> dict;
		dict = Json.Deserialize(data) as Dictionary<string,object>;

		//The first run builds the walls and floors of the rooms
		foreach (KeyValuePair<string, object> entry in dict) {
			//entry.key should be a string, which is the roomId
			//entry.value should be a dictionary type, which contains the room information

			int roomId = int.Parse(entry.Key);
			float posX, posY, posZ;
			float dimX, dimY, dimZ;
			float colorR, colorG, colorB, colorA;

			Dictionary<string, object> entryValueDict = (Dictionary<string,object>)entry.Value;
			List<object> positionList = ((List<object>) entryValueDict["position"]);
			List<object> dimensionList = ((List<object>) entryValueDict["dimension"]);
			List<object> colorList = ((List<object>) entryValueDict["color"]);

			posX = System.Convert.ToSingle(positionList[0]) * lengthPerUnit;
			posY = System.Convert.ToSingle(positionList[1]) * lengthPerUnit;
			posZ = System.Convert.ToSingle(positionList[2]) * lengthPerUnit;
			dimX = System.Convert.ToSingle(dimensionList[0]);
			dimY = System.Convert.ToSingle(dimensionList[1]);
			dimZ = System.Convert.ToSingle(dimensionList[2]);
			colorR = System.Convert.ToSingle(colorList[0]);
			colorG = System.Convert.ToSingle(colorList[1]);
			colorB = System.Convert.ToSingle(colorList[2]);
			colorA = System.Convert.ToSingle(colorList[3]);
			
			Vector3 position = new Vector3(posX, posY, posZ);
			Vector3 dimension = new Vector3(dimX, dimY, dimZ);
			Color color = new Color(colorR, colorG, colorB, colorA);

			Room room = ScriptableObject.CreateInstance<Room>();
			room.Initialize(roomId, position, dimension, color);
			rooms.Insert(roomId, room);

			worldInstance.BuildRoom(position, dimension, color);
		}
		
		//The second run replaces blocks with doors between adjacent rooms
		foreach (KeyValuePair<string, object> entry in dict) {
			int id1 = int.Parse(entry.Key);
			Room room1 = rooms[id1];
			//Dictionary<string, object> entryValueDict = (Dictionary<string,object>)entry.Value;
			//List<object> adjacentRooms = ((List<object>) entryValueDict["adjacent"]);
			for (int i = 0; i < id1; ++i) {
				int id2 = i;
				Room room2 = rooms[id2];
				worldInstance.BuildTunnel(room1, room2);
			}
			/*foreach (object obj in adjacentRooms) {
				int id2 = System.Convert.ToInt32(obj);
				if (id1 > id2) {
					continue;
				}
				Room room2 = rooms[id2];
				roomBorders.BuildTunnel(room1, room2);
			}
		}
	}*/
}
