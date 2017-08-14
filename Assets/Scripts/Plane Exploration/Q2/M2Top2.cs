using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Top2 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] top2;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	public bool collide = false;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));

		// get mesh collider
		meshCollider = (MeshCollider)gameObject.GetComponent(typeof(MeshCollider));


		// set mesh
		top2 = new Vector3[] {
			new Vector3 (1, 1, 0),
			new Vector3 (1, 1, 2), 
			new Vector3 (2, 1, 2),
			new Vector3 (2, 1, 0)
		};

		mesh = new Mesh ();

		mesh.vertices = new Vector3[] {
			top2 [0], top2 [1], top2 [3],
			top2 [3], top2 [1], top2 [2]
		};

		mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5 };

		meshCollider.sharedMesh = mesh;

		// set collide
		collide = false;

	}

	// render
	void Update () {

		if (collide)
			meshFilter.mesh = mesh;

	}

	void OnTriggerEnter(Collider other) 
	{
		collide = true;
		player.GetComponent<Player2>().top2 = true;
	}
}
