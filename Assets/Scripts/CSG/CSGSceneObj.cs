using UnityEngine;
using System.Collections;

public class CSGSceneObj : CSGObject {

	private bool isSelected {get; set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
		//These controls are pretty bad right now
		//Change it to something more natural
		//The Y-axis

		/*
		if (Input.GetKeyDown(KeyCode.I)) {
			this.transform.localPosition += new Vector3(0,1,0);
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			this.transform.localPosition += new Vector3(0,-1,0);
		}

		//The X-axis
		if (Input.GetKeyDown(KeyCode.J)) {
			this.transform.localPosition += new Vector3(-1,0,0);
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			this.transform.localPosition += new Vector3(1,0,0);
		}

		//The Z-axis
		if (Input.GetKeyDown(KeyCode.U)) {
			this.transform.localPosition += new Vector3(0,0,1);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			this.transform.localPosition += new Vector3(0,0,-1);
		}

		//Rotate around Y-axis
		if (Input.GetKeyDown(KeyCode.D)) {
			this.transform.Rotate(0,90,0);
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			this.transform.Rotate(0,-90,0);
		}

		//Rotate around X-axis
		if (Input.GetKeyDown(KeyCode.W)) {
			this.transform.Rotate(90,0,0);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			this.transform.Rotate(-90,0,0);
		}

		//Rotate around Z-axis
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.transform.Rotate(0,0,90);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			this.transform.Rotate(0,0,-90);
		}
		*/
	}

	/*public void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			CSGManager.objectsManager.OnGameObjectClick(this.gameObject);
		}
	}*/

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
