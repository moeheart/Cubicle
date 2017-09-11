using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projection0 : MonoBehaviour {

	public GameObject isoCamera, projectionButton, isoButton;

	public void Click () {

		isoCamera.SetActive(false);

		projectionButton.SetActive (false);
		isoButton.SetActive (true);

	}
}
