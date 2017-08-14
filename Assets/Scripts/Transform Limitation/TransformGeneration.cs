using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGeneration : MonoBehaviour {

	public Dictionary<Vector3, bool> curModel, nextModel;
	public int transNum;
	public GameObject block;
	public GameObject container;
	public GameObject startModel;

	private int[] transformation;

	void OnEnable () {

		transformation = new int[] { 0, 0, 0, 0, 0, 0 };

		curModel = startModel.GetComponent<ModelGeneration> ().model;
		nextModel = new Dictionary<Vector3,bool> ();

		for (int i = 0; i < transNum; i++) {
			int transIndex = Random.Range (0, 6); // 0-2 xyz rot; 3-5 sym

			if ((transIndex <= 2 && transformation [transIndex] <= 3) ||
			    (transIndex >= 3 && transformation [transIndex] < 1
			    && transformation [3] + transformation [4] + transformation [5] < 2)) {
				switch (transIndex) {
				case 0:
					nextModel = RotX (curModel);
					print ("rotX");
					break;
				case 1:
					nextModel = RotY (curModel);
					print ("rotY");
					break;
				case 2:
					nextModel = RotZ (curModel); 
					print ("rotZ");
					break;
				case 3:
					nextModel = SymXY (curModel);
					print ("symXY");
					break;
				case 4:
					nextModel = SymXZ (curModel);
					print ("symXZ");
					break;
				case 5:
					nextModel = SymYZ (curModel);
					print ("symYZ");
					break;
				}
				transformation [transIndex]++;
			}

			curModel = nextModel;
		}
		curModel = AlignModel (curModel);

		RenderTransModel (curModel);
	}


	Dictionary<Vector3, bool> RotX(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++) {
			
			resultModel.Add (new Vector3 (x, 1, 0), curModel [new Vector3 (x, 0, 1)]);
			resultModel.Add (new Vector3 (x, 0, -1), curModel [new Vector3 (x, 1, 0)]);
			resultModel.Add (new Vector3 (x, -1, 0), curModel [new Vector3 (x, 0, -1)]);
			resultModel.Add (new Vector3 (x, 0, 1), curModel [new Vector3 (x, -1, 0)]);
			resultModel.Add (new Vector3 (x, 1, -1), curModel [new Vector3 (x, 1, 1)]);
			resultModel.Add (new Vector3 (x, -1, -1), curModel [new Vector3 (x, 1, -1)]);
			resultModel.Add (new Vector3 (x, -1, 1), curModel [new Vector3 (x, -1, -1)]);
			resultModel.Add (new Vector3 (x, 1, 1), curModel [new Vector3 (x, -1, 1)]);
			resultModel.Add (new Vector3 (x, 0, 0), curModel [new Vector3 (x, 0, 0)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> RotY(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int y = -1; y <= 1; y++) {

			resultModel.Add (new Vector3 (-1, y, 0), curModel [new Vector3 (0, y, 1)]);
			resultModel.Add (new Vector3 (0, y, -1), curModel [new Vector3 (-1, y, 0)]);
			resultModel.Add (new Vector3 (1, y, 0), curModel [new Vector3 (0, y, -1)]);
			resultModel.Add (new Vector3 (0, y, 1), curModel [new Vector3 (1, y, 0)]);
			resultModel.Add (new Vector3 (-1, y, 1), curModel [new Vector3 (1, y, 1)]);
			resultModel.Add (new Vector3 (-1, y, -1), curModel [new Vector3 (-1, y, 1)]);
			resultModel.Add (new Vector3 (1, y, -1), curModel [new Vector3 (-1, y, -1)]);
			resultModel.Add (new Vector3 (1, y, 1), curModel [new Vector3 (1, y, -1)]);
			resultModel.Add (new Vector3 (0, y, 0), curModel [new Vector3 (0, y, 0)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> RotZ(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int z = -1; z <= 1; z++) {

			resultModel.Add (new Vector3 (1, 0, z), curModel [new Vector3 (0, 1, z)]);
			resultModel.Add (new Vector3 (0, -1, z), curModel [new Vector3 (1, 0, z)]);
			resultModel.Add (new Vector3 (-1, 0, z), curModel [new Vector3 (0, -1, z)]);
			resultModel.Add (new Vector3 (0, 1, z), curModel [new Vector3 (-1, 0, z)]);
			resultModel.Add (new Vector3 (1, -1, z), curModel [new Vector3 (1, 1, z)]);
			resultModel.Add (new Vector3 (-1, -1, z), curModel [new Vector3 (1, -1, z)]);
			resultModel.Add (new Vector3 (-1, 1, z), curModel [new Vector3 (-1, -1, z)]);
			resultModel.Add (new Vector3 (1, 1, z), curModel [new Vector3 (-1, 1, z)]);
			resultModel.Add (new Vector3 (0, 0, z), curModel [new Vector3 (0, 0, z)]);

		}

		return resultModel;
	}

	Dictionary<Vector3, bool> SymXY(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (x, y, -z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}

	Dictionary<Vector3, bool> SymXZ(Dictionary<Vector3, bool> curModel) {
		
		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (x, -y, z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}

	Dictionary<Vector3, bool> SymYZ(Dictionary<Vector3, bool> curModel) {

		Dictionary<Vector3, bool> resultModel = new Dictionary<Vector3, bool> ();

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
				for (int z = -1; z <= 1; z++)
					resultModel.Add (new Vector3 (-x, y, z), curModel [new Vector3 (x, y, z)]);

		return resultModel;
	}


	Dictionary<Vector3, bool> AlignModel(Dictionary<Vector3, bool> model) { // align model to prevent wrong judge

		Dictionary<Vector3, bool> alignModel = model;

		float xMin, yMin, zMin;

		xMin = 1.0f;
		yMin = 1.0f;
		zMin = 1.0f;

		foreach (Vector3 point in model.Keys)
		{
			if (model [point]) {
				if (point [0] < xMin)
					xMin = point [0];

				if (point [1] < yMin)
					yMin = point [1];

				if (point [2] < zMin)
					zMin = point [2];
			}
		}

		if (xMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0] - xMin - 1, point [1], point [2])] = true;
			}
			alignModel = tmpModel;

		}

		if (yMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0], point [1] - yMin - 1, point [2])] = true;
			}

			alignModel = tmpModel;

		}

		if (zMin > -1) {

			Dictionary<Vector3, bool> tmpModel = InitializeDic();

			foreach (Vector3 point in alignModel.Keys)
			{
				if (alignModel [point])
					tmpModel [new Vector3 (point [0], point [1], point [2] - zMin - 1)] = true;
			}

			alignModel = tmpModel;

		}

		return alignModel;

	}


	Dictionary<Vector3,bool> InitializeDic() {

		Dictionary<Vector3,bool> model = new Dictionary<Vector3,bool> ();

		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				for (int k = 0; k < 3; k++)
					model.Add (new Vector3 (i - 1, j - 1, k - 1), false);

		return model;
	}


	void RenderTransModel(Dictionary<Vector3, bool> model){

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					if (model [new Vector3 (x, y, z)]) {
						GameObject blockInstance = Instantiate (block);
						blockInstance.transform.parent = container.transform;
						blockInstance.layer = 9;
						blockInstance.transform.position = new Vector3 (x, y, z);
					}
				}
			}
		}

	}

}
