using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5Front1 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] front1;

	private MeshFilter meshFilter;
	private Mesh mesh;

	public bool collide = false;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));


		// set mesh
		front1 = new Vector3[] {
			new Vector3 (1, 2, 0),
			new Vector3 (1, 2, 4), 
			new Vector3 (3, 0, 4),
			new Vector3 (3, 0, 0)
		};

		mesh = new Mesh ();

		mesh.vertices = new Vector3[] {
			front1 [0], front1 [1], front1 [3],
			front1 [3], front1 [1], front1 [2]
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
			player.GetComponent<Player5> ().front1 = true;
		}
	}
}
