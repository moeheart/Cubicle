using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using UnityEditor;

public class GenerateObject : MonoBehaviour {

	public Material defaultMaterial;

	private string prefabPath = "Assets/Prefabs/CSG Prefabs/";
	private GameObject composite;

	// Use this for initialization
	void Start () {
		int childCount = this.transform.childCount;
		GameObject[] objs = new GameObject[childCount];
		for (int i = 0; i < childCount; ++i) {
			objs[i] = transform.GetChild(i).gameObject;
		}
		composite = CSGOperations.Subtract(objs[0], objs[1], defaultMaterial);
		composite.name = "csg1";
		foreach (GameObject obj in objs) {
			Destroy(obj);
		}
		PrefabUtility.CreatePrefab(prefabPath + composite.name + ".prefab", composite);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
