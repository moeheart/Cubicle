using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iso0: MonoBehaviour {

	public GameObject isoCamera;

	public GameObject projectionButton;
	public GameObject isoButton;

	public void Click () {

		isoCamera.SetActive(true);

		projectionButton.SetActive (true);
		isoButton.SetActive (false);

	}
}
