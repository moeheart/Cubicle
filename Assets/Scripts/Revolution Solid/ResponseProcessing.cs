using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseProcessing : MonoBehaviour {
	protected static float wx, wy;
	protected static float resizedScale=1;

	//static float originalHeight;
	//static float originalWidth;


	// Use this for initialization
	void Awake(){
		//originalHeight=475;
		//originalWidth=800;
	}
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
		//resizedScale=Camera.main.pixelWidth/originalWidth;
	}
}
