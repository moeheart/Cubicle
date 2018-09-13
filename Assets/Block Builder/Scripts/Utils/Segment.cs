using UnityEngine;
using System.Collections;

public struct Segment {
	public IntVector2 p1, p2;

	public Segment(IntVector2 p1, IntVector2 p2) {
		//TODO
		//Should we ensure there is a specific order?
		//For example, p2 > p1 ?
		this.p1 = p1;
		this.p2 = p2;
	}
}
