using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model1 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] top1, top2, bottom1, left1, right1, right2, front1, front2, back1, back2;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	private Vector3[] edgeTrack;

	private LineRenderer lineRenderer;
	private int lineLength;

	public GameObject logObject;
	public int trialNum;

	// define points
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
			new Vector3(0, 2, 0),
			new Vector3(0, 2, 1), 
			new Vector3(3, 2, 1),
			new Vector3(3, 2, 0)};

		top2 = new Vector3[] {
			new Vector3(1, 1, 2),
			new Vector3(1, 1, 3), 
			new Vector3(2, 1, 3),
			new Vector3(2, 1, 2)};

		bottom1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(3, 0, 0), 
			new Vector3(3, 0, 3),
			new Vector3(0, 0, 3)};

		left1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(3, 2, 0), 
			new Vector3(3, 0, 0),
			new Vector3(0, 0, 0)};

		right1 = new Vector3[] {
			new Vector3(1, 1, 3),
			new Vector3(1, 0, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 3)};

		right2 = new Vector3[] {
			new Vector3(0, 2, 1),
			new Vector3(0, 0, 3),
			new Vector3(3, 0, 3),
			new Vector3(3, 2, 1)};

		front1 = new Vector3[] {
			new Vector3(3, 2, 0),
			new Vector3(3, 2, 1), 
			new Vector3(3, 0, 3),
			new Vector3(3, 0, 0)};

		front2 = new Vector3[] {
			new Vector3(2, 1, 2),
			new Vector3(2, 1, 3), 
			new Vector3(2, 0, 3)};

		back1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 0), 
			new Vector3(0, 0, 3),
			new Vector3(0, 2, 1)};

		back2 = new Vector3[] {
			new Vector3(1, 1, 2), 
			new Vector3(1, 0, 3),
			new Vector3(1, 1, 3)};

		// define edgetrack for line rendering
		edgeTrack = new Vector3[] {	
			new Vector3(0, 2, 0), // top1
			new Vector3(3, 2, 0),
			new Vector3(3, 2, 1),
			new Vector3(0, 2, 1),
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 0), // back1
			new Vector3(0, 0, 3),
			new Vector3(0, 2, 1),
			new Vector3(3, 2, 1), // right2
			new Vector3(3, 0, 3),
			new Vector3(0, 0, 3),
			new Vector3(1, 0, 3), // back2
			new Vector3(1, 1, 3),
			new Vector3(1, 1, 2),
			new Vector3(1, 1, 3), // right1
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3), // front2
			new Vector3(2, 1, 2),
			new Vector3(1, 1, 2),
			new Vector3(2, 1, 2),
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3),
			new Vector3(3, 0, 3), // front1
			new Vector3(3, 0, 0),
			new Vector3(3, 2, 0),
			new Vector3(3, 0, 0), // left1
			new Vector3(0, 0, 0)
		};

		// get line length
		lineLength = edgeTrack.Length;
		lineRenderer.SetVertexCount(lineLength);

		InitializeLog ();

	}


	public void InitializeLog(){
		logObject.GetComponent<PlaneExplorationLog> ().RecordInitialization (trialNum, 1);
		trialNum++;
	}

	// render
	void Update () {

		mesh.vertices = new Vector3[] {
			top1 [0], top1 [1], top1 [3],
			top1 [3], top1 [1], top1 [2],
			top2 [0], top2 [1], top2 [3],
			top2 [3], top2 [1], top2 [2],
			bottom1 [0], bottom1 [1], bottom1 [3], // 5
			bottom1 [3], bottom1 [1], bottom1 [2],
			left1 [0], left1 [1], left1 [3],
			left1 [3], left1 [1], left1 [2],
			right1 [0], right1 [1], right1 [3],
			right1 [3], right1 [1], right1 [2], // 10
			right2 [0], right2 [1], right2 [3],
			right2 [3], right2 [1], right2 [2],
			front1 [0], front1 [1], front1 [3],
			front1 [3], front1 [1], front1 [2],
			front2 [0], front2 [1], front2 [2], // 15
			back1 [0], back1 [1], back1 [3],
			back1 [3], back1 [1], back1 [2],
			back2 [0], back2 [1], back2 [2] // 18
		};

		// set triangle index
		mesh.triangles = new int[] {
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, //5
			15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, //10
			30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, //15
			45, 46, 47, 48, 49, 50, 51, 52, 53, //18
		};

		// set collider
		meshCollider.sharedMesh = mesh;

		/* render line */
		for (int i = 0; i < lineLength; i++)
			lineRenderer.SetPosition (i, edgeTrack [i]);

		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}
}
