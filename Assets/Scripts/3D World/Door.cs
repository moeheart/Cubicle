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
		//TODO
		GameObject doorChild = this.transform.FindChild("Door").gameObject;
		Destroy(doorChild);
	}
}
