using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaneExplorationWin : MonoBehaviour {

	private string sceneName;

	void Start(){
		sceneName = SceneManager.GetActiveScene ().name;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			if (sceneName == "Q1")
				other.gameObject.GetComponent<Player1> ().Win ();
			else if (sceneName == "Q2")
				other.gameObject.GetComponent<Player2> ().Win ();
			else if (sceneName == "Q3")
				other.gameObject.GetComponent<Player3> ().Win ();
			else if (sceneName == "Q4")
				other.gameObject.GetComponent<Player4> ().Win ();
			else if (sceneName == "Q5")
				other.gameObject.GetComponent<Player5> ().Win ();
			else if (sceneName == "Q6")
				other.gameObject.GetComponent<Player6> ().Win ();
			else if (sceneName == "Q7")
				other.gameObject.GetComponent<Player7> ().Win ();
			else
				;
		}
	}


}
