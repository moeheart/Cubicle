using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model4 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] top1, top2, bottom1, bottom2, left1, left2, right1, right2, right3,
		front1, back1, back2, back3;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	private Vector3[] edgeTrack;

	private LineRenderer lineRenderer;
	private int lineLength;

	// define points
	void Start () {

		// object name is Model
		ModelGameObject = GameObject.Find ("Model");

		// get mesh from mesh filter
		meshFilter = (MeshFilter)ModelGameObject.GetComponent (typeof(MeshFilter));
		mesh = meshFilter.mesh;

		// get line renderer
		lineRenderer = (LineRenderer)ModelGameObject.GetComponent (typeof(LineRenderer));

		// get mesh collider
		meshCollider = (MeshCollider)ModelGameObject.GetComponent (typeof(MeshCollider));

		top1 = new Vector3[] {
			new Vector3 (0, 2, 0),
			new Vector3 (0, 2, 1), 
			new Vector3 (2, 2, 1),
			new Vector3 (2, 2, 0)
		};

		top2 = new Vector3[] {
			new Vector3 (0, 1, 1),
			new Vector3 (0, 1, 2), 
			new Vector3 (1, 1, 2),
			new Vector3 (1, 1, 1)
		};

		bottom1 = new Vector3[] {
			new Vector3 (2, 0, 0),
			new Vector3 (2, 0, 4), 
			new Vector3 (1, 0, 4),
			new Vector3 (1, 0, 2),
			new Vector3 (0, 0, 2),
			new Vector3 (0, 0, 1),
			new Vector3 (1, 0, 1),
			new Vector3 (1, 0, 0)
		};

		bottom2 = new Vector3[] {
			new Vector3 (0, 1, 0),
			new Vector3 (1, 1, 0), 
			new Vector3 (1, 1, 1),
			new Vector3 (0, 1, 1),
		};

		left1 = new Vector3[] {
			new Vector3 (0, 2, 0),
			new Vector3 (2, 2, 0), 
			new Vector3 (2, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (0, 1, 0)
		};

		left2 = new Vector3[] {
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1), 
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1)
		};

		right1 = new Vector3[] {
			new Vector3 (2, 2, 1),
			new Vector3 (1, 2, 1),
			new Vector3 (1, 0, 4),
			new Vector3 (2, 0, 4)
		};

		right2 = new Vector3[] {
			new Vector3 (1, 0, 2),
			new Vector3 (1, 1, 2),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 0, 2)
		};

		right3 = new Vector3[] {
			new Vector3 (1, 2, 1),
			new Vector3 (0, 2, 1),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1)
		};

		front1 = new Vector3[] {
			new Vector3 (2, 2, 0),
			new Vector3 (2, 2, 1), 
			new Vector3 (2, 0, 4),
			new Vector3 (2, 0, 0)
		};

		back1 = new Vector3[] {
			new Vector3 (0, 2, 0),
			new Vector3 (0, 1, 0), 
			new Vector3 (0, 1, 1),
			new Vector3 (0, 2, 1)
		};

		back2 = new Vector3[] {
			new Vector3 (0, 1, 1), 
			new Vector3 (0, 0, 1),
			new Vector3 (0, 0, 2),
			new Vector3 (0, 1, 2),
		};

		back3 = new Vector3[] {
			new Vector3 (1, 2, 0), 
			new Vector3 (1, 0, 0),
			new Vector3 (1, 0, 4),
			new Vector3 (1, 2, 1),
		};

		// define edgetrack for line rendering
		edgeTrack = new Vector3[] {	
			new Vector3 (2, 2, 1), // top1
			new Vector3 (2, 2, 0),
			new Vector3 (0, 2, 0),
			new Vector3 (0, 2, 1),
			new Vector3 (2, 2, 1),
			new Vector3 (2, 0, 4), // right1
			new Vector3 (1, 0, 4),
			new Vector3 (1, 2, 1),
			new Vector3 (1, 1, 1), // right3
			new Vector3 (0, 1, 1),
			new Vector3 (0, 2, 1), // back1
			new Vector3 (0, 2, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 0, 2),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1), // right2
			new Vector3 (1, 2, 1),
			new Vector3 (1, 1.01f, 1), // top2
			new Vector3 (1, 1.01f, 2),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 1, 1), // left2
			new Vector3 (0, 0, 1),
			new Vector3 (1, 0, 1),
			new Vector3 (1, 1, 1),
			new Vector3 (1, 1, 0),
			new Vector3 (1, 0, 0), // left1
			new Vector3 (1, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 2, 0),
			new Vector3 (2, 2, 0),
			new Vector3 (2, 0, 0),
			new Vector3 (1, 0, 0), // bottom1
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 0, 2),
			new Vector3 (1, 0, 2),
			new Vector3 (1, 1, 2),
			new Vector3 (1, 0, 2),
			new Vector3 (1, 0, 4),
			new Vector3 (2, 0, 4),
			new Vector3 (2, 0, 0),
		};

		// get line length
		lineLength = edgeTrack.Length;
		lineRenderer.SetVertexCount(lineLength);

	}

	// render
	void Update () {

		/* render mesh */
		mesh.vertices = new Vector3[] {
			top1[0], top1[1], top1[3],
			top1[3], top1[1], top1[2],
			top2[0], top2[1], top2[3],
			top2[3], top2[1], top2[2],
			bottom1[0], bottom1[1], bottom1[7], // 5
			bottom1[7], bottom1[1], bottom1[2],
			bottom1[3], bottom1[4], bottom1[5],
			bottom1[3], bottom1[5], bottom1[6],
			bottom2[0], bottom2[1], bottom2[3],
			bottom2[3], bottom2[1], bottom2[2], // 10
			left1[0], left1[1], left1[2],
			left1[0], left1[4], left1[5],
			left1[4], left1[2], left1[3],
			left2[0], left2[1], left2[3],
			left2[3], left2[1], left2[2], // 15
			right1[0], right1[1], right1[3],
			right1[3], right1[1], right1[2],
			right2[0], right2[1], right2[3],
			right2[3], right2[1], right2[2],
			right3[0], right3[1], right3[3], // 20
			right3[3], right3[1], right3[2],
			front1[0], front1[1], front1[3],
			front1[3], front1[1], front1[2],
			back1[0], back1[1], back1[3],
			back1[3], back1[1], back1[2], // 25
			back2[0], back2[1], back2[3],
			back2[3], back2[1], back2[2],
			back3[0], back3[1], back3[3],
			back3[3], back3[1], back3[2], // 29
		};
			
		// set indices
		mesh.triangles = new int[] {
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, // 5
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, // 10
			30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, // 15
			45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, // 20
			60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, // 25
			75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86 // 29
		};

		// set collider
		meshCollider.sharedMesh = mesh;

		/* render line */
		for (int i = 0; i < lineLength; i ++)
			lineRenderer.SetPosition(i, edgeTrack[i]);

		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}
}
