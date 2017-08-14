using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5Curve1 : MonoBehaviour {

	public GameObject gameObject;
	public GameObject player;

	private Vector3[] curve1;

	private MeshFilter meshFilter;
	private Mesh mesh;

	public bool collide = false;

	private static int curveSegNum = 12;

	void Start () {

		// get mesh from mesh filter
		meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));

		Vector3 pStart = new Vector3 (0, 2, 2);
		Vector3 pEnd = new Vector3 (1, 2, 2);
		float r = 1;
		float startDegree = 180;
		float endDegree = 360;
		curve1 = GetCurve (pStart, pEnd, startDegree, endDegree, r, curveSegNum);

		mesh = new Mesh ();

		mesh.vertices = GetCurve (pStart, pEnd, startDegree, endDegree, r, curveSegNum);

		mesh.triangles = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, // 5
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, // 10
			30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, // 15
			45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, // 20
			60, 61, 62, 63, 64, 65, };

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
			player.GetComponent<Player5> ().curve1 = true;
		}
	}

	Vector3[] GetCurve(Vector3 pStart, Vector3 pEnd, float dStart, float dEnd, float r, int segNum){

		Vector3[] curveArray = new Vector3[(segNum - 1) * 6];
		Vector3 s0, s1, e0, e1;
		float offsetZ, offsetY;

		offsetZ = r * Mathf.Cos (dStart * Mathf.PI / 180);
		offsetY = r * Mathf.Sin (dStart * Mathf.PI / 180);
		s0 = new Vector3 (pStart [0], pStart [1] + offsetY, pStart [2] + offsetZ);
		e0 = new Vector3 (pEnd [0], pEnd [1] + offsetY, pEnd [2] + offsetZ);


		for (int i = 1; i < segNum; i++) {

			offsetZ = r * Mathf.Cos ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);
			offsetY = r * Mathf.Sin ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);

			s1 = new Vector3 (pStart [0], pStart [1] + offsetY, pStart [2] + offsetZ);
			e1 = new Vector3 (pEnd [0], pEnd [1] + offsetY, pEnd [2] + offsetZ);

			curveArray [6 * i - 6] = s0;
			curveArray [6 * i - 5] = e1;
			curveArray [6 * i - 4] = e0;
			curveArray [6 * i - 3] = s0;
			curveArray [6 * i - 2] = s1;
			curveArray [6 * i - 1] = e1;

			s0 = s1;
			e0 = e1;

		}

		return curveArray;
	}
}
