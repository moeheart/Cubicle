using UnityEngine;
using System.Collections;

public class TriggerDevice : MonoBehaviour {

	public Room thisRoom;

	public void Operate() {
		thisRoom.OnCompleteRoomObjective();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
