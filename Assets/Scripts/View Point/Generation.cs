using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MiniJSON;
using System.Runtime.Serialization.Formatters.Binary;


public class Generation : MonoBehaviour {

	public GameObject cam;

	public GameObject solid0;
	public GameObject solid1;
	public GameObject solid2;
	public GameObject solid3;
	public GameObject solid4;
	public GameObject solid5;
	public GameObject solid6;
	public GameObject solid7;
	public GameObject solid8;
	public GameObject solid9;
	public GameObject solid10;

	private GameObject[] solids;

	private float[] solidsH;

	public int totNum;
	private int solidNum;

	private int iDown, iUp, dDown, dUp, rDown, rUp, sDown, sUp, sRatio, posDegree;
	private float posRadius, scale, rotY, posX, posY, posZ, mScale, x0, x1, z0, z1;

	private List<Vector4> collisionBox;
	private Vector4 box;

	private int id;
	private string jsonFilePath = "Assets/Scripts/Json/Puzzles.json";


	void Start () {

		id = DataUtil.GetCurrentRoomId();
		ParseJson(jsonFilePath, id);

		GameObject container = GameObject.Find("Solids");

		solids = new GameObject[] {solid0, solid1, solid2, solid3, 
			solid4, solid5, solid6, solid7, solid8, solid9, solid10
		};

		solidsH = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.35f, 0.4f };

		iDown = 0;
		iUp = 11;
		dDown = 0;
		dUp = 360;
		rDown = 0;
		rUp = 7;
		sDown = 25 / (totNum + 2);
		sUp = 44 / (totNum + 3);
		sRatio = 3;
		mScale = 1.5f;

		collisionBox = new List<Vector4> ();

		if (totNum > 8)
			totNum = 8;
		if (totNum < 4)
			totNum = 4;

		int dir = cam.GetComponent<ViewPointCameraController> ().camDir;

		for (int i = 0; i < 3; i++) {

			float x0, x1, z0, z1;

			if (dir == 0) {
				x0 = (rUp - 0.5f) * Mathf.Cos (Mathf.PI * (i + 0.25f) / 4.0f);
				z0 = (rUp - 0.5f) * Mathf.Sin (Mathf.PI * (i + 0.25f) / 4.0f);
				x1 = (rUp - 0.5f) * Mathf.Cos (Mathf.PI * (i + 1.25f) / 4.0f);
				z1 = (rUp - 0.5f) * Mathf.Sin (Mathf.PI * (i + 1.25f) / 4.0f);
			} else {
				x0 = (rUp - 0.5f) * Mathf.Cos (Mathf.PI * (8 - dir + i + 0.25f) / 4.0f);
				z0 = (rUp - 0.5f) * Mathf.Sin (Mathf.PI * (8 - dir + i + 0.25f) / 4.0f);
				x1 = (rUp - 0.5f) * Mathf.Cos (Mathf.PI * (8 - dir + i + 1.25f) / 4.0f);
				z1 = (rUp - 0.5f) * Mathf.Sin (Mathf.PI * (8 - dir + i + 1.25f) / 4.0f);
			}

			float tmp;

			if (x0 > x1) {
				tmp = x1;
				x1 = x0;
				x0 = tmp;
			}

			if (z0 > z1) {
				tmp = z1;
				z1 = z0;
				z0 = tmp;
			}


			Vector4 cullBox = new Vector4 (x0, z0, x1 + 1, z1 + 1);
//			print ("x0:" + x0.ToString () + " z0:" + z0.ToString () + " x1:" + x1.ToString () + " z1:" + z1.ToString ());

			collisionBox.Add (cullBox);

		}


		for (int i = 0; i < totNum; i++) {
			
			// get random no.
			solidNum = Random.Range (iDown, iUp);

			// polar coordinates
			posDegree = Random.Range (dDown, dUp);
			posRadius = Random.Range (rDown, rUp);
			posZ = posRadius * Mathf.Cos (posDegree / 180.0f * Mathf.PI);
			posX = posRadius * Mathf.Sin (posDegree / 180.0f * Mathf.PI);

			// scale
			scale = Random.Range (sDown, sUp) * 1.0f / sRatio;

			// height
			posY = solidsH[solidNum] * scale;

			// rotation
			rotY = Random.Range (dDown, dUp);

			x0 = posX - mScale * scale;
			z0 = posZ - mScale * scale;
			x1 = posX + mScale * scale;
			z1 = posZ + mScale * scale;
			box = new Vector4 (x0, z0, x1, z1);
//			print ("x0:" + x0.ToString () + " z0:" + z0.ToString () + " x1:" + x1.ToString () + " z1:" + z1.ToString ());

			if (!CheckCollision (box)) {
				
				// render
				GameObject solidInstance = Instantiate (solids [solidNum]);
				solidInstance.transform.parent = container.transform;
				solidInstance.transform.position = new Vector3 (posX, posY, posZ);
				solidInstance.transform.localScale = new Vector3 (scale, scale, scale);
				solidInstance.transform.eulerAngles = new Vector3 (0, rotY, 0);
			
				collisionBox.Add (box);

			} else {
//				totNum++;
				i--;
			}
		}

//		box = new Vector4 (0, 2, 2, 4);
//		collisionBox.Add (box);
//		Vector4 box1 = new Vector4 (-1, -1, 1, 3);
//		print (CheckCollision (box1));

	}


	private void ParseJson(string jsonFilePath, int roomId) {
		string jsonString = File.ReadAllText(jsonFilePath);
		Dictionary<string, object> dict;
		dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
		dict = (Dictionary<string, object>)dict[roomId.ToString()];

		totNum = System.Convert.ToInt32 (dict ["solidNum"]);
		print (totNum);

	}


	bool CheckCollision(Vector4 checkBox) {

		float x0, z0, x1, z1, b_x0, b_z0, b_x1, b_z1;
		x0 = checkBox [0];
		z0 = checkBox [1];
		x1 = checkBox [2];
		z1 = checkBox [3];
		
		foreach (Vector4 box in collisionBox) {

			b_x0 = box [0];
			b_z0 = box [1];
			b_x1 = box [2];
			b_z1 = box [3];

			if (x0 >= b_x0 && x0 <= b_x1 && z0 >= b_z0 && z0 <= b_z1)
				return true;
			if (x1 >= b_x0 && x1 <= b_x1 && z0 >= b_z0 && z0 <= b_z1)
				return true;
			if (x0 >= b_x0 && x0 <= b_x1 && z1 >= b_z0 && z1 <= b_z1)
				return true;
			if (x1 >= b_x0 && x1 <= b_x1 && z1 >= b_z0 && z1 <= b_z1)
				return true;
			if (b_x0 >= x0 && b_x0 <= x1 && b_z0 >= z0 && b_z0 <= z1)
				return true;
			if (b_x1 >= x0 && b_x1 <= x1 && b_z0 >= z0 && b_z0 <= z1)
				return true;
			if (b_x0 >= x0 && b_x0 <= x1 && b_z1 >= z0 && b_z1 <= z1)
				return true;
			if (b_x1 >= x0 && b_x1 <= x1 && b_z1 >= z0 && b_z1 <= z1)
				return true;
		}

		return false;
	}

}
