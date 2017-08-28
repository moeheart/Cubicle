using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model7 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] top1, top2, top3, back1, left1, left2, right1, right2, front1, front2,
		fronttop1, fronttop2, righttop1, bottom1, bottom2;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	private Vector3[] edgeTrack;

	private LineRenderer lineRenderer;
	private int lineLength;

	public GameObject logObject;
	private int trialNum;


	void Start () {

		trialNum = 0;

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
			new Vector3 (0, 3, 0),
			new Vector3 (0, 3, 1), 
			new Vector3 (1, 3, 1),
			new Vector3 (1, 3, 0)
		};

		top2 = new Vector3[] {
			new Vector3 (0, 2, 2),
			new Vector3 (0, 2, 3), 
			new Vector3 (1, 2, 3),
			new Vector3 (1, 2, 2)
		};

		top3 = new Vector3[] {
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 3), 
			new Vector3 (3, 1, 3),
			new Vector3 (3, 1, 0)
		};

		back1 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (0, 0, 1), 
			new Vector3 (0, 3, 0),
			new Vector3 (0, 3, 0),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 3, 1),
			new Vector3 (0, 2, 2),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 2, 3),
			new Vector3 (0, 1, 1),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 1, 2),
			new Vector3 (0, 2, 3),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 0, 3),
			new Vector3 (0, 3, 1),
			new Vector3 (0, 2, 1),
			new Vector3 (0, 2, 2),
		};

		right1 = new Vector3[] {
			new Vector3 (0, 2, 3),
			new Vector3 (0, 0, 3),
			new Vector3 (3, 0, 3),
			new Vector3 (0, 2, 3),
			new Vector3 (3, 0, 3),
			new Vector3 (1, 2, 3),
			new Vector3 (3, 0, 3),
			new Vector3 (3, 1, 3),
			new Vector3 (2, 1, 3),
		};

		right2 = new Vector3[] {
			new Vector3 (0, 2, 1),
			new Vector3 (0, 1, 1),
			new Vector3 (2, 1, 1),
			new Vector3 (1, 2, 1)
		};

		left1 = new Vector3[] {
			new Vector3 (0, 3, 0),
			new Vector3 (1, 3, 0), 
			new Vector3 (1, 2, 0),
			new Vector3 (2, 1, 0),
			new Vector3 (3, 1, 0),
			new Vector3 (3, 0, 0),
			new Vector3 (0, 0, 0),
			new Vector3 (0, 3, 0),
			new Vector3 (3, 0, 0),
		};

		left2 = new Vector3[] {
			new Vector3 (0, 1, 2),
			new Vector3 (0, 2, 2), 
			new Vector3 (1, 2, 2),
			new Vector3 (2, 1, 2)
		};


		front1 = new Vector3[] {
			new Vector3 (1, 3, 0),
			new Vector3 (1, 3, 1), 
			new Vector3 (1, 2, 2),
			new Vector3 (1, 2, 0)
		};

		front2 = new Vector3[] {
			new Vector3 (3, 1, 0),
			new Vector3 (3, 1, 3), 
			new Vector3 (3, 0, 3),
			new Vector3 (3, 0, 0)
		};

		fronttop1 = new Vector3[] {
			new Vector3 (1, 2, 0),
			new Vector3 (1, 2, 1), 
			new Vector3 (2, 1, 1),
			new Vector3 (2, 1, 0)
		};

		fronttop2 = new Vector3[] {
			new Vector3 (1, 2, 2),
			new Vector3 (1, 2, 3), 
			new Vector3 (2, 1, 3),
			new Vector3 (2, 1, 2)
		};

		righttop1 = new Vector3[] {
			new Vector3 (0, 3, 1),
			new Vector3 (0, 2, 2), 
			new Vector3 (1, 2, 2),
			new Vector3 (1, 3, 1)
		};

		bottom1 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (3, 0, 0), 
			new Vector3 (3, 0, 3),
			new Vector3 (0, 0, 3)
		};

		bottom2 = new Vector3[] {
			new Vector3 (0, 2, 0),
			new Vector3 (1, 2, 0), 
			new Vector3 (1, 2, 2),
			new Vector3 (0, 2, 2)
		};



		// define edgetrack for line rendering
		edgeTrack = new Vector3[] {	
			new Vector3 (0, 3.01f, 0), // back1
			new Vector3 (0, 0, 0),
			new Vector3 (0, 0, 3),
			new Vector3 (0, 2.01f, 3),
			new Vector3 (0, 2.01f, 2),
			new Vector3 (0, 3.01f, 1),
			new Vector3 (0, 3.01f, 0), // top1
			new Vector3 (1, 3.01f, 0),
			new Vector3 (1, 3.01f, 1),
			new Vector3 (0, 3.01f, 1),
			new Vector3 (0, 2.01f, 2), // righttop
			new Vector3 (1, 2.01f, 2),
			new Vector3 (1, 3.01f, 1),
			new Vector3 (1, 2.01f, 2), // front2
			new Vector3 (1, 2.01f, 0),
			new Vector3 (1, 3.01f, 0),
			new Vector3 (1, 2.01f, 0), // fronttop1
			new Vector3 (2, 1.01f, 0),
			new Vector3 (2, 1.01f, 1),
			new Vector3 (1, 2.01f, 1), // right2
			new Vector3 (0, 2.01f, 1),
			new Vector3 (0, 1.01f, 1),
			new Vector3 (2, 1.01f, 1),
			new Vector3 (2, 1.01f, 0),
			new Vector3 (3, 1.01f, 0), // top3
			new Vector3 (3, 0, 0),
			new Vector3 (0, 0, 0), // bottom
			new Vector3 (0, 0, 3),
			new Vector3 (3, 0, 3),
			new Vector3 (3, 0, 0), // front1
			new Vector3 (3, 1.01f, 0),
			new Vector3 (3, 1.01f, 3),
			new Vector3 (3, 0, 3),
			new Vector3 (3, 1.01f, 3),
			new Vector3 (2, 1.01f, 3),
			new Vector3 (1, 2.01f, 3), // right1
			new Vector3 (0, 2.01f, 3),
			new Vector3 (1, 2.01f, 3),
			new Vector3 (1, 2.01f, 2),
			new Vector3 (1, 2.01f, 3),
			new Vector3 (2, 1.01f, 3),
			new Vector3 (2, 1.01f, 2), // fronttop2
			new Vector3 (1, 2.01f, 2),
			new Vector3 (2, 1.01f, 2), // left2
			new Vector3 (0, 1.01f, 2),
			new Vector3 (0, 1.01f, 1),
			new Vector3 (0, 2.01f, 1),
			new Vector3 (0, 2.01f, 2),
			new Vector3 (0, 1.01f, 2),
		};

		// get line length
		lineLength = edgeTrack.Length;
		lineRenderer.SetVertexCount(lineLength);

		InitializeLog ();
	}

	public void InitializeLog(){
		logObject.GetComponent<PlaneExplorationLog> ().RecordInitialization (trialNum, 7);
		trialNum++;
	}
	// render
	void Update () {

		/* render mesh */
		mesh.vertices = new Vector3[] {
			top1[0], top1[1], top1[3],
			top1[3], top1[1], top1[2],
			top2[0], top2[1], top2[3],
			top2[3], top2[1], top2[2],
			top3[0], top3[1], top3[3], // 5
			top3[3], top3[1], top3[2],
			back1[0], back1[1], back1[2], 
			back1[3], back1[4], back1[5], 
			back1[6], back1[7], back1[8],
			back1[9], back1[10], back1[11],
			back1[12], back1[13], back1[14],
			back1[15], back1[16], back1[17],
			right1[0], right1[1], right1[2], // 
			right1[3], right1[4], right1[5], 
			right1[6], right1[7], right1[8],
			left1[0], left1[1], left1[2], 
			left1[3], left1[4], left1[5], 
			left1[6], left1[7], left1[8], // 
			left2[0], left2[1], left2[3],
			left2[3], left2[1], left2[2],
			front1[0], front1[1], front1[3],
			front1[3], front1[1], front1[2],
			front2[0], front2[1], front2[3], // 
			front2[3], front2[1], front2[2],
			fronttop1[0], fronttop1[1], fronttop1[3],
			fronttop1[3], fronttop1[1], fronttop1[2],
			fronttop2[0], fronttop2[1], fronttop2[3],
			fronttop2[3], fronttop2[1], fronttop2[2], // 
			righttop1[0], righttop1[1], righttop1[3],
			righttop1[3], righttop1[1], righttop1[2],
			bottom2[0], bottom2[1], bottom2[3],
			bottom2[3], bottom2[1], bottom2[2], //32
			bottom1[0], bottom1[1], bottom1[3],
			bottom1[3], bottom1[1], bottom1[2], //34
			right2[0], right2[1], right2[3],
			right2[3], right2[1], right2[2], //36
		};
			
		// set indices
		mesh.triangles = new int[] {
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, // 5
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, // 10
			30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, // 15
			45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, // 20
			60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, // 25
			75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, // 30
			90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, //35
			105, 106, 107
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
