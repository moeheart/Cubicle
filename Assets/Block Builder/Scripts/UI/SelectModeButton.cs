using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeButton : MonoBehaviour {

	public BlockBuilderConfigs.RotationMethod rotationMethod;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMode() {
		BlockBuilderConfigs.rotationMethod = rotationMethod;
	}
}
