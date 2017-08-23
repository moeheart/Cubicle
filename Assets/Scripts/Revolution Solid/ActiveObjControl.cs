using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveObjControl : MonoBehaviour {

	protected delegate void ObjectBehaviour(int objectIndex);
	protected ObjectBehaviour objectBehaviour;

	public static List<RevSolid> revSolids;
	public static List<ActiveObject> activeObjects;
	protected static Vector3 midPos=new Vector3(0,0,0);

	// Use this for initialization
	void Awake(){
		
	}

	void Start () {

		InitPolygon ();
		ActivateObjects ();
		StartCoroutine("RecoverObjects");

		CheckLevelAndAddScript();

	}

	void CheckLevelAndAddScript(){
		RevSolidGameInfo.levelOfDifficulty = (SceneManager.GetActiveScene().name == "Easy") ? 0 : 1;
		if (RevSolidGameInfo.levelOfDifficulty == 0) {
			Camera.main.gameObject.AddComponent <Easy>();
		} else if (RevSolidGameInfo.levelOfDifficulty == 1) {
			Camera.main.gameObject.AddComponent <Hard>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (RevSolidGameInfo.levelOfDifficulty == 0) {
			for (int i = 0; i < RevSolidGameInfo.MaxPanelNum; i++) {
				activeObjects[i].RecordReactionTime ();
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
		

		
}
