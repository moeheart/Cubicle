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

	public static GameObject gameObjectJustHit;

	// Use this for initialization
	void Awake(){
		CheckLevel();
		AddScript (RevSolidGameInfo.GetLODByInt());
	}

	void OnEnable(){

	}

	void OnDisable(){

	}

	void Start () {

		InitPolygon ();
		ActivateObjects ();
		StartCoroutine ("GenerateInitialObjects");
		gameObjectJustHit=activeObjects[0].gameObject;

	}

	void CheckLevel(){
		RevSolidGameInfo.levelOfDifficulty = ParseJson("levelNum");
	}

	int ParseJson(string lineTitle){
		int roomId=DataUtil.GetCurrentRoomId();
		string jsonFilePath = Path.Combine(Application.streamingAssetsPath, Configurations.jsonFilename);

		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		return System.Convert.ToInt32 (dict [lineTitle]);

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

	IEnumerator GenerateInitialObjects(){
		for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
			RecoverObjects ();
			yield return new WaitForSeconds (RevSolidGameInfo.RecoverInterval);
		}
	}

	public void RecoverObjects(){
		//recover & shift sectionPanel-polygon correspondence
		for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
			if (activeObjects [i].isKilled == true) {
				
				//replace with one of the polygons(that is currently not on screen
				int k;
				while (true) {
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
				RevSolidGameInfo.polygonGenerationCount++;
				if (!RevSolidGameInfo.IfNoviceGuideEnds ()) {
					Tutorial.IndicateCorrectAns (i);
				} 
				if (RevSolidGameInfo.WhenNoviceGuideEnds ()) {
					StartCoroutine (this.GetComponent<RevSolidUIControl> ().ShowStartGamePanel ());
				}
				activeObjects [i].Refresh ();
				break;
			}
		}
	}
		
	protected void RaycastHit(int objIndex){
		if (activeObjects [objIndex].isKilled == false) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				gameObjectJustHit = hit.collider.gameObject;
			}
		}
	}

	protected void Rotate(int objIndex){
		if (activeObjects [objIndex].isKilled == false) {


			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {

				gameObjectJustHit.transform.Rotate (new Vector3 (1.0f, 0, 0));

			} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {

				gameObjectJustHit.transform.Rotate (new Vector3 (-1.0f, 0, 0));

			} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				gameObjectJustHit.transform.Rotate (new Vector3 (0, 1.0f, 0));

			} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				gameObjectJustHit.transform.Rotate (new Vector3 (0, -1.0f, 0));

			}else {
				SnapBack (gameObjectJustHit.transform);
			}
		}
	}

	void SnapBack(Transform transform){
		transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(-45.0f,0,0),0.01f);

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
		
}
