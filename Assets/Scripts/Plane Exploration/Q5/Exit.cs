using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public void Click () {
		Application.LoadLevel ("Q5");
		SceneManager.LoadScene("World Scene");
	}
}
