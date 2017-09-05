using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class BlockBuilderLog {

	public static void Log(string path, int id, string log) {
		File.AppendAllText(path, id + ": " + log + "\n");
	}
}
