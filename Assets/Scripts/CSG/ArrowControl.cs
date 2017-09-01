using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowDirection {
	plusX = 0,
	plusZ = 1,
	minusX = 2,
	minusZ = 3,
	plusY = 4,
	minusY = 5
}

public class ArrowControl : MonoBehaviour {
	[SerializeField]
	public ArrowDirection direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			yield return new WaitForSeconds(0.1f);
			if (CSGManager.objectsManager.selectedObj)
				CSGManager.objectsManager.selectedObj.MoveInDirection(direction);
		}
	}
}
