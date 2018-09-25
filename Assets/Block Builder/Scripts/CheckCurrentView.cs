using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCurrentView {

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
}
