using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotationController : MonoBehaviour {

	public Camera controllerCamera;
	public Material highlightMaterial;
	public Material originMaterial;

	public GameObject xArrow, yArrow, zArrow, xyMirror, xzMirror, yzMirror, undoArrow; 

	private GameObject curObject, lastObject;
	public GameObject lastNotation;

	public GameObject mirrorModel;
	public GameObject block;

	public Dictionary<Vector3, bool> curModel;

	private static int XY = 0, XZ = 1, YZ = 2;

	void Start() {
		lastObject = null;
		lastNotation = null;

	}

	void Update () {

		Ray ray;
		RaycastHit rayhit;
		float fDistance = 20f;

		ray = controllerCamera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out rayhit, fDistance)) {
			
			curObject = rayhit.collider.gameObject;


			if (lastObject)
				lastObject.GetComponent<Renderer> ().material = originMaterial;

			if (lastNotation)
				lastNotation.SetActive (false);
			
			if (curObject.transform.parent.transform.parent &&
			    curObject.transform.parent.transform.parent.name == "Controller") {

				Dictionary<Vector3, bool> nextModel = new Dictionary<Vector3, bool>(); // to show mirror image
					
				curObject.GetComponent<Renderer> ().material = highlightMaterial;

				string operation = curObject.name;
				try
				{
				curModel = GetComponentInParent<Controller> ().curModel;
				}
				catch(Exception e){
				curModel = GetComponentInParent<TutorialController> ().curModel;
				}


				if (operation.Equals ("X Axis")) {
					xArrow.SetActive (true);
					lastNotation = xArrow;
				} else if (operation.Equals ("Y Axis")) {
					yArrow.SetActive (true);
					lastNotation = yArrow;
				} else if (operation.Equals ("Z Axis")) {
					zArrow.SetActive (true);
					lastNotation = zArrow;
				} else if (operation.Equals ("XY Plane")) {
					xyMirror.SetActive (true);
					lastNotation = xyMirror;
					nextModel = SymXY (curModel);
					RenderTransModel (nextModel, XY);
				} else if (operation.Equals ("XZ Plane")) {
					xzMirror.SetActive (true);
					lastNotation = xzMirror;
					nextModel = SymXZ (curModel);
					RenderTransModel (nextModel, XZ);
				} else if (operation.Equals ("YZ Plane")) {
					yzMirror.SetActive (true);
					lastNotation = yzMirror;
					nextModel = SymYZ (curModel);
					RenderTransModel (nextModel, YZ);
				} else {
					;
				}

				lastObject = curObject;


			}
			
		} else {
			
			if (lastObject)
				lastObject.GetComponent<Renderer> ().material = originMaterial;

			if (lastNotation)
				lastNotation.SetActive (false);

			DestroyMirrorModel ();
			
		}
	}

	void DestroyMirrorModel(){
		for (int i = 0; i < mirrorModel.transform.childCount; i++) {
			GameObject block = mirrorModel.transform.GetChild (i).gameObject;
			Destroy (block);
		}
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


	void RenderTransModel(Dictionary<Vector3, bool> model, int index){

		DestroyMirrorModel ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					if (model [new Vector3 (x, y, z)]) {
						GameObject blockInstance = Instantiate (block);
						blockInstance.transform.parent = mirrorModel.transform;
						blockInstance.layer = 8;
						if (index == XY)
							blockInstance.transform.position = new Vector3 (x, y, z - 3.5f);
						else if (index == XZ)
							blockInstance.transform.position = new Vector3 (x, y - 3.5f, z);
						else
							blockInstance.transform.position = new Vector3 (x - 3.5f, y, z);
						
					}
				}
			}
		}

	}

}
