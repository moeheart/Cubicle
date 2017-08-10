using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using ConstructiveSolidGeometry;

public class CSGManager : MonoBehaviour {

	public GameObject newObjectPrefab;
	private List<GameObject> gameObjects = new List<GameObject>();

	private static GameObject opA, opB;

	public static void OnClickGameObject(GameObject gameObject) {
		if (opA == null) {
			opA = gameObject;
		}
		else if (opB == null) {
			opB = gameObject;
		}
		else {
			opA = opB;
			opB = gameObject;
		}
	}

	// Use this for initialization
	void Start () {
		opA = GameObject.CreatePrimitive(PrimitiveType.Cube);
		opB = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		opA.transform.position = Vector3.zero;
		opB.transform.position = new Vector3(0.5f,0,0);
		gameObjects.Add(opA);
		gameObjects.Add(opB);

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			//GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			CSG result= CSGOperations.Subtract(opA, opB);
			GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        	if (result != null)
				newGo.GetComponent<MeshFilter>().mesh = result.toMesh();
			Destroy(opA);
			Destroy(opB);
		}*/
	}
}
