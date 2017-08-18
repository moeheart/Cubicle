using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class ObjectsManager : MonoBehaviour {

	public Material opAMaterial, opBMaterial;
	public Material defaultMaterial;
	public Material targetMaterial;
	public Mesh targetMesh;

	private GameObject target;

	private List<GameObject> gameObjects = new List<GameObject>();
	private GameObject opA, opB;
	private GameObject selectedGameObject;

	public void LoadGameObjects() {

		GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj1.transform.parent = this.transform;
		obj2.transform.parent = this.transform;
		obj1.transform.localPosition = new Vector3(-3,-3,0);
		obj2.transform.localPosition = new Vector3(-3,3,0);
		obj1.AddComponent<ObjectBehaviors>();
		obj2.AddComponent<ObjectBehaviors>();
		obj1.GetComponent<MeshRenderer>().material = defaultMaterial;
		obj2.GetComponent<MeshRenderer>().material = defaultMaterial;
		obj2.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		gameObjects.Add(obj1);
		gameObjects.Add(obj2);
		//opA = obj1;
		//opB = obj2;

		target = new GameObject("Target");
		Debug.Log("Target Volume" + CSGUtil.VolumeOfMesh(targetMesh));

		target.AddComponent<MeshFilter>().mesh = targetMesh;
		target.AddComponent<MeshRenderer>().material = targetMaterial;
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
		if (Input.GetKeyDown(KeyCode.W)) {
			selectedGameObject.transform.localPosition += new Vector3(0,1,0);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			selectedGameObject.transform.localPosition += new Vector3(0,-1,0);
		}

		//The X-axis
		if (Input.GetKeyDown(KeyCode.A)) {
			selectedGameObject.transform.localPosition += new Vector3(-1,0,0);
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			selectedGameObject.transform.localPosition += new Vector3(1,0,0);
		}

		//The Z-axis
		if (Input.GetKeyDown(KeyCode.Q)) {
			selectedGameObject.transform.localPosition += new Vector3(0,0,1);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			selectedGameObject.transform.localPosition += new Vector3(0,0,-1);
		}

		//Rotate around Y-axis
		if (Input.GetKeyDown(KeyCode.L)) {
			selectedGameObject.transform.Rotate(0,90,0);
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			selectedGameObject.transform.Rotate(0,-90,0);
		}

		//Rotate around X-axis
		if (Input.GetKeyDown(KeyCode.I)) {
			selectedGameObject.transform.Rotate(90,0,0);
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			selectedGameObject.transform.Rotate(-90,0,0);
		}

		//Rotate around Z-axis
		if (Input.GetKeyDown(KeyCode.U)) {
			selectedGameObject.transform.Rotate(0,0,90);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			selectedGameObject.transform.Rotate(0,0,-90);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			if (opA == null || opB == null) {
				return;
			}
			GameObject composite = CSGUtil.Subtract(opA, opB, defaultMaterial);
			composite.name = "(" + opA.name + ") - (" + opB.name + ")";
			gameObjects.Remove(opA);
			gameObjects.Remove(opB);
			gameObjects.Add(composite);
			Destroy(opA);
			Destroy(opB);
			Debug.Log("There are " + gameObjects.Count + " objects left in scene");

			Mesh m = composite.GetComponent<MeshFilter>().mesh;
			Debug.Log("your volume: " + CSGUtil.VolumeOfMesh(m));
			Mesh m1 = CSG.Union(target, composite);
			Mesh m2 = CSG.Intersect(target, composite);
			//GameObject g1 = CSGUtil.Subtract(target, composite, defaultMaterial);
			//GameObject g2 = CSGUtil.Subtract(composite, target, defaultMaterial);
			//g1.transform.localPosition = new Vector3(-2,0,0);
			//g2.transform.localPosition = new Vector3(2,0,0);
			Debug.Log("union volume: " + CSGUtil.VolumeOfMesh(m1));
			Debug.Log("intersection volume: " + CSGUtil.VolumeOfMesh(m2));
			if (CSGUtil.VolumeOfMesh(m1) - CSGUtil.VolumeOfMesh(m2) < 1e-2) {
				Debug.Log("You completed the level..!!");
			}
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
		float alpha = defaultMaterial.color.a;
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
