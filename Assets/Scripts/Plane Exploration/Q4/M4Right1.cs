using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4Right1 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] right1;

	private MeshFilter meshFilter;
	private Mesh mesh;

	public bool collide = false;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));


		// set mesh
		right1 = new Vector3[] {
			new Vector3 (2, 2, 1),
			new Vector3 (1, 2, 1),
			new Vector3 (1, 0, 4),
			new Vector3 (2, 0, 4)
		};

		mesh = new Mesh ();

		mesh.vertices = new Vector3[] {
			right1 [0], right1 [1], right1 [3],
			right1 [3], right1 [1], right1 [2]
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
			player.GetComponent<Player4> ().right1 = true;
		}
	}
}
