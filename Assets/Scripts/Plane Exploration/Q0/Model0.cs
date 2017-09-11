using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model0 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] m1top1, m1bottom1, m1right1, m1left1, m1front1, m1back1;
	private Vector3[] m2top1, m2top2, m2bottom1, m2right1, m2left1, m2front1, m2back1, m2righttop1;
	private Vector3[] m3top1, m3top2, m3bottom1, m3right1, m3left1, m3front1, m3front2, m3back1, m3righttop1;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	private Vector3[] edgeTrack1, edgeTrack2, edgeTrack3;

	private LineRenderer lineRenderer;
	private int lineLength1, lineLength2, lineLength3;

	public GameObject deadTrigger;

	private int modelStage;
	public GameObject tutObject;

	// define points
	void Start () {

		modelStage = 2;

		// object name is Model
		ModelGameObject = GameObject.Find ("Model");

		// get mesh from mesh filter
		meshFilter = (MeshFilter)ModelGameObject.GetComponent (typeof(MeshFilter));
		mesh = meshFilter.mesh;

		// get line renderer
		lineRenderer = (LineRenderer)ModelGameObject.GetComponent (typeof(LineRenderer));

		// get mesh collider
		meshCollider = (MeshCollider)ModelGameObject.GetComponent (typeof(MeshCollider));


		// tut model1
		m1top1 = new Vector3[] {
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 3), 
			new Vector3(2, 1, 3),
			new Vector3(2, 1, 0)};

		m1bottom1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(2, 0, 0), 
			new Vector3(2, 0, 3),
			new Vector3(0, 0, 3)};

		m1right1 = new Vector3[] {
			new Vector3(2, 1, 3),
			new Vector3(0, 1, 3), 
			new Vector3(0, 0, 3),
			new Vector3(2, 0, 3)};

		m1left1 = new Vector3[] {
			new Vector3(0, 1, 0),
			new Vector3(2, 1, 0), 
			new Vector3(2, 0, 0),
			new Vector3(0, 0, 0)};

		m1front1 = new Vector3[] {
			new Vector3(2, 0, 0),
			new Vector3(2, 1, 0), 
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3)};

		m1back1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 3), 
			new Vector3(0, 1, 3),
			new Vector3(0, 1, 0)};

		edgeTrack1 = new Vector3[] {	
			new Vector3(0, 1, 0), // top1
			new Vector3(0, 1, 3),
			new Vector3(2, 1, 3),
			new Vector3(2, 1, 0),
			new Vector3(0, 1, 0),
			new Vector3(0, 0, 0),
			new Vector3(2, 0, 0),
			new Vector3(2, 1, 0),
			new Vector3(2, 0, 0),
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3),
			new Vector3(0, 0, 3),
			new Vector3(0, 1, 3),
			new Vector3(0, 0, 3),
			new Vector3(0, 0, 0),
		};

		lineLength1 = edgeTrack1.Length;

		// tut model2
		m2top1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(0, 2, 1), 
			new Vector3(2, 2, 1),
			new Vector3(2, 2, 0)};
		
		m2top2 = new Vector3[] {
			new Vector3(0, 1, 2),
			new Vector3(0, 1, 3), 
			new Vector3(2, 1, 3),
			new Vector3(2, 1, 2)};

		m2front1 = new Vector3[] {
			new Vector3(2, 2, 0),
			new Vector3(2, 2, 1), 
			new Vector3(2, 0, 0),
			new Vector3(2, 0, 0),
			new Vector3(2, 2, 1),
			new Vector3(2, 0, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 2),
			new Vector3(2, 1, 3)};

		m2right1 = new Vector3[] {
			new Vector3(2, 1, 3),
			new Vector3(0, 1, 3), 
			new Vector3(0, 0, 3),
			new Vector3(2, 0, 3)};

		m2left1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(2, 2, 0), 
			new Vector3(2, 0, 0),
			new Vector3(0, 0, 0)};

		m2back1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 3), 
			new Vector3(0, 2, 0),
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 3),
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2),
			new Vector3(0, 0, 3),
			new Vector3(0, 1, 3),};

		m2bottom1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(2, 0, 0), 
			new Vector3(2, 0, 3),
			new Vector3(0, 0, 3),};

		m2righttop1 = new Vector3[] {
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2), 
			new Vector3(2, 1, 2),
			new Vector3(2, 2, 1)};
		
		edgeTrack2 = new Vector3[] {	
			new Vector3(0, 2, 0), // top1
			new Vector3(0, 2, 1),
			new Vector3(2, 2, 1),
			new Vector3(2, 2, 0),
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 0),
			new Vector3(2, 0, 0),
			new Vector3(2, 2, 0),
			new Vector3(2, 2, 1),
			new Vector3(2, 1, 2),
			new Vector3(0, 1, 2),
			new Vector3(2, 1, 2),
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 0, 0),
			new Vector3(2, 0, 3),
			new Vector3(0, 0, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 3),
			new Vector3(0, 1, 3),
			new Vector3(0, 0, 3),
			new Vector3(0, 1, 3),
			new Vector3(0, 1, 2),
			new Vector3(0, 2, 1),
		};

		lineLength2 = edgeTrack2.Length;


		// tut model3
		m3top1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(0, 2, 1), 
			new Vector3(1, 2, 1),
			new Vector3(1, 2, 0)};
		
		m3top2 = new Vector3[] {
			new Vector3(0, 1, 0),
			new Vector3(0, 1, 3), 
			new Vector3(2, 1, 3),
			new Vector3(2, 1, 0)};

		m3front1 = new Vector3[] {
			new Vector3(1, 2, 0),
			new Vector3(1, 2, 1), 
			new Vector3(1, 1, 2),
			new Vector3(1, 1, 0)};

		m3front2 = new Vector3[] {
			new Vector3(2, 1, 0),
			new Vector3(2, 1, 3), 
			new Vector3(2, 0, 3),
			new Vector3(2, 0, 0)};

		m3right1 = new Vector3[] {
			new Vector3(2, 1, 3),
			new Vector3(0, 1, 3), 
			new Vector3(0, 0, 3),
			new Vector3(2, 0, 3)};

		m3left1 = new Vector3[] {
			new Vector3(0, 2, 0),
			new Vector3(1, 2, 0), 
			new Vector3(1, 1, 0),
			new Vector3(1, 1, 0),
			new Vector3(2, 1, 0),
			new Vector3(2, 0, 0),
			new Vector3(0, 2, 0),
			new Vector3(2, 0, 0),
			new Vector3(0, 0, 0)};

		m3back1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 3), 
			new Vector3(0, 2, 0),
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 3),
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2),
			new Vector3(0, 0, 3),
			new Vector3(0, 1, 3),};

		m3bottom1 = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(2, 0, 0), 
			new Vector3(2, 0, 3),
			new Vector3(0, 0, 3),};

		m3righttop1 = new Vector3[] {
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2), 
			new Vector3(1, 1, 2),
			new Vector3(1, 2, 1)};

		// define edgetrack for line rendering
		edgeTrack3 = new Vector3[] {	
			new Vector3(0, 2, 0), // top1
			new Vector3(0, 2, 1),
			new Vector3(1, 2, 1),
			new Vector3(1, 2, 0),
			new Vector3(0, 2, 0),
			new Vector3(0, 0, 0), // left1
			new Vector3(2, 0, 0),
			new Vector3(2, 1, 0),
			new Vector3(1, 1, 0),
			new Vector3(1, 2, 0),
			new Vector3(1.01f, 1.01f, 0),
			new Vector3(1.01f, 1.01f, 2),
			new Vector3(1, 2, 1),
			new Vector3(0, 2, 1),
			new Vector3(0, 1, 2),
			new Vector3(1, 1, 2),
			new Vector3(0, 1, 2),
			new Vector3(0, 1, 3),
			new Vector3(0, 0, 3),
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 0, 0),
			new Vector3(2, 1, 0),
			new Vector3(2, 1, 3),
			new Vector3(2, 0, 3),
			new Vector3(2, 1, 3),
			new Vector3(0, 1, 3),

		};

		// get line length
		lineLength3 = edgeTrack3.Length;
		

	}
	

	// render
	void Update () {

		modelStage = tutObject.GetComponent<TutorialStage>().modelStage;

		if(modelStage == 1){
			mesh.vertices = new Vector3[] {
				m1top1 [0], m1top1 [1], m1top1 [3],
				m1top1 [3], m1top1 [1], m1top1 [2],
				m1front1 [0], m1front1 [1], m1front1 [3],
				m1front1 [3], m1front1 [1], m1front1 [2],
				m1right1 [0], m1right1 [1], m1right1 [3],
				m1right1 [3], m1right1 [1], m1right1 [2],
				m1left1 [0], m1left1 [1], m1left1 [3],
				m1left1 [3], m1left1 [1], m1left1 [2],
				m1back1 [0], m1back1 [1], m1back1 [3],
				m1back1 [3], m1back1 [1], m1back1 [2],
				m1bottom1 [0], m1bottom1 [1], m1bottom1 [3],
				m1bottom1 [3], m1bottom1 [1], m1bottom1 [2],
			};

			// set triangle index
			mesh.triangles = new int[] {
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, //5
				15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, //10
				30, 31, 32, 33, 34, 35,
			};

			// render line
			lineRenderer.SetVertexCount(lineLength1);

			for (int i = 0; i < lineLength1; i++)
				lineRenderer.SetPosition (i, edgeTrack1 [i]);

		}else if(modelStage == 2 || modelStage == 3){
			mesh.vertices = new Vector3[] {
				m2top1 [0], m2top1 [1], m2top1 [3],
				m2top1 [3], m2top1 [1], m2top1 [2],
				m2top2 [0], m2top2 [1], m2top2 [3],
				m2top2 [3], m2top2 [1], m2top2 [2],
				m2front1 [0], m2front1 [1], m2front1 [2], // 5
				m2front1 [3], m2front1 [4], m2front1 [5],
				m2front1 [6], m2front1 [7], m2front1 [8],
				m2right1 [0], m2right1 [1], m2right1 [3],
				m2right1 [3], m2right1 [1], m2right1 [2],
				m2left1 [0], m2left1 [1], m2left1 [3], // 10
				m2left1 [3], m2left1 [1], m2left1 [2],
				m2back1 [0], m2back1 [1], m2back1 [2],
				m2back1 [3], m2back1 [4], m2back1 [5],
				m2back1 [6], m2back1 [7], m2back1 [8], 
				m2bottom1 [0], m2bottom1 [1], m2bottom1 [3], // 15
				m2bottom1 [3], m2bottom1 [1], m2bottom1 [2],
				m2righttop1 [0], m2righttop1 [1], m2righttop1 [3],
				m2righttop1 [3], m2righttop1 [1], m2righttop1 [2], // 18
			};

			// set triangle index
			mesh.triangles = new int[] {
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, //5
				15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, //10
				30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, //15
				45, 46, 47, 48, 49, 50, 51, 52, 53,
			};

			// render line
			lineRenderer.SetVertexCount(lineLength2);

			for (int i = 0; i < lineLength2; i++)
				lineRenderer.SetPosition (i, edgeTrack2 [i]);

		}else{ // 3

			deadTrigger.SetActive(true);

			mesh.vertices = new Vector3[] {
				m3top1 [0], m3top1 [1], m3top1 [3],
				m3top1 [3], m3top1 [1], m3top1 [2],
				m3top2 [0], m3top2 [1], m3top2 [3],
				m3top2 [3], m3top2 [1], m3top2 [2],
				m3front1 [0], m3front1 [1], m3front1 [3], // 5
				m3front1 [3], m3front1 [1], m3front1 [2],
				m3front2 [0], m3front2 [1], m3front2 [3],
				m3front2 [3], m3front2 [1], m3front2 [2],
				m3right1 [0], m3right1 [1], m3right1 [3],
				m3right1 [3], m3right1 [1], m3right1 [2], // 10
				m3left1 [0], m3left1 [1], m3left1 [2],
				m3left1 [3], m3left1 [4], m3left1 [5],
				m3left1 [6], m3left1 [7], m3left1 [8],
				m3back1 [0], m3back1 [1], m3back1 [2],
				m3back1 [3], m3back1 [4], m3back1 [5], // 15
				m3back1 [6], m3back1 [7], m3back1 [8],
				m3bottom1 [0], m3bottom1 [1], m3bottom1 [3],
				m3bottom1 [3], m3bottom1 [1], m3bottom1 [2],
				m3righttop1 [0], m3righttop1 [1], m3righttop1 [3],
				m3righttop1 [3], m3righttop1 [1], m3righttop1 [2], // 20
			};

			// set triangle index
			mesh.triangles = new int[] {
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, //5
				15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, //10
				30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, //15
				45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55 ,56, 57, 58, 59
			};

			// render line
			lineRenderer.SetVertexCount(lineLength3);
			for (int i = 0; i < lineLength3; i++)
				lineRenderer.SetPosition (i, edgeTrack3 [i]);
		}

		// set collider
		meshCollider.sharedMesh = mesh;

		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}
}
