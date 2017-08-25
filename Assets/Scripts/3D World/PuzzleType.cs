using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PuzzleType {
	None = -1, 
	BlockBuilder = 0,
	CSG = 1,
	RevolutionSolid = 2,
	CubeShift = 3,
    Unfolding = 4,
	ViewPoint = 5,
	PlaneExploration = 6,
	TransformLimitation = 7,
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
        if (str == "Unfolding")
        {
            return PuzzleType.Unfolding;
        }
 		if (str == "cube shift") {
 			return PuzzleType.CubeShift;
 		}
		if (str == "view point") {
			return PuzzleType.ViewPoint;
		}
		if (str == "plane exploration") {
			return PuzzleType.PlaneExploration;
		}
		if (str == "transform limitation") {
			return PuzzleType.TransformLimitation;
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
			SceneManager.LoadScene("Revolution Solid Scene_"+DataUtil.GetCurrentRoomId().ToString(), LoadSceneMode.Single);
 		}
        if (type == PuzzleType.Unfolding)
        {
            SceneManager.LoadScene("Unfolding Scene", LoadSceneMode.Single);
        }
 		if (type == PuzzleType.CubeShift) {
 			SceneManager.LoadScene("Cube Shift Scene", LoadSceneMode.Single);
 		}
		if (type == PuzzleType.ViewPoint) {
			SceneManager.LoadScene("View Point", LoadSceneMode.Single);
		}
		if (type == PuzzleType.PlaneExploration) {
			SceneManager.LoadScene("Q1", LoadSceneMode.Single);
		}
		if (type == PuzzleType.TransformLimitation) {
			SceneManager.LoadScene("Transform Limitation", LoadSceneMode.Single);
		}
	}
}