using UnityEngine;
using System.Collections;

public class CubeBlock : MonoBehaviour {

	private Color defaultColor = new Color(1,1,1,0.8f);
	private Color highlightColor = new Color(0,1,1,0.8f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HighlightCube() {
		this.gameObject.GetComponent<Renderer>().material.color = highlightColor;

	}

	public void UnhighlightCube() {
		this.gameObject.GetComponent<Renderer>().material.color = defaultColor;
	}
}
