using UnityEngine;
using System.Collections;

public class Room : ScriptableObject {

	public int id;
	public Vector3 position;
	public Vector3 dimension;

	public Color color;

	public void Initialize(int id, Vector3 position, Vector3 dimension, Color color) {
		this.id = id;
		this.position = position;
		this.dimension = dimension;
		this.color = color;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
