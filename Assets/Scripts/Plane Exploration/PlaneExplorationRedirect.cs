using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class PlaneExplorationRedirect : MonoBehaviour {

	private string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";
	private int level;
	private int id;

	void Start () {

		id = DataUtil.GetCurrentRoomId();
		ParseJson(jsonFilePath, id);

		switch (level) {
		case 1:
			SceneManager.LoadScene ("Q1", LoadSceneMode.Single);
			break;
		case 2:
			SceneManager.LoadScene("Q2", LoadSceneMode.Single);
			break;
		case 3:
			SceneManager.LoadScene("Q3", LoadSceneMode.Single);
			break;
		case 4:
			SceneManager.LoadScene("Q4", LoadSceneMode.Single);
			break;
		case 5:
			SceneManager.LoadScene("Q5", LoadSceneMode.Single);
			break;
		case 6:
			SceneManager.LoadScene("Q6", LoadSceneMode.Single);
			break;
		case 7:
			SceneManager.LoadScene("Q7", LoadSceneMode.Single);
			break;
		}
	}

	private void ParseJson(string jsonFilePath, int roomId) {

		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
//		print (roomId);
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		level = System.Convert.ToInt32 (dict ["level"]);
	}
}
