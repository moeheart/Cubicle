using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
public class LevelManager : MonoBehaviour {

	public RoomBorders roomPrefab;

	public GameObject playerPrefab;

	private RoomBorders roomBorders;
	private List<Room> rooms = new List<Room>();
	private const string levelJsonFilePath = "Assets/Scripts/Json/Level.json";
	private const float lengthPerUnit = Configurations.lengthPerUnit;

	// Use this for initialization
	void Start () {
		roomBorders = Instantiate(roomPrefab) as RoomBorders;
		ReadAndParse();
		GameObject player = Instantiate(playerPrefab) as GameObject;
		player.transform.position = new Vector3(15,10,15);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ReadAndParse() {
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

			roomBorders.BuildRoom(position, dimension);
		}
		
		//The second run replaces blocks with doors between adjacent rooms
		foreach (KeyValuePair<string, object> entry in dict) {
			int id1 = int.Parse(entry.Key);
			Room room1 = rooms[id1];
			Dictionary<string, object> entryValueDict = (Dictionary<string,object>)entry.Value;
			List<object> adjacentRooms = ((List<object>) entryValueDict["adjacent"]);
			foreach (object obj in adjacentRooms) {
				int id2 = System.Convert.ToInt32(obj);
				if (id1 > id2) {
					continue;
				}
				Room room2 = rooms[id2];
				roomBorders.BuildTunnel(room1, room2);
			}
		}
	}
}
