using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class BlockBuilderLog { 

	public static string logPath = Path.Combine(Application.persistentDataPath, "Logs/Block Builder");
	public static string logFile = Path.Combine(logPath, "log.csv");

	public static void Log(int id, string log) {

		Directory.CreateDirectory(logPath);
		
		if (!File.Exists(logFile)) {
			File.AppendAllText(logFile, "name" + ", " + "rotationMethod" + ", " + "levelId" + ", " + "TimeSinceGameStart" + ", " + "TimeSinceEnterLevel" + ", " + "Action" + "\n");
		}
		float t = Time.time - BaseGrid.startTime;
		// Debug.Log("rotationMethod:" + BlockBuilderConfigs.thisLevelRotationMethod.ToString());
		File.AppendAllText(
			logFile, BlockBuilderConfigs.participantName + ", " + BlockBuilderConfigs.thisLevelRotationMethod.ToString() + ", " + id + ", " + Time.time + ", " + t + ", " +  log + "\n");
		
	}
}
