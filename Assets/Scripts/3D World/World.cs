using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;

public class World : MonoBehaviour {

	//Responsible for instantiating the walls, floors and doors

	public GameObject playerPrefab;
	public GameObject floor;
	public GameObject wallXY;
	public GameObject wallZY;
	public Door trapdoor;
	public Door doorXY;
	public Door doorZY;
	public Room roomPrefab;

	private List<Room> rooms = new List<Room>();
	private GameObject player;
	public int currentRoomId {private get; set;}

	private const float lengthPerUnit = Configurations.lengthPerUnit;
	private const float borderThickness = Configurations.borderThickness;

	private string saveFilePath;

	private Dictionary<string, object> gameState;

	public void GenerateWorld(string jsonFilePath) {
		saveFilePath = Path.Combine(Application.persistentDataPath, "game.dat");
		string jsonString = File.ReadAllText(jsonFilePath);
		ParseJsonString(jsonString);
		//rooms[0].OnCompleteRoomObjective();
		foreach (Room room in rooms) {
			room.AddTrigger();
			if (room.puzzleType == PuzzleType.None) {
				room.OnCompleteRoomObjective();
			}
		}
		player = Instantiate(playerPrefab) as GameObject;
		player.transform.position = new Vector3(15,10,15);
	}

	private void ParseJsonString(string data) {
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

			string puzzleTypeString = (string)entryValueDict["puzzle type"];
			
			Vector3 position = new Vector3(posX, posY, posZ);
			Vector3 dimension = new Vector3(dimX, dimY, dimZ);
			Color color = new Color(colorR, colorG, colorB, colorA);
			PuzzleType puzzleType = PuzzleType.None;

			puzzleType = PuzzleTypes.GetTypeFromString(puzzleTypeString);

			Room room = Instantiate(roomPrefab) as Room;
			room.Initialize(roomId, position, dimension, color, puzzleType);
			rooms.Insert(roomId, room);

			BuildRoom(position, dimension, color);
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
				BuildTunnel(room1, room2);
			}
		}
	}

	public void LoadData() {

		if (!File.Exists(saveFilePath)) {
			gameState = new Dictionary<string, object>();
			Debug.Log("No saved game.");
			return;
		}

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open(saveFilePath, FileMode.Open);
		gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
		stream.Close();

		player.transform.position = new Vector3(
			(float)gameState["player position x"],
			(float)gameState["player position y"],
			(float)gameState["player position z"]
		);
		player.transform.rotation = new Quaternion(
			(float)gameState["player rotation x"],
			(float)gameState["player rotation y"],
			(float)gameState["player rotation z"],
			(float)gameState["player rotation w"]
		);

		List<int> unlockedRooms = (List<int>) gameState["unlocked rooms"];
		foreach (int id in unlockedRooms) {
			rooms[id].OnCompleteRoomObjective();
		}

		Debug.Log("Successfully Loaded save game...!!");

	}

	public void SaveData() {

		//TODO
		//Actually, should not "save from scratch", as the save file contains other info

		gameState["player position x"] = player.transform.position.x;
		gameState["player position y"] = player.transform.position.y;
		gameState["player position z"] = player.transform.position.z;

		gameState["player rotation x"] = player.transform.rotation.x;
		gameState["player rotation y"] = player.transform.rotation.y;
		gameState["player rotation z"] = player.transform.rotation.z;
		gameState["player rotation w"] = player.transform.rotation.w;

		gameState["current room id"] = currentRoomId;

		if (gameState.ContainsKey("unlocked rooms") == false) {
			gameState["unlocked rooms"] = new List<int>();
		}

		/*gameState.Add("player position x", player.transform.position.x);
		gameState.Add("player position y", player.transform.position.y);
		gameState.Add("player position z", player.transform.position.z);

		gameState.Add("player rotation w", player.transform.rotation.w);
		gameState.Add("player rotation x", player.transform.rotation.x);
		gameState.Add("player rotation y", player.transform.rotation.y);
		gameState.Add("player rotation z", player.transform.rotation.z);

		gameState.Add("current room id", currentRoomId);*/

		FileStream stream = File.Create(saveFilePath);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, gameState);
		stream.Close();

	}

	private void BuildRoom(Vector3 position, Vector3 dimension, Color color) {
		//Debug.Log("building room... position: " + position + "size: " + dimension);
		//Responsible for building the walls of the room
		//Builds the six sides without considering the doors
		//The doors are handled in BuildTunnel

		Vector3 bottomFacePosition = new Vector3(position.x, position.y + borderThickness/2, position.z);
		BuildSide(bottomFacePosition, Direction.XZ, new Vector2(dimension[0], dimension[2]), color);

		Vector3 frontFacePosition = new Vector3(position.x, position.y, position.z + borderThickness/2);
		BuildSide(frontFacePosition, Direction.XY, new Vector2(dimension[0], dimension[1]), color);

		Vector3 leftFacePosition = new Vector3(position.x + borderThickness/2, position.y, position.z);
		BuildSide(leftFacePosition, Direction.ZY, new Vector2(dimension[2], dimension[1]), color);

		Vector3 topFacePosition = new Vector3(position.x, 
			position.y + dimension[1] * lengthPerUnit - borderThickness/2, position.z);
		BuildSide(topFacePosition, Direction.XZ, new Vector2(dimension[0], dimension[2]), color);

		Vector3 backFacePosition = new Vector3(position.x, position.y, 
			position.z + dimension[2] * lengthPerUnit - borderThickness/2);
		BuildSide(backFacePosition, Direction.XY, new Vector2(dimension[0], dimension[1]), color);

		Vector3 rightFacePosition = new Vector3(position.x + dimension[0] * lengthPerUnit - borderThickness/2, 
			position.y, position.z);
		BuildSide(rightFacePosition, Direction.ZY, new Vector2(dimension[2], dimension[1]), color);
	}

	private void BuildSide(Vector3 position, Direction direction, Vector2 size, Color color) {
		//Instantiates a cube
		IntVector2 dimension = new IntVector2((int)size.x, (int)size.y);
		for (int i = 0; i < dimension.x; ++i) {
			for (int j = 0; j < dimension.z; ++j) {
				GameObject border;
				string name;
				Vector3 pos;
				switch (direction) {
					case Direction.XZ:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y, position.z + j * lengthPerUnit);
						name = pos + "-XZ";

						if (GameObject.Find(name) == null) {
							border = Instantiate(floor) as GameObject;
							border.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;

					case Direction.XY:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y + j * lengthPerUnit, position.z);
						name = pos + "-XY";

						if (GameObject.Find(name) == null) {
							border = Instantiate(wallXY) as GameObject;
							border.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;

					case Direction.ZY:
						pos = new Vector3(position.x, 
							position.y + j * lengthPerUnit, position.z + i * lengthPerUnit);
						name = pos + "-ZY";

						if (GameObject.Find(name) == null) {
							border = Instantiate(wallZY) as GameObject;
							border.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;
				}
			}
		}
	}

	private void BuildTunnel(Room room1, Room room2) {
		//Builds a tunnel between adjacent rooms
		Vector3 position;
		Vector2 size;
		Direction direction;
		bool doOverlap = FindOverlapArea(room1, room2, out position, out size, out direction);
		if (doOverlap == false) {
			return;
		}

		IntVector2 overlapDimension = new IntVector2((int)(size.x/lengthPerUnit), (int)(size.y/lengthPerUnit));
		//Debug.Log("Room: " + room1.id + " " + room2.id + ". Pos=" + position + " Size=" + overlapDimension + " Direction:" + direction);

		//First delete the walls blocking them
		for (int i = 0; i < overlapDimension.x; ++i) {
			for (int j = 0; j < overlapDimension.z; ++j) {
				string name;
				Vector3 pos;
				switch (direction) {
					case Direction.XZ:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y + borderThickness/2, position.z + j * lengthPerUnit);
						name = pos + "-XZ";
						DestroyGameObjectByName(name);

						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y - borderThickness/2, position.z + j * lengthPerUnit);
						name = pos + "-XZ";
						DestroyGameObjectByName(name);
						break;

					case Direction.XY:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y + j * lengthPerUnit, position.z + borderThickness/2);
						name = pos + "-XY";
						DestroyGameObjectByName(name);

						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y + j * lengthPerUnit, position.z - borderThickness/2);
						name = pos + "-XY";
						DestroyGameObjectByName(name);
						break;

					case Direction.ZY:
						pos = new Vector3(position.x + borderThickness/2, 
							position.y + j * lengthPerUnit, position.z + i * lengthPerUnit);
						name = pos + "-ZY";
						DestroyGameObjectByName(name);

						pos = new Vector3(position.x - borderThickness/2, 
							position.y + j * lengthPerUnit, position.z + i * lengthPerUnit);
						name = pos + "-ZY";
						DestroyGameObjectByName(name);
						break;
				}
			}
		}

		Door entrance;
		switch (direction) {
			case Direction.XZ:
				entrance = Instantiate(trapdoor) as Door;
				entrance.transform.parent = this.transform;
				entrance.transform.position = position;
				entrance.transform.localScale = new Vector3(overlapDimension.x, 2, overlapDimension.z);
				room1.AddDoor(entrance);
				room2.AddDoor(entrance);
				break;

			case Direction.XY:
				entrance = Instantiate(doorXY) as Door;
				entrance.transform.parent = this.transform;
				entrance.transform.position = position;
				entrance.transform.localScale = new Vector3(overlapDimension.x, overlapDimension.z, 2);
				room1.AddDoor(entrance);
				room2.AddDoor(entrance);
				break;

			case Direction.ZY:
				entrance = Instantiate(doorZY) as Door;
				entrance.transform.parent = this.transform;
				entrance.transform.position = position;
				entrance.transform.localScale = new Vector3(overlapDimension.x, overlapDimension.z, 2);
				room1.AddDoor(entrance);
				room2.AddDoor(entrance);
				break;
		}
	}

	private void DestroyGameObjectByName(string name) {
		if (GameObject.Find(name) != null) {
			Destroy(GameObject.Find(name));
		}
		else {
			Debug.LogError("Cannot find block: " + name);
		}
	}

	private bool FindOverlapArea(Room room1, Room room2, out Vector3 position, out Vector2 size, out Direction direction) {
		//Find the overlap area of two rooms
		//Use this information to build the tunnels
		position = Vector3.zero;
		size = Vector2.zero;
		direction = Direction.XZ;

		Vector2 intersectionPoint;
		Vector2 intersectionSize;

		bool doOverlap = false;

		if (room1.position.y == room2.position.y + room2.size.y
				|| room1.position.y + room1.size.y == room2.position.y) {
			//room1's bottom and room2's top
			//Debug.Log("The XZ Plane overlaps");
			direction = Direction.XZ;
			Vector2 pos1 = new Vector2(room1.position.x, room1.position.z);
			Vector2 pos2 = new Vector2(room2.position.x, room2.position.z);
			Vector2 size1 = new Vector2(room1.size.x, room1.size.z);
			Vector2 size2 = new Vector2(room2.size.x, room2.size.z);
			
			doOverlap = FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);
			if (doOverlap == true) {
				if (room1.position.y == room2.position.y + room2.size.y) {
					position = new Vector3(intersectionPoint[0], room1.position.y, intersectionPoint[1]);
					size = intersectionSize;
					return true;
				}

				else {
					position = new Vector3(intersectionPoint[0], room2.position.y, intersectionPoint[1]);
					size = intersectionSize;
					return true;
				}
			}

		}

		if (room1.position.x == room2.position.x + room2.size.x
				|| room1.position.x + room1.size.x == room2.position.x) {
			//room1's left and room2's right
			//Debug.Log("The ZY Plane overlaps");
			direction = Direction.ZY;
			Vector2 pos1 = new Vector2(room1.position.z, room1.position.y);
			Vector2 pos2 = new Vector2(room2.position.z, room2.position.y);
			Vector2 size1 = new Vector2(room1.size.z, room1.size.y);
			Vector2 size2 = new Vector2(room2.size.z, room2.size.y);

			doOverlap = FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);
			if (doOverlap == true) {
				if (room1.position.x == room2.position.x + room2.size.x) {
					position = new Vector3(room1.position.x, intersectionPoint[1], intersectionPoint[0]);
					size = intersectionSize;
					return true;
				}

				else {
					position = new Vector3(room2.position.x, intersectionPoint[1], intersectionPoint[0]);
					size = intersectionSize;
					return true;
				}
			}
		}

		if (room1.position.z == room2.position.z + room2.size.z
				|| room1.position.z + room1.size.z == room2.position.z) {
			//room1's front and room2's back
			//Debug.Log("The XY Plane overlaps");
			direction = Direction.XY;
			Vector2 pos1 = new Vector2(room1.position.x, room1.position.y);
			Vector2 pos2 = new Vector2(room2.position.x, room2.position.y);
			Vector2 size1 = new Vector2(room1.size.x, room1.size.y);
			Vector2 size2 = new Vector2(room2.size.x, room2.size.y);

			doOverlap = FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);
			if (doOverlap == true) {
				if (room1.position.z == room2.position.z + room2.size.z) {
					position = new Vector3(intersectionPoint[0], intersectionPoint[1], room1.position.z);
					size = intersectionSize;
					return true;
				}

				else {
					position = new Vector3(intersectionPoint[0], intersectionPoint[1], room2.position.z);
					size = intersectionSize;
					return true;
				}
			}
		}

		return false;
	}

	private bool FindOverlapOfRectangle(Vector2 pos1, Vector2 pos2, Vector2 size1, Vector2 size2, 
			out Vector2 outPos, out Vector2 outSize) {
		//A Util function that finds the intersection area of two rectangles
		float left = Mathf.Max(pos1.x, pos2.x);
		float right = Mathf.Min(pos1.x + size1.x , pos2.x + size2.x);
		float bottom = Mathf.Max(pos1.y, pos2.y);
		float top = Mathf.Min(pos1.y + size1.y, pos2.y + size2.y);
		outPos = new Vector2(left, bottom);
		outSize = new Vector2(right - left, top - bottom);

		if (right > left && top > bottom)
			return true;
		else
			return false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
		//Add a key to delete the save file
		if (Input.GetKeyDown(KeyCode.Delete)) {
			File.Delete(saveFilePath);
		}
	}
}
