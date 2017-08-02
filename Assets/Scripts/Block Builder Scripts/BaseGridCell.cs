using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseGridCell : MonoBehaviour {

	public IntVector2 coordinates;

	public CubeBlock cubeBlockPrefab;
	
	private int maxHeight = Configuration.maxHeight;

	//private bool isHighlighted = false;
	private LinkedList<CubeBlock> cubes = new LinkedList<CubeBlock>();

	private const float cubeLength = 1.0f;

	public int height {
		get {
			return cubes.Count;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Highlight() {
		Transform baseCell = this.transform.GetChild(0);
		//isHighlighted = true;
		baseCell.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
		if (cubes.Count > 0) {
			cubes.Last.Value.HighlightCube();
		}
	}

	public void Unhighlight() {
		Transform baseCell = this.transform.GetChild(0);
		//isHighlighted = false;
		baseCell.gameObject.GetComponent<Renderer>().material.color = Color.white;
		if (cubes.Count > 0) {
			cubes.Last.Value.UnhighlightCube();
		}
	}

	public void AddCube() {
		CubeBlock newCubeBlock = Instantiate(cubeBlockPrefab) as CubeBlock;
		newCubeBlock.transform.parent = this.transform;
		if (cubes.Count == maxHeight) {
			return;
		}
		if (cubes.Count == 0) {
			newCubeBlock.transform.localPosition = new Vector3(0f, .55f, 0f);
		}
		else {
			newCubeBlock.transform.localPosition = cubes.Last.Value.transform.localPosition 
				+ new Vector3(0f, cubeLength, 0f);
			cubes.Last.Value.UnhighlightCube();
		}
		cubes.AddLast(newCubeBlock);
		cubes.Last.Value.HighlightCube();
	}

	public void DeleteCube() {
		if (cubes.Count == 0) {
			return;
		}
		Destroy(cubes.Last.Value.gameObject);
		cubes.RemoveLast();
		if (cubes.Count > 0) {
			cubes.Last.Value.HighlightCube();
		}
	}

}
