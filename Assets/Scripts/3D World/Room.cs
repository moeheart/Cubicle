using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : ScriptableObject {

	public int id;
	public Vector3 position;
	public Vector3 dimension;
	public Color color;

	private List<Door> doors = new List<Door>();

	private const float lengthPerUnit = Configurations.lengthPerUnit;

	public Vector3 size {
		get {
			return dimension * lengthPerUnit;
		}
	}

	public void Initialize(int id, Vector3 position, Vector3 dimension, Color color) {
		this.id = id;
		this.position = position;
		this.dimension = dimension;
		this.color = color;
	}

	public void AddDoor(Door door) {
		doors.Add(door);
	}

	public void OnCompleteRoomObjective() {
		foreach (Door door in doors) {
			door.Unlock();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
