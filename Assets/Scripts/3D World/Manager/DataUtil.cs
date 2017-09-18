using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataUtil {

	private static Dictionary<string, object> GetCurrentGameState() {
		string saveFilePath = Path.Combine(Application.persistentDataPath, Configurations.saveFilename);
		//Get the id
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open(saveFilePath, FileMode.Open);
		Dictionary<string, object> gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
		stream.Close();
		return gameState;
	}
	public static int GetCurrentRoomId () {
		Dictionary<string, object> gameState = GetCurrentGameState();
		int id = (int)gameState["current room id"];
		return id;
	}

	public static void UnlockCurrentRoom() {
		string saveFilePath = Path.Combine(Application.persistentDataPath, Configurations.saveFilename);
		Dictionary<string, object> gameState = GetCurrentGameState();
		((List<int>) gameState["unlocked rooms"]).Add((int)gameState["current room id"]);
		FileStream stream = File.Create(saveFilePath);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, gameState);
		stream.Close();
	}

	public static void UnlockRoom() {

	}
}
