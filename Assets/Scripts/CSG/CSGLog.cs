using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class CSGLog{
	public static void Log(string path, int id, string log) {
		if (DataUtil.IsUnlocked(id)) {
			return;
		}
		File.AppendAllText(path, id + ": " + log + "\n");
	}
}
