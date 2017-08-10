using UnityEngine;
using System.Collections;

public class ObjectControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
		//These controls are pretty bad right now
		//Change it to something more natural
		//The Y-axis
		if (Input.GetKeyDown(KeyCode.I)) {
			this.transform.localPosition += new Vector3(0,1,0);
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			this.transform.localPosition += new Vector3(0,-1,0);
		}

		//The X-axis
		if (Input.GetKeyDown(KeyCode.J)) {
			this.transform.localPosition += new Vector3(-1,0,0);
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			this.transform.localPosition += new Vector3(1,0,0);
		}

		//The Z-axis
		if (Input.GetKeyDown(KeyCode.U)) {
			this.transform.localPosition += new Vector3(0,0,1);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			this.transform.localPosition += new Vector3(0,0,-1);
		}

		//Rotate around Y-axis
		if (Input.GetKeyDown(KeyCode.D)) {
			this.transform.Rotate(0,90,0);
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			this.transform.Rotate(0,-90,0);
		}

		//Rotate around X-axis
		if (Input.GetKeyDown(KeyCode.W)) {
			this.transform.Rotate(90,0,0);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			this.transform.Rotate(-90,0,0);
		}

		//Rotate around Z-axis
		if (Input.GetKeyDown(KeyCode.Q)) {
			this.transform.Rotate(0,0,90);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			this.transform.Rotate(0,0,-90);
		}
	}
}
