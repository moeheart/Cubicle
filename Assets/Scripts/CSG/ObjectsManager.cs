using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour {
	public SceneObject CSGObjectPrefab;
	//public SceneObject CSGCube, CSGSphere, CSGCylinder;
	public GameObject targetPrefab;
	public Mesh targetMesh;
	//public Material wireframeMaterial;
	public Material targetMaterial;
	//public Material wireframeMaterial;

	private List<SceneObject> sceneObjs;
	private GameObject targetObj;
	private SceneObject opA, opB;
	private SceneObject selectedObj;

	public void LoadGameObjects() {
		sceneObjs = new List<SceneObject>();

		SceneObject cube = Instantiate(CSGObjectPrefab) as SceneObject;
		SceneObject sphere = Instantiate(CSGObjectPrefab) as SceneObject;
		PrimitiveHelper.SetAsType(cube, PrimitiveType.Cube);
		PrimitiveHelper.SetAsType(sphere, PrimitiveType.Sphere);
		//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		

		//Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;
		//cube.GetComponent<MeshFilter>().sharedMesh = cubeMesh;
		cube.transform.localPosition = new Vector3(-2,2,0);
		sphere.transform.localPosition = new Vector3(-2,-2,0);
		sphere.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		sceneObjs.Add(cube);
		sceneObjs.Add(sphere);

		targetObj = Instantiate(targetPrefab) as GameObject;
		targetObj.name = "Target";
		targetObj.GetComponent<MeshFilter>().sharedMesh = Instantiate(targetMesh) as Mesh;
	}

	void Update() {

	}



	public void OnSceneObjClick(SceneObject obj) {
		if (selectedObj == obj) {
			return;
		}
		if (selectedObj) {
			selectedObj.OnDeselect();
		}
		selectedObj = obj;
		selectedObj.OnSelect();
		if (opA == null) {
			opA = obj;
		}
		else if (opB == null) {
			opB = obj;
		}
		else {
			opA.SetDefaultMaterial();
			opA = opB;
			opB = obj;
		}
		UpdateMaterial();
	}

	private void UpdateMaterial() {
		if (opA != null) {
			opA.SetOpAMaterial();
		}
		if (opB != null) {
			opB.SetOpBMaterial();
		}
	}





	public void SubtractAB() {
		if (opA && opB) {
			CSGUtil.Subtract(opA.gameObject, opB.gameObject);
			sceneObjs.Remove(opB);
			Destroy(opB.gameObject);

			//Need to set to default because this is no longer opA
			opA.SetDefaultMaterial();
			opA.GenerateBarycentric();
			opA = null;
		}
	}

	public void SubtractBA() {
		if (opA && opB) {
			CSGUtil.Subtract(opB.gameObject, opA.gameObject);
			sceneObjs.Remove(opA);
			Destroy(opA.gameObject);

			//Need to set to default because this is no longer opA
			opB.SetDefaultMaterial();
			opB.GenerateBarycentric();
			opB = null;
		}
	}

	public void Intersect() {
		if (opA && opB) {
			CSGUtil.Intersect(opA.gameObject, opB.gameObject);
			sceneObjs.Remove(opB);
			Destroy(opB.gameObject);

			//Need to set to default because this is no longer opA
			opA.SetDefaultMaterial();
			opA.GenerateBarycentric();
			opA = null;
		}
	}

	public void Union() {
		if (opA && opB) {
			CSGUtil.Union(opA.gameObject, opB.gameObject);
			sceneObjs.Remove(opB);
			Destroy(opB.gameObject);

			//Need to set to default because this is no longer opA
			opA.SetDefaultMaterial();
			opA.GenerateBarycentric();
			opA = null;
		}
	}

	public void Check() {
		if (sceneObjs.Count == 1) {
			GameObject composite = sceneObjs[0].gameObject;
			float compositeVolume = CSGUtil.VolumeOfMesh(composite);
			float targetVolume = CSGUtil.VolumeOfMesh(targetObj);
			//GameObject flagObj = Instantiate(composite) as GameObject;
			CSGUtil.Union(targetObj, composite);
			float unionVolume = CSGUtil.VolumeOfMesh(targetObj);
			
			Debug.Log(unionVolume + " " + targetVolume + " " + compositeVolume);
			if ((unionVolume - targetVolume < 2e-2) && (unionVolume - compositeVolume < 2e-2)) {
				Debug.Log(unionVolume + " " + targetVolume + " " + compositeVolume);
				Debug.Log("You win...!!!");
				Destroy(composite);
				Destroy(targetObj);
				return;
			}
			targetObj.GetComponent<MeshFilter>().sharedMesh = Instantiate(targetMesh) as Mesh;
			targetObj.GetComponent<MeshRenderer>().materials
				= new Material[] {targetMaterial};
			targetObj.name = "Target";
		}
	}

	public void DeselectAll() {
		if (opA)
			opA.SetDefaultMaterial();
		if (opB)
			opB.SetDefaultMaterial();
		opA = null;
		opB = null;

		if (selectedObj)
			selectedObj.OnDeselect();
		selectedObj = null;
	}

	public void ResetScene() {
		opA = null;
		opB = null;
		selectedObj = null;
		foreach (SceneObject obj in sceneObjs) {
			Destroy(obj.gameObject);
		}
		Destroy(targetObj);
		LoadGameObjects();
	}

}

/*public class ObjectsManager : MonoBehaviour {

	public Material opAMaterial, opBMaterial;
	public Material defaultMaterial;
	public Material targetMaterial;
	public Mesh targetMesh;
	public Material wireframeMaterial = null;

	private GameObject target;

	private List<GameObject> gameObjects = new List<GameObject>();
	private GameObject opA, opB;
	private GameObject selectedGameObject;

	private bool wireframe = false;

	float wireframe_alpha = 0f, cur_alpha = 0f, dest_alpha = 1f, start_time = 0f;

	public void LoadGameObjects() {

		GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		obj1.transform.parent = this.transform;
		obj2.transform.parent = this.transform;
		obj1.transform.localPosition = new Vector3(-3,-3,0);
		obj2.transform.localPosition = new Vector3(-3,3,0);
		obj1.AddComponent<CSGObject>();
		obj2.AddComponent<CSGObject>();
		obj1.AddComponent<ObjectBehaviors>();
		obj2.AddComponent<ObjectBehaviors>();
		//obj1.GetComponent<MeshRenderer>().material = wireframeMaterial;
		//obj2.GetComponent<MeshRenderer>().material = wireframeMaterial;
		obj2.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		gameObjects.Add(obj1);
		gameObjects.Add(obj2);
		obj1.GetComponent<ObjectBehaviors>().GenerateBarycentric();
		obj2.GetComponent<ObjectBehaviors>().GenerateBarycentric();
		//opA = obj1;
		//opB = obj2;

		target = new GameObject("Target");
		Debug.Log("Target Volume" + CSGUtil.VolumeOfMesh(targetMesh));

		target.AddComponent<MeshFilter>().sharedMesh = targetMesh;
		target.AddComponent<MeshRenderer>().sharedMaterial = wireframeMaterial;

		wireframeMaterial.SetFloat("_Opacity", 0);
		cur_alpha = 0f;
		dest_alpha = 0f;
		
		ToggleWireframe();
	}

	public void ToggleWireframe()
	{
		wireframe = !wireframe;

		cur_alpha = wireframe ? 0f : 1f;
		dest_alpha = wireframe ? 1f : 0f;
		start_time = Time.time;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		wireframe_alpha = Mathf.Lerp(cur_alpha, dest_alpha, Time.time - start_time);
		wireframeMaterial.SetFloat("_Opacity", wireframe_alpha);
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

			CSGObject obj = opA.GetComponent<CSGObject>();
			GameObject[] slaves = new GameObject[2] {opA, opB};
			Destroy(opB);
			obj.PerformCSG(CsgOperation.ECsgOperation.CsgOper_Subtractive, slaves);
			Destroy(opA.GetComponent<Collider>());

			GameObject union = Instantiate(opA) as GameObject;
			CSGObject unionCSG = union.AddComponent<CSGObject>();
			slaves = new GameObject[2] {union, target};
			unionCSG.PerformCSG(CsgOperation.ECsgOperation.CsgOper_Additive, slaves);
			Debug.Log(CSGUtil.VolumeOfMesh(union.GetComponent<MeshFilter>().mesh));

			/*GameObject composite = CSGUtil.Subtract(opA, opB, defaultMaterial);
			composite.name = "(" + opA.name + ") - (" + opB.name + ")";
			gameObjects.Remove(opA);
			gameObjects.Remove(opB);
			gameObjects.Add(composite);
			composite.GetComponent<ObjectBehaviors>().GenerateBarycentric();
			Destroy(opA);
			Destroy(opB);
			Debug.Log("There are " + gameObjects.Count + " objects left in scene");

			Mesh m = composite.GetComponent<MeshFilter>().sharedMesh;
			Debug.Log("your volume: " + CSGUtil.VolumeOfMesh(m));
			Mesh m1 = CSG.Subtract(target, composite);
			Mesh m2 = CSG.Subtract(composite, target);
			//GameObject g1 = CSGUtil.Subtract(target, composite, wireframeMaterial);
			//GameObject g2 = CSGUtil.Subtract(composite, target, wireframeMaterial);
			//g1.transform.localPosition = new Vector3(-2,0,0);
			//g2.transform.localPosition = new Vector3(2,0,0);
			//g1.AddComponent<ObjectBehaviors>().GenerateBarycentric();
			//g2.AddComponent<ObjectBehaviors>().GenerateBarycentric();
			Debug.Log("union volume: " + CSGUtil.VolumeOfMesh(m1));
			Debug.Log("intersection volume: " + CSGUtil.VolumeOfMesh(m2));
			if (CSGUtil.VolumeOfMesh(m1) - CSGUtil.VolumeOfMesh(m2) < 1e-2) {
				Debug.Log("You completed the level..!!");
			}*/
/*		}	
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
		//UpdateMaterial();
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

}*/
