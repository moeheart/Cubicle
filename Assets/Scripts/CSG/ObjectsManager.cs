using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sabresaurus.SabreCSG;
using ConstructiveSolidGeometry;

public class ObjectsManager : MonoBehaviour {

	public GameObject newObjectPrefab;

	private List<GameObject> gameObjects;
	private static GameObject opA, opB;

	CSGModelBase csgModel;

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
		if (Input.GetKeyDown(KeyCode.Space)) {
			//GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			CSG result= CSGOperations.Subtract(opA, opB);
			GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        	if (result != null)
				newGo.GetComponent<MeshFilter>().mesh = result.toMesh();
			Destroy(opA);
			Destroy(opB);
		}
	}

	public void LoadGameObjects() {

		csgModel = gameObject.AddComponent<CSGModelBase>();

		// Random position for brush
		Vector3 localPosition = Random.insideUnitCircle * 100;
		// All brushes same size
		Vector3 localSize = new Vector3(20,16,20);
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//GameObject newObject = csgModel.CreateBrush(obj, localPosition, localSize, Quaternion.identity);
	}
}
