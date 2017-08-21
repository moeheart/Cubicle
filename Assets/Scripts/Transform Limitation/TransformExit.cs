using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformExit : MonoBehaviour {

	public void Click(){
		SceneManager.LoadScene ("World Scene");
	}
}
