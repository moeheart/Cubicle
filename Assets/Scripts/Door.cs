using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private Color unlockColor = Color.cyan;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Unlock() {
		int childCount = this.transform.childCount;
		for (int i = 0; i < childCount; ++i) {
			GameObject cube = this.transform.GetChild(i).gameObject;
			cube.GetComponent<MeshRenderer>().material.color = unlockColor;
		}
	}
}
