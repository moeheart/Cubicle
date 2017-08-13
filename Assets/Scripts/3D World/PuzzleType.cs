using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PuzzleType {
	None = -1, 
	BlockBuilder = 0,
	CSG = 1,
	RevolutionSolid = 2,
	CubeShift = 3
}

public static class PuzzleTypes {
	public static PuzzleType GetTypeFromString(this string str) {
		if (str == "block builder") {
			return PuzzleType.BlockBuilder;
		}
		if (str == "CSG") {
			return PuzzleType.CSG;
		}
		if (str == "revolution solid") {
 			return PuzzleType.RevolutionSolid;
 		}
 		if (str == "cube shift") {
 			return PuzzleType.CubeShift;
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
		if (type == PuzzleType.RevolutionSolid) {
 			SceneManager.LoadScene("Revolution Solid Scene", LoadSceneMode.Single);
 		}
 		if (type == PuzzleType.CubeShift) {
 			SceneManager.LoadScene("Cube Shift Scene", LoadSceneMode.Single);
 		}
	}
}