using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Model5 : MonoBehaviour {

	private GameObject ModelGameObject;

	private Vector3[] curve1, top1, top2, bottom1, right1, left1, front1, back1, back2;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private MeshCollider meshCollider;

	private Vector3[] edgeTrack;

	private LineRenderer lineRenderer;
	private int lineLength;

	private static int curveSegNum = 12;

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
			new Vector3 (0, 2, 0),
			new Vector3 (0, 2, 1), 
			new Vector3 (1, 2, 1),
			new Vector3 (1, 2, 0)
		};

		top2 = new Vector3[] {
			new Vector3 (0, 2, 3),
			new Vector3 (0, 2, 4), 
			new Vector3 (1, 2, 4),
			new Vector3 (1, 2, 3)
		};

		bottom1 = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (3, 0, 0), 
			new Vector3 (3, 0, 4),
			new Vector3 (0, 0, 4)
		};

		front1 = new Vector3[] {
			new Vector3 (1, 2, 0),
			new Vector3 (1, 2, 4), 
			new Vector3 (3, 0, 4),
			new Vector3 (3, 0, 0)
		};

		back2 = new Vector3[] {
			new Vector3 (1, 2, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 0, 4),
			new Vector3 (1, 2, 4)
		};

		right1 = new Vector3[] {
			new Vector3 (0, 2, 4),
			new Vector3 (0, 0, 4), 
			new Vector3 (3, 0, 4),
			new Vector3 (1, 2, 4)
		};

		left1 = new Vector3[] {
			new Vector3 (0, 2, 0),
			new Vector3 (1, 2, 0), 
			new Vector3 (3, 0, 0),
			new Vector3 (0, 0, 0)
		};

		Vector3 pStart = new Vector3 (0, 2, 2);
		Vector3 pEnd = new Vector3 (1, 2, 2);
		float r = 1;
		float startDegree = 180;
		float endDegree = 360;
		curve1 = GetCurve (pStart, pEnd, startDegree, endDegree, r, curveSegNum);
		back1 = GetBack (curveSegNum);


		// define edgetrack for line rendering

		Vector3[] backCurve = GetBackCurve (curveSegNum);
		Vector3[] frontCurve = GetFrontCurve (curveSegNum);

		edgeTrack = new Vector3[] {
			new Vector3 (0, 2, 0), // top1
			new Vector3 (1, 2, 0),
			new Vector3 (1, 2, 1),
			new Vector3 (0, 2, 1),
			new Vector3 (0, 2, 0),
			new Vector3 (0, 0, 0), // left1
			new Vector3 (3, 0, 0),
			new Vector3 (1, 2, 0),
			new Vector3 (1, 2, 4), // front1
			new Vector3 (3, 0, 4),
			new Vector3 (3, 0, 0),
			new Vector3 (3, 0, 4), // right1
			new Vector3 (0, 0, 4),
			new Vector3 (0, 2, 4),
			new Vector3 (1, 2, 4),
			new Vector3 (0, 2, 4), // back1
			new Vector3 (0, 0, 4),
			new Vector3 (0, 0, 0),
			new Vector3 (0, 2, 0),
			new Vector3 (0, 2, 1),
			backCurve[0], backCurve[1], backCurve[2], // backcurve
			backCurve[3], backCurve[4], backCurve[5], 
			backCurve[6], backCurve[7], backCurve[8], 
			backCurve[9], backCurve[10], backCurve[11], 
			new Vector3 (0, 2, 4), // top2
			new Vector3 (0, 2, 3),
			new Vector3 (1, 2, 3), 
			frontCurve[0], frontCurve[1], frontCurve[2], // frontcurve
			frontCurve[3], frontCurve[4], frontCurve[5], 
			frontCurve[6], frontCurve[7], frontCurve[8], 
			frontCurve[9], frontCurve[10], frontCurve[11], 

		};

		// get line length
		lineLength = edgeTrack.Length;
		lineRenderer.SetVertexCount(lineLength);

		InitializeLog ();
	}

	public void InitializeLog(){
		logObject.GetComponent<PlaneExplorationLog> ().RecordInitialization (trialNum, 5);
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
			bottom1[0], bottom1[1], bottom1[3], // 5
			bottom1[3], bottom1[1], bottom1[2],
			front1[0], front1[1], front1[3],
			front1[3], front1[1], front1[2],
			back2[0], back2[1], back2[3],
			back2[3], back2[1], back2[2], // 10
			right1[0], right1[1], right1[3],
			right1[3], right1[1], right1[2],
			left1[0], left1[1], left1[3],
			left1[3], left1[1], left1[2], // 14
			curve1[0],curve1[1],curve1[2],
			curve1[3],curve1[4],curve1[5],
			curve1[6],curve1[7],curve1[8],
			curve1[9],curve1[10],curve1[11],
			curve1[12],curve1[13],curve1[14],
			curve1[15],curve1[16],curve1[17],
			curve1[18],curve1[19],curve1[20],
			curve1[21],curve1[22],curve1[23],
			curve1[24],curve1[25],curve1[26],
			curve1[27],curve1[28],curve1[29],
			curve1[30],curve1[31],curve1[32],
			curve1[33],curve1[34],curve1[35],
			curve1[36],curve1[37],curve1[38],
			curve1[39],curve1[40],curve1[41],
			curve1[42],curve1[43],curve1[44],
			curve1[45],curve1[46],curve1[47],
			curve1[48],curve1[49],curve1[50],
			curve1[51],curve1[52],curve1[53],
			curve1[54],curve1[55],curve1[56],
			curve1[57],curve1[58],curve1[59],
			curve1[60],curve1[61],curve1[62],
			curve1[63],curve1[64],curve1[65], // 36
			back1[0],back1[1],back1[2],
			back1[3],back1[4],back1[5],
			back1[6],back1[7],back1[8],
			back1[9],back1[10],back1[11],
			back1[12],back1[13],back1[14],
			back1[15],back1[16],back1[17],
			back1[18],back1[19],back1[20],
			back1[21],back1[22],back1[23],
			back1[24],back1[25],back1[26],
			back1[27],back1[28],back1[29],
			back1[30],back1[31],back1[32],
			back1[33],back1[34],back1[35],
			back1[36],back1[37],back1[38],
			back1[39],back1[40],back1[41],
			back1[42],back1[43],back1[44],
			back1[45],back1[46],back1[47],
			back1[48],back1[49],back1[50],
			back1[51],back1[52],back1[53],
			back1[54],back1[55],back1[56],
			back1[57],back1[58],back1[59],
			back1[60],back1[61],back1[62],
			back1[63],back1[64],back1[65],
			back1[66],back1[67],back1[68],
			back1[69],back1[70],back1[71],
			back1[72],back1[73],back1[74],
			back1[75],back1[76],back1[77], // 62
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
			120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, // 45
			135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, // 50
			150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, // 55
			165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, // 60
			180, 181, 182, 183, 184, 185, // 62
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

	Vector3[] GetBack(int segNum) {
		
		Vector3[] backArray = new Vector3[(segNum - 1) * 6 + 12];
		Vector3 s0, s1;
		float offsetZ, offsetY;

		backArray [0] = new Vector3 (0, 0, 0);
		backArray [1] = new Vector3 (0, 2, 1);
		backArray [2] = new Vector3 (0, 2, 0);
		backArray [3] = new Vector3 (0, 0, 0);
		backArray [4] = new Vector3 (0, 0, 1);
		backArray [5] = new Vector3 (0, 2, 1);
		backArray [6] = new Vector3 (0, 2, 3);
		backArray [7] = new Vector3 (0, 0, 3);
		backArray [8] = new Vector3 (0, 0, 4);
		backArray [9] = new Vector3 (0, 2, 3);
		backArray [10] = new Vector3 (0, 0, 4);
		backArray [11] = new Vector3 (0, 2, 4);

		float r = 1, dStart = 180, dEnd = 360;
		Vector3 p = new Vector3 (0, 2, 2);

		s0 = new Vector3 (0, 2, 1);

		for (int i = 1; i < segNum; i++) {

			offsetZ = r * Mathf.Cos ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);
			offsetY = r * Mathf.Sin ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);

			s1 = new Vector3 (p [0], p [1] + offsetY, p [2] + offsetZ);

			backArray [6 * i + 6] = s0;
			backArray [6 * i + 7] = new Vector3 (s0 [0], 0, s0 [2]);
			backArray [6 * i + 8] = s1;
			backArray [6 * i + 9] = s1;
			backArray [6 * i + 10] = new Vector3 (s0 [0], 0, s0 [2]);
			backArray [6 * i + 11] = new Vector3 (s1 [0], 0, s1 [2]);

			s0 = s1;

		}

		return backArray;
	}


	Vector3[] GetBackCurve(int segNum) {
		
		Vector3[] backCurve = new Vector3[segNum];

		float dStart = 180, dEnd = 360;
		float rX = 0, rY = 2, rZ = 2;
		float r = 1;
		float offsetY, offsetZ;

		for (int i = 0; i < segNum; i++) {

			offsetZ = r * Mathf.Cos ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);
			offsetY = r * Mathf.Sin ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);

			backCurve [i] = new Vector3 (rX, rY + offsetY, rZ + offsetZ);
		}

		return backCurve;

	}


	Vector3[] GetFrontCurve(int segNum) {
		
		Vector3[] frontCurve = new Vector3[segNum];

		float dStart = 180, dEnd = 360;
		float rX = 0.99f, rY = 2.01f, rZ = 2;
		float r = 1;
		float offsetY, offsetZ;

		for (int i = 0; i < segNum; i++) {

			offsetZ = r * Mathf.Cos ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);
			offsetY = r * Mathf.Sin ((dStart + (dEnd - dStart) * i / (segNum - 1)) * Mathf.PI / 180);

			frontCurve [i] = new Vector3 (rX, rY + offsetY, rZ + offsetZ);
		}

		return frontCurve;
	}


}
