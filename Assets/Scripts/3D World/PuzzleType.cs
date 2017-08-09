using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PuzzleType {
	None = -1, 
	BlockBuilder = 0,
	CSG = 1
}

public static class PuzzleTypes {
	public static PuzzleType GetTypeFromString(this string str) {
		if (str == "block builder") {
			return PuzzleType.BlockBuilder;
		}
		if (str == "CSG") {
			return PuzzleType.CSG;
		}
		return PuzzleType.None;
	}

	public static void LoadScene(this PuzzleType type) {
		if (type == PuzzleType.BlockBuilder) {
			SceneManager.LoadScene("Block Builder Scene", LoadSceneMode.Single);
		}
		if (type == PuzzleType.CSG) {
			SceneManager.LoadScene("CSG Scene", LoadSceneMode.Single);
		}
	}
}