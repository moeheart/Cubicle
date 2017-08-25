using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MiniJSON;


public class ActiveObjControl : MonoBehaviour {

	protected delegate void ObjectBehaviour(int objectIndex);
	protected ObjectBehaviour objectBehaviour;

	public static List<RevSolid> revSolids;
	public static List<ActiveObject> activeObjects;
	protected static Vector3 midPos=new Vector3(0,0,0);

	public static string reactionTimeToLog;

	// Use this for initialization
	void Awake(){
		CheckLevel();
		AddScript (RevSolidGameInfo.GetLODByInt());
	}

	void OnEnable(){
		
		//EventManager.StartListening ("RecordReactionTime",ReactionTimeToLog);
	}

	void OnDisable(){
		//EventManager.StopListening ("RecordReactionTime",ReactionTimeToLog);
	}

	void Start () {

		InitPolygon ();
		ActivateObjects ();
		StartCoroutine("RecoverObjects");

	}

	void CheckLevel(){
		RevSolidGameInfo.levelOfDifficulty = ParseJson();
	}

	int ParseJson(){
		int roomId=DataUtil.GetCurrentRoomId();
		string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";

		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		return System.Convert.ToInt32 (dict ["levelNum"]);

	}

	void AddScript(int level){
		
		if (level == 1) {
			Camera.main.gameObject.AddComponent <Easy>();
				
		} else if (level == 2) {
			Camera.main.gameObject.AddComponent <Hard>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (RevSolidGameInfo.GetLODByInt() == 1) {
			for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
				activeObjects[i].GetReactionTime ();
			}
		}
	}
		
	void InitPolygon(){
		revSolids = new List<RevSolid> ();//constructor
		for (int i = 0; i < RevSolidGameInfo.MaxPolygonNum; i++) {
			revSolids.Add (new RevSolid (i));
		}
	}

	void ActivateObjects(){
		activeObjects = new List<ActiveObject> ();//constructor
		for (int i=0;i <RevSolidGameInfo.MaxPanelNum;i++) {
			activeObjects.Add (new ActiveObject (i,i));
			//yield return new WaitForSeconds (2);
		}
	}

	IEnumerator RecoverObjects(){
		while (true) {
			//recover & shift sectionPanel-polygon correspondence
			for(int i=0;i<RevSolidGameInfo.MaxPanelNum;i++){
				if (activeObjects [i].isKilled == true) {
					//replace with one of the polygons(that is currently not on screen
					int k;
					while(true){
						k = Mathf.FloorToInt (Random.value * RevSolidGameInfo.MaxPolygonNum);
						int j;
						for (j = 0; j < RevSolidGameInfo.MaxPanelNum; j++) {
							if (activeObjects [j].polygonIndex == k)
								break;//for
						}
						if (j == RevSolidGameInfo.MaxPanelNum) {
							break;//while
						}
					}
					activeObjects [i].polygonIndex = k;
					activeObjects [i].Refresh ();
					/*
					Debug.Log (i);
					Debug.Log ("+1");*/
				}
				yield return new WaitForSeconds (RevSolidGameInfo.RecoverInterval);
			}
			yield return new WaitForSeconds (RevSolidGameInfo.RecoverInterval);

		}
	}

	public static bool WithinViewport(int objIndex){
		return (Mathf.Abs (activeObjects [objIndex].gameObject.transform.position.x - midPos.x) <= 8
			&& Mathf.Abs (activeObjects [objIndex].gameObject.transform.position.y - midPos.y) <= 5);
	}


	protected void Rotate(int objIndex){
		if (activeObjects [objIndex].isKilled == false) {
			activeObjects [objIndex].gameObject.transform.Rotate (0.5f, 0.5f, 0.5f);
		}
	}

	protected void FadeInOrOut(int objIndex){
		if (!activeObjects [objIndex].isKilled) {
			activeObjects [objIndex].gameObject.GetComponent<MeshRenderer> ().material.SetFloat ("_AlphaScale",Mathf.Clamp(activeObjects [objIndex].alphaScale+=0.2f,0.0f,0.6f));
		}

		if (activeObjects [objIndex].isKilled) {
			activeObjects [objIndex].gameObject.GetComponent<MeshRenderer> ().material.SetFloat ("_AlphaScale",Mathf.Clamp(activeObjects [objIndex].alphaScale-=0.2f,0.0f,0.6f));
			activeObjects [objIndex].gameObject.transform.position = new Vector3 (-100,-100,0);
		}
			
	}
		
	public static void RecordReactionTimeWhenObjectKilled(float reactionTime){
		reactionTimeToLog = reactionTime.ToString();
		EventManager.TriggerEvent("RecordReactionTime");
	}

	static void ReactionTimeToLog(){
		
	}
		
}
