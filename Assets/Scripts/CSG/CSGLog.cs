using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class CSGLog{
	public static void Log(string path, int id, string log) {
		float t = Time.time - ObjectsManager.startTime;
		File.AppendAllText(path, id + ": " + t + ", " + log + "\n");
	}
}
