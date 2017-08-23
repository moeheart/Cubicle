using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevSolid {
	public GameObject gameObject;
	public int index;
	private Material mat;
	public Shader alphaShader;

	public RevSolid(int newIndex){
		index = newIndex;

		gameObject = GameObject.Instantiate(Resources.Load("revsolid"+index.ToString()) as GameObject,new Vector3(15,0,0),Quaternion.identity);//GameObject.Find ("poly" + index.ToString());

		if (gameObject.GetComponent<MeshRenderer>().material !=null) {
			mat = gameObject.GetComponent<MeshRenderer>().material;
			if (alphaShader != null) {
				mat.shader = alphaShader;
			} else {
				alphaShader = mat.shader;
				//Debug.Log (shader);
			}
			
		} else {
			gameObject.GetComponent<MeshRenderer> ().material = new Material (alphaShader);
			mat=gameObject.GetComponent<MeshRenderer>().material;
		}

	}
		
}
