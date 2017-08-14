using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sabresaurus.SabreCSG;

public class ObjectsManager : MonoBehaviour {

	private List<GameObject> gameObjects;
	private static GameObject opA, opB;

	CSGModelBase csgModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
