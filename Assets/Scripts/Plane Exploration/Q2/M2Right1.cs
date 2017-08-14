using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Right1 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] right1;

	private MeshFilter meshFilter;
	private Mesh mesh;

	public bool collide;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));

		// set mesh
		right1 = new Vector3[] {
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2),
			new Vector3(0, 0, 3), 
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 2), 
			new Vector3(1, 1, 2),
			new Vector3(1, 2, 1)
		};

		mesh = new Mesh ();

		mesh.vertices = new Vector3[] {
			right1[0], right1[1], right1[6],
			right1[6], right1[1], right1[5],
			right1[4], right1[1], right1[2],
			right1[4], right1[2], right1[3]
		};

		mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 ,9, 10, 11 };

	}

	// render
	void Update () {

		if (collide)
			meshFilter.mesh = mesh;

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			collide = true;
			player.GetComponent<Player2>().right1 = true;
		}
	}

}
