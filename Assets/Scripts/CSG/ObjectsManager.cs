using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class ObjectsManager : MonoBehaviour {

	public Material opAMaterial, opBMaterial;
	public Material objMaterial;

	private List<GameObject> gameObjects = new List<GameObject>();
	private GameObject opA, opB;
	private GameObject selectedGameObject;

	public void LoadGameObjects() {

		GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj1.SetActive(false);
		obj2.SetActive(false);
		obj1.transform.parent = this.transform;
		obj2.transform.parent = this.transform;
		obj1.AddComponent<ObjectBehaviors>();
		obj2.AddComponent<ObjectBehaviors>();
		obj1.GetComponent<MeshRenderer>().material = objMaterial;
		obj2.GetComponent<MeshRenderer>().material = objMaterial;
		obj2.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		gameObjects.Add(obj1);
		gameObjects.Add(obj2);
		opA = obj1;
		opB = obj2;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//TODO
		//These controls are pretty bad right now
		//Change it to something more natural
		//Also, move the object control to a different function

		//The Y-axis
		if (Input.GetKeyDown(KeyCode.I)) {
			selectedGameObject.transform.localPosition += new Vector3(0,1,0);
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			selectedGameObject.transform.localPosition += new Vector3(0,-1,0);
		}

		//The X-axis
		if (Input.GetKeyDown(KeyCode.J)) {
			selectedGameObject.transform.localPosition += new Vector3(-1,0,0);
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			selectedGameObject.transform.localPosition += new Vector3(1,0,0);
		}

		//The Z-axis
		if (Input.GetKeyDown(KeyCode.U)) {
			selectedGameObject.transform.localPosition += new Vector3(0,0,1);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			selectedGameObject.transform.localPosition += new Vector3(0,0,-1);
		}

		//Rotate around Y-axis
		if (Input.GetKeyDown(KeyCode.D)) {
			selectedGameObject.transform.Rotate(0,90,0);
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			selectedGameObject.transform.Rotate(0,-90,0);
		}

		//Rotate around X-axis
		if (Input.GetKeyDown(KeyCode.W)) {
			selectedGameObject.transform.Rotate(90,0,0);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			selectedGameObject.transform.Rotate(-90,0,0);
		}

		//Rotate around Z-axis
		if (Input.GetKeyDown(KeyCode.Q)) {
			selectedGameObject.transform.Rotate(0,0,90);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			selectedGameObject.transform.Rotate(0,0,-90);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			GameObject composite;
			Mesh m = CSG.Subtract(opA, opB);
			composite = new GameObject();
			composite.AddComponent<MeshFilter>().sharedMesh = m;
			composite.AddComponent<MeshRenderer>().sharedMaterial = opA.GetComponent<MeshRenderer>().sharedMaterial;
			Destroy(opA);
			Destroy(opB);
		}	
	}

	public void OnGameObjectClick(GameObject gameObject) {
		DeselectGameObject(selectedGameObject);
		SelectGameObject(gameObject);
		Debug.Log(selectedGameObject.name);
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
		UpdateMaterial();
	}

	private void UpdateMaterial() {
		if (opA != null) {
			opA.GetComponent<MeshRenderer>().material = opAMaterial;
		}
		if (opB != null) {
			opB.GetComponent<MeshRenderer>().material = opBMaterial;
		}
	}

	private void DeselectGameObject(GameObject gameObject) {
		if (selectedGameObject == null) {
			return;
		}
		float alpha = objMaterial.color.a;
		Color color = gameObject.GetComponent<MeshRenderer>().material.color;
		color.a = alpha;
		gameObject.GetComponent<MeshRenderer>().material.color = color;
	}

	private void SelectGameObject(GameObject gameObject) {
		selectedGameObject = gameObject;
		Color color = gameObject.GetComponent<MeshRenderer>().material.color;
		color.a = 1f;
		gameObject.GetComponent<MeshRenderer>().material.color = color;
	}

}
