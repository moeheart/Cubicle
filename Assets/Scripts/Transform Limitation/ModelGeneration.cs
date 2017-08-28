using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;

public class ModelGeneration : MonoBehaviour {

	public Dictionary<Vector3, bool> model;
	public int blockNum;
	public GameObject block;
	public GameObject container;
	public GameObject tarModelObject;

	private List<Vector3> selectedPoints;

	private int id;
	public int levelNum;
	public int level;
	private string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";
	public string method;

	void Awake () {

		level = 0;

		Initialize ();

	}

	public void Initialize(){

		id = DataUtil.GetCurrentRoomId();
		ParseJson(jsonFilePath, id, level);

		selectedPoints = new List<Vector3> ();

		CheckBlockNum ();
		InitializeModel ();
		GenerateModel ();
		RenderModel ();
		
	}

	private void ParseJson(string jsonFilePath, int roomId, int level) {
		
		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;

		// Debug.Log(roomId);
		// foreach (KeyValuePair<string, object> pair in dict)  
		// {  
		// 	Debug.Log(pair.Key + " " + pair.Value);  
		// }
		
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		levelNum = System.Convert.ToInt32 (dict ["levelNum"]);

		dict = (Dictionary<string, object>)dict["levels"];
		dict = (Dictionary<string, object>)dict[level.ToString()];

		blockNum = System.Convert.ToInt32 (dict ["blockNum"]);
		method = System.Convert.ToString (dict ["method"]);

		tarModelObject.GetComponent<TransformGeneration>().transNum = 
			System.Convert.ToInt32 (dict ["baicSteps"]);
		tarModelObject.GetComponent<TransformGeneration>().difficulty = 
			System.Convert.ToInt32 (dict ["difficulty"]);

	}

	void CheckBlockNum() {
		
		if (blockNum < 1)
			blockNum = 1;
		
		if (blockNum > 27)
			blockNum = 27;
		
	}

	void InitializeModel() {
		
		model = new Dictionary<Vector3,bool> ();

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				for (int k = 0; k < 3; k++)
					model.Add (new Vector3 (i - 1, j - 1, k - 1), false);
		
	}

	void GenerateModel() {

		float x, y, z;

		// modelling start point

		int startPointIndex = Random.Range (0, 27);

		x = startPointIndex % 3 - 1;
		startPointIndex = (startPointIndex - startPointIndex % 3) / 3;
		y = startPointIndex % 3 - 1;
		startPointIndex = (startPointIndex - startPointIndex % 3) / 3;
		z = startPointIndex % 3 - 1;

		model [new Vector3 (x, y, z)] = true;
		selectedPoints.Add (new Vector3 (x, y, z));


		// generate others

		/* random index mapping rules
		 * 0: (1, 0, 0)
		 * 1: (-1, 0, 0)
		 * 2: (0, 1, 0)
		 * 3: (0, -1, 0)
		 * 4: (0, 0, 1)
		 * 5: (0, 0, -1)
		 */

		for (int i = 0; i < blockNum - 1; i++) { 

			int nextPointIndex = Random.Range (0, 6);
			Vector3 fromPoint = selectedPoints [Random.Range (0, selectedPoints.Count)]; 
			// select a start point randomly to prevent endless loop

			x = fromPoint [0];
			y = fromPoint [1];
			z = fromPoint [2];

			switch (nextPointIndex) {
			case 0:
				x += 1;
				break;
			case 1:
				x -= 1;
				break;
			case 2:
				y += 1;
				break;
			case 3:
				y -= 1;
				break;
			case 4:
				z += 1;
				break;
			case 5:
				z -= 1;
				break;
			}

			if (x < -1 || x > 1 || y < -1 || y > 1 || z < -1 || z > 1) { // check if is outside
				i--;
			} else if (model [new Vector3 (x, y, z)] == true) {
				i--;
			} else {
				model [new Vector3 (x, y, z)] = true;
				selectedPoints.Add (new Vector3 (x, y, z));
			}
		}

	}

	void RenderModel(){

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					if (model [new Vector3 (x, y, z)]) {
						GameObject blockInstance = Instantiate (block);
						blockInstance.transform.parent = container.transform;
						blockInstance.layer = 8;
						blockInstance.transform.position = new Vector3 (x, y, z);
					}
				}
			}
		}

	}


	void AlignModel() { // align model to prevent wrong judge

		Dictionary<Vector3, bool> alignModel = model;

		float xMin, yMin, zMin;

		xMin = 1.0f;
		yMin = 1.0f;
		zMin = 1.0f;

		foreach (Vector3 point in model.Keys)
		{
			if (model [point]) {
				if (point [0] < xMin)
					xMin = point [0];

				if (point [1] < yMin)
					yMin = point [1];

				if (point [2] < zMin)
					zMin = point [2];
			}
		}

		if (xMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0] - xMin - 1, point [1], point [2])] = true;
			}
			alignModel = tmpModel;

		}

		if (yMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0], point [1] - yMin - 1, point [2])] = true;
			}

			alignModel = tmpModel;

		}

		if (zMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0], point [1], point [2] - zMin - 1)] = true;
			}

			alignModel = tmpModel;

		}

		model = alignModel;

	}


	Dictionary<Vector3,bool> InitializeDic() {

		Dictionary<Vector3,bool> model = new Dictionary<Vector3,bool> ();

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				for (int k = 0; k < 3; k++)
					model.Add (new Vector3 (i - 1, j - 1, k - 1), false);

		return model;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Q)) {
			SceneManager.LoadScene ("World Scene");
		}
	}
}
