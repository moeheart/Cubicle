using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retry : MonoBehaviour {

	public void Click () {
		Application.LoadLevelAsync ("View Point");
	}

}
