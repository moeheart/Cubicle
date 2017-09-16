using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffects : MonoBehaviour {

	public static BrightnessSaturationAndContrast bscCamera;
	public Camera camera = new Camera();
	public GameInfo gameInfo;
	float initialSaturation;
	float initialGreenValue;
	// Use this for initialization
	void Awake(){
		gameInfo = GameInfo.getInstance ();
		camera = Camera.main;
		bscCamera = camera.GetComponent<BrightnessSaturationAndContrast>();
		initialSaturation = bscCamera.saturation;
		initialGreenValue = bscCamera.green;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void GreyTheScreen(){
		bscCamera.saturation = 0;
		bscCamera.green = 0.1f;
	}
}
