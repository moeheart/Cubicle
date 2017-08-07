using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PuzzleType {
	None = -1, 
	BlockBuilder = 0
}

public static class PuzzleTypes {
	public static PuzzleType GetTypeFromString(this string str) {
		if (str == "block builder") {
			return PuzzleType.BlockBuilder;
		}
		return PuzzleType.None;
	}

	public static void LoadScene(this PuzzleType type) {
		if (type == PuzzleType.BlockBuilder) {
			SceneManager.LoadScene("Block Builder Scene", LoadSceneMode.Single);
		}
	}
}