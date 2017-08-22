using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;


public class Controller : MonoBehaviour {

	public Camera controllerCamera;

	public GameObject controllerObject;
	public GameObject block;
	public GameObject container;

	public GameObject startModel;
	public GameObject targetModel;
	public GameObject notationController;

	public int difficulty; // smaller, harder. 0 is smallest.

	public Dictionary<Vector3, bool> curModel;
	public Dictionary<Vector3, bool> tarModel;

	public Text text;

	public int restStep;

	private int id;
	private string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";

	public GameObject undoObject;
	public Dictionary<Vector3, bool> lastModel;

	public GameObject retryButton;

	public GameObject logObject;

	void Start () {

		id = DataUtil.GetCurrentRoomId();
		ParseJson(jsonFilePath, id);

		curModel = startModel.GetComponent<ModelGeneration> ().model;
		tarModel = targetModel.GetComponent<TransformGeneration> ().curModel;
		lastModel = null;

		restStep = targetModel.GetComponent<TransformGeneration> ().transNum + difficulty;
		text.text = " Rest Steps: " + restStep;


	}


	void ParseJson(string jsonFilePath, int roomId) {
		
		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		difficulty = System.Convert.ToInt32 (dict ["difficulty"]);

	}


	void Update () {



		Ray ray;
		RaycastHit rayhit;
		float fDistance = 20f;


		if (Input.GetMouseButtonDown (0)) {
			ray = controllerCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out rayhit, fDistance)) {
				string operation = rayhit.collider.gameObject.name;
				if (rayhit.collider.gameObject.transform.parent.transform.parent.name == "Controller") {
					Dictionary<Vector3, bool> nextModel = new Dictionary<Vector3, bool>();
					if (operation.Equals ("X Axis")) {
						nextModel = RotX (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("RotX");
						AfterTrans (nextModel);
					}
					else if (operation.Equals ("Y Axis")) {
						nextModel = RotY (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("RotY");
						AfterTrans (nextModel);
					}
					else if (operation.Equals ("Z Axis")) {
						nextModel = RotZ (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("RotZ");
						AfterTrans (nextModel);
					}
					else if (operation.Equals ("XY Plane")) {
						nextModel = SymXY (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("SymXY");
						AfterTrans (nextModel);
					}
					else if (operation.Equals ("XZ Plane")) {
						nextModel = SymXZ (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("SymXZ");
						AfterTrans (nextModel);
					}
					else if (operation.Equals ("YZ Plane")) {
						nextModel = SymYZ (curModel);
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("SymYZ");
						AfterTrans (nextModel);
					}
					else {  // undo
						logObject.GetComponent<TransformLimitationLog> ().LogDetail ("Undo");
						curModel = lastModel;
						restStep += 1;
						text.text = " Rest Steps: " + restStep;
						lastModel = null;
					}
				}
			}
		}

		RenderTransModel (curModel);

		if (lastModel != null)
			undoObject.SetActive (true);
		else
			undoObject.SetActive (false);



	}

	void AfterTrans(Dictionary<Vector3, bool> nextModel){
		lastModel = curModel;
		curModel = nextModel;
		curModel = AlignModel (curModel);
		restStep -= 1;
		text.text = " Rest Steps: " + restStep;
		CheckResult ();
	}


	void CheckDifficulty(){
		
		if (difficulty < 0)
			difficulty = 0;
		
	}


	Dictionary<Vector3, bool> RotX(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++) {

			resultModel.Add (new Vector3 (x, 1, 0), curModel [new Vector3 (x, 0, 1)]);
			resultModel.Add (new Vector3 (x, 0, -1), curModel [new Vector3 (x, 1, 0)]);
			resultModel.Add (new Vector3 (x, -1, 0), curModel [new Vector3 (x, 0, -1)]);
			resultModel.Add (new Vector3 (x, 0, 1), curModel [new Vector3 (x, -1, 0)]);
			resultModel.Add (new Vector3 (x, 1, -1), curModel [new Vector3 (x, 1, 1)]);
			resultModel.Add (new Vector3 (x, -1, -1), curModel [new Vector3 (x, 1, -1)]);
			resultModel.Add (new Vector3 (x, -1, 1), curModel [new Vector3 (x, -1, -1)]);
			resultModel.Add (new Vector3 (x, 1, 1), curModel [new Vector3 (x, -1, 1)]);
			resultModel.Add (new Vector3 (x, 0, 0), curModel [new Vector3 (x, 0, 0)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> RotY(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int y = -1; y <= 1; y++) {

			resultModel.Add (new Vector3 (-1, y, 0), curModel [new Vector3 (0, y, 1)]);
			resultModel.Add (new Vector3 (0, y, -1), curModel [new Vector3 (-1, y, 0)]);
			resultModel.Add (new Vector3 (1, y, 0), curModel [new Vector3 (0, y, -1)]);
			resultModel.Add (new Vector3 (0, y, 1), curModel [new Vector3 (1, y, 0)]);
			resultModel.Add (new Vector3 (-1, y, 1), curModel [new Vector3 (1, y, 1)]);
			resultModel.Add (new Vector3 (-1, y, -1), curModel [new Vector3 (-1, y, 1)]);
			resultModel.Add (new Vector3 (1, y, -1), curModel [new Vector3 (-1, y, -1)]);
			resultModel.Add (new Vector3 (1, y, 1), curModel [new Vector3 (1, y, -1)]);
			resultModel.Add (new Vector3 (0, y, 0), curModel [new Vector3 (0, y, 0)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> RotZ(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int z = -1; z <= 1; z++) {

			resultModel.Add (new Vector3 (1, 0, z), curModel [new Vector3 (0, 1, z)]);
			resultModel.Add (new Vector3 (0, -1, z), curModel [new Vector3 (1, 0, z)]);
			resultModel.Add (new Vector3 (-1, 0, z), curModel [new Vector3 (0, -1, z)]);
			resultModel.Add (new Vector3 (0, 1, z), curModel [new Vector3 (-1, 0, z)]);
			resultModel.Add (new Vector3 (1, -1, z), curModel [new Vector3 (1, 1, z)]);
			resultModel.Add (new Vector3 (-1, -1, z), curModel [new Vector3 (1, -1, z)]);
			resultModel.Add (new Vector3 (-1, 1, z), curModel [new Vector3 (-1, -1, z)]);
			resultModel.Add (new Vector3 (1, 1, z), curModel [new Vector3 (-1, 1, z)]);
			resultModel.Add (new Vector3 (0, 0, z), curModel [new Vector3 (0, 0, z)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> SymXY(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (x, y, -z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}

	Dictionary<Vector3, bool> SymXZ(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (x, -y, z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}

	Dictionary<Vector3, bool> SymYZ(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (-x, y, z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}


	void RenderTransModel(Dictionary<Vector3, bool> model){

//		startModel.GetComponentInChildren
		for (int i = 0; i < startModel.transform.childCount; i++) {
			GameObject block = startModel.transform.GetChild (i).gameObject;
			Destroy (block);
		}

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					if (model [new Vector3 (x, y, z)]) {
						GameObject blockInstance = Instantiate (block);
						blockInstance.transform.parent = container.transform;
						blockInstance.transform.position = new Vector3 (x, y, z);
						blockInstance.layer = 8;
					}
				}
			}
		}

	}


	Dictionary<Vector3, bool> AlignModel(Dictionary<Vector3, bool> model) { // align model to prevent wrong judge

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

		return alignModel;

	}


	Dictionary<Vector3,bool> InitializeDic() {

		Dictionary<Vector3,bool> model = new Dictionary<Vector3,bool> ();

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				for (int k = 0; k < 3; k++)
					model.Add (new Vector3 (i - 1, j - 1, k - 1), false);

		return model;
	}


	void CheckResult() {

		foreach (Vector3 point in tarModel.Keys)
		{
			if (curModel [point] != tarModel [point]) {
				if (restStep <= 0)
					Over ();
				return;
			}
		}

		Win();
	}

	void Win(){
		text.text = "You Win!";
		controllerObject.SetActive(false);
		notationController.SetActive(false);
		logObject.GetComponent<TransformLimitationLog> ().RecordResult (true);
		DataUtil.UnlockCurrentRoom();
	}

	void Over(){
		text.text = "Game Over!";
		controllerObject.SetActive(false);
		notationController.SetActive(false);
		retryButton.SetActive (true);
		logObject.GetComponent<TransformLimitationLog> ().RecordResult (false);

	}
		

}
