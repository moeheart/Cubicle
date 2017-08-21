using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseProcessing : MonoBehaviour {
	protected float wx, wy;
	// Use this for initialization
	void Start () {
		wx = Camera.main.pixelRect.center.x;
		wy = Camera.main.pixelRect.center.y;
	}
	
	// Update is called once per frame
	void Update () {
		RevSolidGameInfo.CheckEndOfGame ();
	}

	protected void OnResize(){
		if (wx != Camera.main.pixelRect.center.x || wy != Camera.main.pixelRect.center.y) {
			wx = Camera.main.pixelRect.center.x;
			wy = Camera.main.pixelRect.center.y;
		}
	}
}
