using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class BlockBuilderLog {

	public static void Log(string path, int id, string log) {
		/*if (!File.Exists(path)) {
			File.AppendAllText(path, "RoomID, PuzzleType:  TimeSinceGameStart, TimeSinceEnterLevel,  Action\n");
		}
		float t = Time.time - BaseGrid.startTime;
		File.AppendAllText(path, id + ", Block Builder: " + Time.time + ", " + t + ", " +  log + "\n");*/
	}
}
