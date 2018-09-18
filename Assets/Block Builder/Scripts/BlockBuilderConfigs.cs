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

	public static Vector3 topViewAngle = new Vector3(-90, 90, -90);

	public static Vector3 frontViewAngle = new Vector3(0, 0, 0);

	public static Vector3 rightViewAngle = new Vector3(0, 90, 0);

	public static float sensitivityGyro = 8.0f;

}
