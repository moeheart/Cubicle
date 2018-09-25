using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class BlockBuilderLog { 

	public static string logPath = Path.Combine(Application.persistentDataPath, "Logs/Block Builder/Block Builder.txt");

	public static void Log(int id, string log) {
		if (!File.Exists(logPath)) {
			File.AppendAllText(logPath, "levelId" + '\t' + "TimeSinceGameStart" + '\t' + "TimeSinceEnterLevel" + '\t' + "Action" + "\n");
		}
		float t = Time.time - BaseGrid.startTime;
		File.AppendAllText(logPath, id + "\t" + Time.time + "\t" + t + ", " +  log + "\n");
	}
}
