using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5Top2 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] top2;

	private MeshFilter meshFilter;
	private Mesh mesh;

	public bool collide = false;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));


		// set mesh
		top2 = new Vector3[] {
			new Vector3 (0, 2, 3),
			new Vector3 (0, 2, 4), 
			new Vector3 (1, 2, 4),
			new Vector3 (1, 2, 3)
		};

		mesh = new Mesh ();

		mesh.vertices = new Vector3[] {
			top2 [0], top2 [1], top2 [3],
			top2 [3], top2 [1], top2 [2]
		};

		mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5 };

		// set collide
		collide = false;


	}

	// render
	void Update () {

		if (collide)
			meshFilter.mesh = mesh;

	}

	// collision trigger
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player")) {
			collide = true;
			player.GetComponent<Player5> ().top2 = true;
		}
	}
}
