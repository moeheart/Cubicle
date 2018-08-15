using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsPanel : MonoBehaviour {

	public Slider touchHorSlider;
	public Slider touchVertSlider;
	public Slider gyroHorSlider;
	public Slider gyroVertSlider;

	private GameObject player;

	// Use this for initialization
	void Start () {
		//TODO: Set Slider position. 
		player = GameObject.FindGameObjectWithTag("Player");
		touchHorSlider.value = iOSPlayerTouchInput.touchSensitivityHor;
		touchVertSlider.value = iOSPlayerTouchInput.touchSensitivityVert;
		gyroHorSlider.value = iOSPlayerTouchInput.sensitivityHor;
		gyroVertSlider.value = iOSPlayerTouchInput.sensitivityVert;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTouchHorSliderValueChanged() {
		iOSPlayerTouchInput.touchSensitivityHor = touchHorSlider.value;
	}

	public void OnTouchVertSliderValueChanged() {
		iOSPlayerTouchInput.touchSensitivityVert = touchVertSlider.value;
	}

	public void OnGyroHorSliderValueChanged() {
		iOSPlayerTouchInput.sensitivityHor = gyroHorSlider.value;
	}

	public void OnGyroVertSliderValueChanged() {
		iOSPlayerTouchInput.sensitivityVert = gyroVertSlider.value;
	}
}
