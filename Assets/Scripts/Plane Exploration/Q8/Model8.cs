using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model8 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] top1, top2, top3, front1, front2, front3, back1, back2, back3, right1, left1,
		left2, righttop1, righttop2, lefttop1, lefttop2, fronttop1, backtop1, right2, left3, bottom1, back4;

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
			new Vector3 (0, 3, 3),
			new Vector3 (0, 3, 4), 
			new Vector3 (1, 3, 4),
			new Vector3 (1, 3, 3)
		};

		top2 = new Vector3[] {
			new Vector3 (1, 2, 1),
			new Vector3 (1, 2, 2), 
			new Vector3 (2, 2, 2),
			new Vector3 (2, 2, 1)
		};

		top3 = new Vector3[] {
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 6), 
			new Vector3 (3, 1, 6),
			new Vector3 (3, 1, 0)
		};

		front1 = new Vector3[] {
			new Vector3 (3, 1, 0),
			new Vector3 (3, 1, 6), 
			new Vector3 (3, 0, 6),
			new Vector3 (3, 0, 0)
		};

		front2 = new Vector3[] {
			new Vector3 (2, 1, 0),
			new Vector3 (2, 2, 1), 
			new Vector3 (2, 2, 2),
			new Vector3 (2, 1, 4)
		};

		front3 = new Vector3[] {
			new Vector3 (1, 1, 1),
			new Vector3 (1, 3, 3), 
			new Vector3 (1, 3, 4),
			new Vector3 (1, 1, 5)
		};

		back1 = new Vector3[] {
			new Vector3 (0, 2, 2),
			new Vector3 (0, 1, 5), 
			new Vector3 (0, 3, 4),
			new Vector3 (0, 3, 3)
		};

		back2 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (0, 0, 6), 
			new Vector3 (0, 1, 6),
			new Vector3 (0, 1, 0)
		};

		back3 = new Vector3[] {
			new Vector3 (1, 1, 0),
			new Vector3 (1, 1, 4),
			new Vector3 (1, 2, 2),
			new Vector3 (1, 2, 1)
		};

		right1 = new Vector3[] {
			new Vector3 (3, 1, 6),
			new Vector3 (0, 1, 6), 
			new Vector3 (0, 0, 6),
			new Vector3 (3, 0, 6)
		};

		left1 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (0, 1, 0), 
			new Vector3 (3, 1, 0),
			new Vector3 (3, 0, 0)
		};

		left2 = new Vector3[] {
			new Vector3 (0, 1, 1),
			new Vector3 (1, 2, 1), 
			new Vector3 (2, 2, 1),
			new Vector3 (3, 1, 1)
		};

		righttop1 = new Vector3[] {
			new Vector3 (0, 3, 4),
			new Vector3 (0, 1, 5), 
			new Vector3 (1, 1, 5),
			new Vector3 (1, 3, 4)
		};

		righttop2 = new Vector3[] {
			new Vector3 (1, 2, 2),
			new Vector3 (1, 1, 4), 
			new Vector3 (2, 1, 4),
			new Vector3 (2, 2, 2)
		};

		lefttop1 = new Vector3[] {
			new Vector3 (0, 3, 3),
			new Vector3 (1, 3, 3), 
			new Vector3 (1, 2, 2),
			new Vector3 (0, 2, 2)
		};

		lefttop2 = new Vector3[] {
			new Vector3 (1, 1, 0),
			new Vector3 (1, 2, 1), 
			new Vector3 (2, 2, 1),
			new Vector3 (2, 1, 0)
		};

		fronttop1 = new Vector3[] {
			new Vector3 (2, 2, 1),
			new Vector3 (2, 2, 2), 
			new Vector3 (3, 1, 2),
			new Vector3 (3, 1, 1)
		};

		backtop1 = new Vector3[] {
			new Vector3 (1, 2, 1),
			new Vector3 (0, 1, 1), 
			new Vector3 (0, 1, 2),
			new Vector3 (1, 2, 2)
		};

		right2 = new Vector3[] {
			new Vector3 (2, 2, 2),
			new Vector3 (2, 1, 2), 
			new Vector3 (3, 1, 2)
		};

		left3 = new Vector3[] {
			new Vector3 (0, 2, 2),
			new Vector3 (1, 2, 2), 
			new Vector3 (1, 1, 2),
			new Vector3 (0, 1, 2)
		};

		bottom1 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (3, 0, 0), 
			new Vector3 (3, 0, 6),
			new Vector3 (0, 0, 6)
		};

		back4 = new Vector3[] {
			new Vector3 (0, 2, 2),
			new Vector3 (0, 1, 2), 
			new Vector3 (0, 1, 6)
		};


		// define edgetrack for line rendering
		edgeTrack = new Vector3[] {	
			new Vector3 (0, 3, 3), // top1
			new Vector3 (0, 3, 4),
			new Vector3 (1, 3, 4),
			new Vector3 (1, 3, 3),
			new Vector3 (0, 3, 3),
			new Vector3 (0, 2, 2), // topleft1
			new Vector3 (1, 2, 2),
			new Vector3 (1, 3, 3),
			new Vector3 (1, 3, 4), // front 3
			new Vector3 (1, 1, 5), // righttop1
			new Vector3 (0, 1, 5),
			new Vector3 (0, 3, 4),
			new Vector3 (0, 1, 5),
			new Vector3 (0, 1, 6), // top3
			new Vector3 (3, 1, 6),
			new Vector3 (3, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 2), // back1
			new Vector3 (0, 2, 2),
			new Vector3 (0, 1.01f, 1.99f),
			new Vector3 (1, 2.01f, 1.99f), // backtop1
			new Vector3 (1, 2, 1),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 2, 1),
			new Vector3 (0.99f, 1.01f, 1),
			new Vector3 (0.99f, 1.01f, 0), // back2
			new Vector3 (1, 2, 1),
			new Vector3 (1, 1.01f, 0.99f), // left2
			new Vector3 (0, 1.01f, 0.99f),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 0, 0),
			new Vector3 (3, 0, 0),
			new Vector3 (3, 1, 0),
			new Vector3 (3, 0, 0),
			new Vector3 (3, 0, 6),
			new Vector3 (3, 1, 6),
			new Vector3 (3, 0, 6),
			new Vector3 (0, 0, 6),
			new Vector3 (0, 1, 6),
			new Vector3 (0, 0, 6),
			new Vector3 (0, 0, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (2, 1, 0),
			new Vector3 (2, 2, 1),
			new Vector3 (2, 1, 1),
			new Vector3 (2.01f, 1.01f, 0),
			new Vector3 (2.01f, 1.01f, 0.99f),
			new Vector3 (3.01f, 1.01f, 0.99f),
			new Vector3 (2, 2, 1),
			new Vector3 (2, 2, 2),
			new Vector3 (3, 1.01f, 2),
			new Vector3 (2, 1, 2),
			new Vector3 (2, 2, 2),
			new Vector3 (2, 1, 4),
			new Vector3 (2.01f, 1.01f, 2),
			new Vector3 (2.01f, 1.01f, 4),
			new Vector3 (1.01f, 1.01f, 4),
			new Vector3 (1.01f, 2.01f, 2),
			new Vector3 (1, 2, 1),
			new Vector3 (2, 2, 1),
			new Vector3 (2, 2, 2),
			new Vector3 (1.01f, 2.01f, 2),
			new Vector3 (1.01f, 1.01f, 4),
			new Vector3 (1, 1, 5),

		};

		// get line length
		lineLength = edgeTrack.Length;
		lineRenderer.SetVertexCount(lineLength);

		InitializeLog ();
	}

	public void InitializeLog(){
		logObject.GetComponent<PlaneExplorationLog> ().RecordInitialization (trialNum, 8);
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
			front1[0], front1[1], front1[3],
			front1[3], front1[1], front1[2],
			front2[0], front2[1], front2[3],
			front2[3], front2[1], front2[2], // 10
			front3[0], front3[1], front3[3],
			front3[3], front3[1], front3[2],
			back1[0], back1[1], back1[3],
			back1[3], back1[1], back1[2],
			back2[0], back2[1], back2[3], // 15
			back2[3], back2[1], back2[2],
			back3[0], back3[1], back3[3], 
			back3[3], back3[1], back3[2],
			right1[0], right1[1], right1[3],
			right1[3], right1[1], right1[2], // 20
			left1[0], left1[1], left1[3],
			left1[3], left1[1], left1[2],
			left2[0], left2[1], left2[3],
			left2[3], left2[1], left2[2],
			righttop1[0], righttop1[1], righttop1[3], // 25
			righttop1[3], righttop1[1], righttop1[2],
			righttop2[0], righttop2[1], righttop2[3],
			righttop2[3], righttop2[1], righttop2[2],
			lefttop1[0], lefttop1[1], lefttop1[3],
			lefttop1[3], lefttop1[1], lefttop1[2], // 30
			lefttop2[0], lefttop2[1], lefttop2[3],
			lefttop2[3], lefttop2[1], lefttop2[2],
			fronttop1[0], fronttop1[1], fronttop1[3],
			fronttop1[3], fronttop1[1], fronttop1[2],
			backtop1[0], backtop1[1], backtop1[3], // 35
			backtop1[3], backtop1[1], backtop1[2],
			right2[0], right2[1], right2[2],
			left3[0], left3[1], left3[3],
			left3[3], left3[1], left3[2],
			bottom1[0], bottom1[1], bottom1[3], // 40
			bottom1[3], bottom1[1], bottom1[2], // 41
			back4[0], back4[1], back4[2],
			
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
			105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, // 40
			120, 121, 122, 123, 124, 125
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
