using UnityEngine;
using System.Collections;

public class SceneObject : MonoBehaviour {

	public Material wireframeMaterial;
	[SerializeField]
	public ArrowControl[] arrowPrefabs;

	private bool isSelected;
	private Material myMaterial;
	private ArrowControl[] arrows;

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		//wireframe_alpha = Mathf.Lerp(cur_alpha, dest_alpha, Time.time - start_time);
		//wireframeMaterial.SetFloat("_Opacity", wireframe_alpha);
		//TODO
		//These controls are pretty bad right now
		//Change it to something more natural
		//The Y-axis

		if (isSelected == true) {
			if (Input.GetKeyDown(KeyCode.W)) {
				this.transform.localPosition += new Vector3(0,1,0);
			}
			if (Input.GetKeyDown(KeyCode.S)) {
				this.transform.localPosition += new Vector3(0,-1,0);
			}

			//The X-axis
			if (Input.GetKeyDown(KeyCode.A)) {
				this.transform.localPosition += new Vector3(-1,0,0);
			}
			if (Input.GetKeyDown(KeyCode.D)) {
				this.transform.localPosition += new Vector3(1,0,0);
			}

			//The Z-axis
			if (Input.GetKeyDown(KeyCode.Q)) {
				this.transform.localPosition += new Vector3(0,0,1);
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				this.transform.localPosition += new Vector3(0,0,-1);
			}

			//Rotate around Y-axis
			if (Input.GetKeyDown(KeyCode.L)) {
				this.transform.Rotate(0,90,0);
			}
			if (Input.GetKeyDown(KeyCode.J)) {
				this.transform.Rotate(0,-90,0);
			}

			//Rotate around X-axis
			if (Input.GetKeyDown(KeyCode.I)) {
				this.transform.Rotate(90,0,0);
			}
			if (Input.GetKeyDown(KeyCode.K)) {
				this.transform.Rotate(-90,0,0);
			}

			//Rotate around Z-axis
			if (Input.GetKeyDown(KeyCode.U)) {
				this.transform.Rotate(0,0,90);
			}
			if (Input.GetKeyDown(KeyCode.O)) {
				this.transform.Rotate(0,0,-90);
			}
		}
	}

	public void Init() {
		GenerateBarycentric();
		myMaterial = Instantiate(wireframeMaterial) as Material;
		this.GetComponent<MeshRenderer>().sharedMaterials 
			= new Material[1] {myMaterial};
		myMaterial.SetFloat("_Opacity", 0);
		this.GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
		GenerateControls();
	}

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			CSGManager.objectsManager.OnSceneObjClick(this);
		}
	}

	public void OnSelect() {
		//Debug.Log("Selected: " + this.name + "...!!!");
		Debug.Log(this.name + " " + GetComponent<Collider>().bounds);
		isSelected = true;
		for (int i = 0; i < 6; ++i) {
			arrows[i].gameObject.SetActive(false);
		}
	}

	public void OnDeselect() {
		//Debug.Log("Deselected: " + this.name + "...!!!");
		isSelected = false;
		for (int i = 0; i < 6; ++i) {
			arrows[i].gameObject.SetActive(false);
		}
	}

	public void SetDefaultMaterial() {
		HideWireframe();
		myMaterial.color = Color.white;
	}

	public void SetOpAMaterial() {
		DisplayWireframe();
		myMaterial.color = Color.red;
	}

	public void SetOpBMaterial() {
		DisplayWireframe();
		myMaterial.color = Color.green;
	}

	public void GenerateControls() {
		if (arrows != null) {
			foreach (ArrowControl arrow in arrows) {
				Destroy(arrow.gameObject);
			}
		}
		else {
			arrows = new ArrowControl[6];
		}
		//Vector3 center = GetComponent<Renderer>().bounds.center;
		Vector3 extents = GetComponent<Renderer>().bounds.extents;
		for (int i = 0; i < 6; ++i) {
			arrows[i] = Instantiate(arrowPrefabs[i]) as ArrowControl;
			arrows[i].transform.parent = this.transform;
			arrows[i].gameObject.SetActive(false);
		}
		arrows[0].transform.localPosition = new Vector3(extents.x, 0, 0);
		arrows[1].transform.localPosition = new Vector3(0, 0, extents.z);
		arrows[2].transform.localPosition = new Vector3(-extents.x, 0, 0);
		arrows[3].transform.localPosition = new Vector3(0, 0, -extents.z);
		arrows[4].transform.localPosition = new Vector3(0, extents.y, 0);
		arrows[5].transform.localPosition = new Vector3(0, -extents.y, 0);
	}

	public void MoveInDirection(ArrowDirection direction) {
		switch (direction) {
			case ArrowDirection.plusY: 
				this.transform.localPosition += new Vector3(0,1,0);
				break;
			case ArrowDirection.minusY: 
				this.transform.localPosition += new Vector3(0,-1,0);
				break;

			case ArrowDirection.plusX: 
				this.transform.localPosition += new Vector3(1,0,0);
				break;
			case ArrowDirection.minusX: 
				this.transform.localPosition += new Vector3(-1,0,0);
				break;

			case ArrowDirection.plusZ: 
				this.transform.localPosition += new Vector3(0,0,1);
				break;
			case ArrowDirection.minusZ: 
				this.transform.localPosition += new Vector3(0,0,-1);
				break;
		}
	}

	private void DisplayWireframe() {
		myMaterial.SetFloat("_Opacity", 1f);
	}

	private void HideWireframe() {
		myMaterial.SetFloat("_Opacity", 0f);
	}

	public void GenerateBarycentric()
	{
		GameObject go = this.gameObject;
		Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

		if(m == null) return;

		int[] tris = m.triangles;
		int triangleCount = tris.Length;

		Vector3[] mesh_vertices		= m.vertices;
		Vector3[] mesh_normals		= m.normals;
		Vector2[] mesh_uv			= m.uv;

		Vector3[] vertices 	= new Vector3[triangleCount];
		Vector3[] normals 	= new Vector3[triangleCount];
		Vector2[] uv 		= new Vector2[triangleCount];
		Color[] colors 		= new Color[triangleCount];

		for(int i = 0; i < triangleCount; i++)
		{
			vertices[i] = mesh_vertices[tris[i]];
			normals[i] 	= mesh_normals[tris[i]];
			uv[i] 		= mesh_uv[tris[i]];

			colors[i] = i % 3 == 0 ? new Color(1, 0, 0, 0) : (i % 3) == 1 ? new Color(0, 1, 0, 0) : new Color(0, 0, 1, 0);

			tris[i] = i;
		}

		Mesh wireframeMesh = new Mesh();

		wireframeMesh.Clear();
		wireframeMesh.vertices = vertices;
		wireframeMesh.triangles = tris;
		wireframeMesh.normals = normals;
		wireframeMesh.colors = colors;
		wireframeMesh.uv = uv;

		go.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
	}
}
