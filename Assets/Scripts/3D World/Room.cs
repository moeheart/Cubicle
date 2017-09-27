using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public int id;
	public Vector3 position;
	public Vector3 dimension;
	public Color color;
	public TriggerDevice[] triggerPrefabs;

	public PuzzleType puzzleType;
	public List<Door> doorsToUnlock {get; set;}

	private List<Door> doors = new List<Door>();
	private TriggerDevice trigger;

	private const float lengthPerUnit = Configurations.lengthPerUnit;

	public bool isUnlocked {get; private set;}

	public Vector3 size {
		get {
			return dimension * lengthPerUnit;
		}
	}

	public void Initialize(int id, Vector3 position, Vector3 dimension, Color color, PuzzleType puzzleType) {
		this.id = id;
		this.position = position;
		this.dimension = dimension;
		this.color = color;
		this.puzzleType = puzzleType;
		this.transform.position = position;
		this.isUnlocked = false;
	}

	public void AddDoor(Door door) {
		doors.Add(door);
	}

	public void OnCompleteRoomObjective() {
		isUnlocked = true;
		if (doorsToUnlock != null) {
			//Debug.Log("doors to unlock isnt null, size:" + doorsToUnlock.Count);
			foreach (Door door in doorsToUnlock) {
				door.Unlock();
			}
		}
		else {
			//Debug.Log("doors to unlock is null");
			foreach (Door door in doors) {
				door.Unlock();
			}
		}
	}

	public void AddTrigger() {
		if (puzzleType == PuzzleType.None) {
			return;
		}
		trigger = Instantiate(triggerPrefabs[(int)puzzleType]) as TriggerDevice;
		trigger.transform.parent = this.transform;
		trigger.transform.localPosition = new Vector3(this.size.x/2, 
			1.0f + trigger.gameObject.GetComponent<Collider>().bounds.extents.y, 
			this.size.z/2);
		trigger.thisRoom = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
