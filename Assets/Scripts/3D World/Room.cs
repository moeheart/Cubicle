using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public int id;
	public Vector3 position;
	public Vector3 dimension;
	public Color color;
	public TriggerDevice triggerPrefab;

	private List<Door> doors = new List<Door>();
	private TriggerDevice trigger;

	private const float lengthPerUnit = Configurations.lengthPerUnit;

	public bool isUnlocked;

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
		this.transform.position = position;
	}

	public void AddDoor(Door door) {
		doors.Add(door);
	}

	public void OnCompleteRoomObjective() {
		isUnlocked = true;
		foreach (Door door in doors) {
			door.Unlock();
		}
	}

	public void AddTrigger() {
		trigger = Instantiate(triggerPrefab) as TriggerDevice;
		trigger.transform.parent = this.transform;
		trigger.transform.localPosition = new Vector3(this.size.x/2, 1.5f, this.size.z/2);
		trigger.thisRoom = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
