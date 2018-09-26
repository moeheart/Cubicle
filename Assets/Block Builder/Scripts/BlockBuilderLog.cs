using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class BlockBuilderLog { 

	public static string logPath = Path.Combine(Application.persistentDataPath, "Logs/Block Builder");
	public static string logFile = Path.Combine(logPath, "log.txt");

	public static void Log(int id, string log) {

		Directory.CreateDirectory(logPath);
		
		if (!File.Exists(logFile)) {
			File.AppendAllText(logFile, "levelId" + '\t' + "TimeSinceGameStart" + '\t' + "TimeSinceEnterLevel" + '\t' + "Action" + "\n");
		}
		float t = Time.time - BaseGrid.startTime;
		File.AppendAllText(logFile, id + "\t" + Time.time + "\t" + t + ", " +  log + "\n");
		
	}
}
