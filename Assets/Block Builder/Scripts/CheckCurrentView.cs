using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewUtil {

	private static float cosineSimilarityLowerBound = BlockBuilderConfigs.cosineSimilarityLowerBound;

	public static bool IsAlignedWithTopView(Transform transform) {
		return IsAligned(transform.right, -transform.forward, transform.up);
	}

	public static bool IsAlignedWithFrontView(Transform transform) {
		return IsAligned(transform.right, transform.up, transform.forward);
	}

	public static bool IsAlignedWithRightView(Transform transform) {
		return IsAligned(-transform.forward, transform.up, transform.right);
	}

	private static bool IsAligned(Vector3 a, Vector3 b, Vector3 c) {
		float cos1 = Vector3.Dot(a, Vector3.right);
		float cos2 = Vector3.Dot(b, Vector3.up);
		float cos3 = Vector3.Dot(c, Vector3.forward);
		bool isAligned =  (cos1 > BlockBuilderConfigs.cosineSimilarityLowerBound
				&& cos2 > BlockBuilderConfigs.cosineSimilarityLowerBound
				&& cos3 > BlockBuilderConfigs.cosineSimilarityLowerBound);
		return isAligned;
	}
	public static bool canRotateAroundXAxis(Transform transform) {
		float cos1 = Vector3.Dot(transform.right, Vector3.right);
		float cos2 = Vector3.Dot(transform.right, Vector3.forward);
		cos1 = Mathf.Abs(cos1);
		cos2 = Mathf.Abs(cos2);
		return (cos1 > cosineSimilarityLowerBound || cos2 > cosineSimilarityLowerBound);
	}

	public static bool canRotateAroundYAxis(Transform transform) {
		float cos1 = Vector3.Dot(transform.up, Vector3.up);
		cos1 = Mathf.Abs(cos1);
		return (cos1 > cosineSimilarityLowerBound);
	}

	public static bool canRotateAroundZAxis(Transform transform) {
		float cos1 = Vector3.Dot(transform.forward, Vector3.up);
		cos1 = Mathf.Abs(cos1);
		return (cos1 > cosineSimilarityLowerBound);
	}

	public static void PlaceCameraFromRotation(Transform transform, float dist) {
		Vector3 point = transform.position + transform.forward * dist;
		transform.position -= point;
	}

}
