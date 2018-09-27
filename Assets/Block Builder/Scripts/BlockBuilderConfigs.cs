using UnityEngine;
using System.Collections;

public class BlockBuilderConfigs{

	public static IntVector2 gridSize = new IntVector2(3,3);

	public static int maxHeight = 5;

	public static int panelLengthPerBlock = 100;

	public static string jsonFilename = "Block Builder.json";

	public static int id = 1;

	public static string blockBuilderTutorialSceneName = "Block Builder Tutorial";
	public static string blockBuilderMainGameSceneName = "Block Builder Main Game";

	public enum RotationMethod {Gyro, Button, GyroAndButton};

	public static RotationMethod rotationMethod = RotationMethod.Button;

	public static Vector3 baseGridWorldPosition = Vector3.zero;

	public static float sensitivityGyro = 8.0f;

	public static float distanceToBaseGrid = 10.0f;

	public static float cosineSimilarityLowerBound = 0.94f;

	public static string participantName;

	private static Vector3 cameraPos = new Vector3 (5, 4.5f, -8.5f);
	private static Vector3 rotation = new Vector3 (20, -30, 0);

}
