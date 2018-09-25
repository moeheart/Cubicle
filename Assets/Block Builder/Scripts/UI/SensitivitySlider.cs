using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Slider>().value = BlockBuilderConfigs.sensitivityGyro;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnValueChanged() {
		BlockBuilderConfigs.sensitivityGyro = this.GetComponent<Slider>().value;
	}

}
