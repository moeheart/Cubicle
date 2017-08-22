using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewPointExit : MonoBehaviour {

	public void Click(){
		SceneManager.LoadScene ("World Scene");
	}
}
