using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeView {

	public static Dictionary<Segment, LineType> GetFrontView(Dictionary<IntVector3,bool> cubes) {
		Dictionary<Segment, LineType> frontView = new Dictionary<Segment, LineType>();
		IntVector2 size = new IntVector2(Configuration.gridSize.x, Configuration.maxHeight);
		for (int x = 0; x <= size.x; ++x) {
			for (int z = 0; z <= size.z; ++z) {
				IntVector2 p1 = new IntVector2(x,z);

				if (x < size.x) {
					IntVector2 p2 = new IntVector2(x+1, z);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.FrontView);
					frontView.Add(segment, lineType);
				}

				if (z < size.z) {
					IntVector2 p2 = new IntVector2(x, z+1);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.FrontView);
					frontView.Add(segment, lineType);
				}
			}
		}
		return frontView;
	}

	public static Dictionary<Segment, LineType> GetRightView(Dictionary<IntVector3,bool> cubes) {
		Dictionary<Segment, LineType> rightView = new Dictionary<Segment, LineType>();
		IntVector2 size = new IntVector2(Configuration.gridSize.z, Configuration.maxHeight);
		for (int x = 0; x <= size.x; ++x) {
			for (int z = 0; z <= size.z; ++z) {
				IntVector2 p1 = new IntVector2(x,z);

				if (x < size.x) {
					IntVector2 p2 = new IntVector2(x+1, z);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.RightView);
					rightView.Add(segment, lineType);
				}

				if (z < size.z) {
					IntVector2 p2 = new IntVector2(x, z+1);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.RightView);
					rightView.Add(segment, lineType);
				}
			}
		}
		return rightView;
	}

	public static Dictionary<Segment, LineType> GetTopView(Dictionary<IntVector3,bool> cubes) {
		Dictionary<Segment, LineType> topView = new Dictionary<Segment, LineType>();
		IntVector2 size = new IntVector2(Configuration.gridSize.x, Configuration.gridSize.z);
		for (int x = 0; x <= size.x; ++x) {
			for (int z = 0; z <= size.z; ++z) {
				IntVector2 p1 = new IntVector2(x,z);

				if (x < size.x) {
					IntVector2 p2 = new IntVector2(x+1, z);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.TopView);
					topView.Add(segment, lineType);
				}

				if (z < size.z) {
					IntVector2 p2 = new IntVector2(x, z+1);
					Segment segment = new Segment(p1, p2);
					LineType lineType = GetLineType(segment, cubes, ViewType.TopView);
					topView.Add(segment, lineType);
				}
			}
		}
		return topView;
	}//End of GetTopView

	private static LineType GetLineType(Segment segment, Dictionary<IntVector3, bool> cubes, ViewType viewType) {
		switch (viewType) {
			case ViewType.TopView: {
				//Currently only supports cubes, and there cant be any "hollows"
				int height = Configuration.maxHeight;
				List<bool> blockListA = new List<bool>(height);
				List<bool> blockListB = new List<bool>(height);
				IntVector2 gridCoordA;
				IntVector2 gridCoordB;
				if (segment.p2.x == segment.p1.x + 1) {
					gridCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					gridCoordB = new IntVector2(segment.p1.x, segment.p1.z-1);
				}
				else if (segment.p2.z == segment.p1.z + 1) {
					gridCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					gridCoordB = new IntVector2(segment.p1.x - 1, segment.p1.z);
				}
				else {
					return LineType.NoLine;
				}

				for (int h = height - 1; h >= 0; --h) {
					IntVector3 coordA = new IntVector3(gridCoordA, h);
					IntVector3 coordB = new IntVector3(gridCoordB, h);
					bool value;
					cubes.TryGetValue(coordA, out value);
					blockListA.Add(value);
					cubes.TryGetValue(coordB, out value);
					blockListB.Add(value);
				}

				return ComputeLineType(blockListA, blockListB);
			}//End of case ViewType.TopView

			case ViewType.FrontView: {
				int length = Configuration.gridSize.z;
				List<bool> blockListA = new List<bool>(length);
				List<bool> blockListB = new List<bool>(length);
				IntVector2 frontPlaneCoordA;
				IntVector2 frontPlaneCoordB;
				if (segment.p2.x == segment.p1.x + 1) {
					frontPlaneCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					frontPlaneCoordB = new IntVector2(segment.p1.x, segment.p1.z - 1);
				}
				else if (segment.p2.z == segment.p1.z + 1) {
					frontPlaneCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					frontPlaneCoordB = new IntVector2(segment.p1.x - 1, segment.p1.z);
				}
				else {
					return LineType.NoLine;
				}

				for (int h = 0; h < length; ++h) {
					IntVector3 coordA = new IntVector3(frontPlaneCoordA.x, h, frontPlaneCoordA.z);
					IntVector3 coordB = new IntVector3(frontPlaneCoordB.x, h, frontPlaneCoordB.z);
					bool value;
					cubes.TryGetValue(coordA, out value);
					blockListA.Add(value);
					cubes.TryGetValue(coordB, out value);
					blockListB.Add(value);
				}

				return ComputeLineType(blockListA, blockListB);
			}//End of case ViewType.FrontView

			case ViewType.RightView: {
				int length = Configuration.gridSize.x;
				List<bool> blockListA = new List<bool>(length);
				List<bool> blockListB = new List<bool>(length);
				IntVector2 rightPlaneCoordA;
				IntVector2 rightPlaneCoordB;
				if (segment.p2.x == segment.p1.x + 1) {
					rightPlaneCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					rightPlaneCoordB = new IntVector2(segment.p1.x, segment.p1.z - 1);
				}
				else if (segment.p2.z == segment.p1.z + 1) {
					rightPlaneCoordA = new IntVector2(segment.p1.x, segment.p1.z);
					rightPlaneCoordB = new IntVector2(segment.p1.x - 1, segment.p1.z);
				}
				else {
					return LineType.NoLine;
				}

				for (int h = length - 1; h >= 0; --h) {
					IntVector3 coordA = new IntVector3(h, rightPlaneCoordA.x, rightPlaneCoordA.z);
					IntVector3 coordB = new IntVector3(h, rightPlaneCoordB.x, rightPlaneCoordB.z);
					bool value;
					cubes.TryGetValue(coordA, out value);
					blockListA.Add(value);
					cubes.TryGetValue(coordB, out value);
					blockListB.Add(value);
				}

				return ComputeLineType(blockListA, blockListB);
			}//End of case ViewType.RightView
		}

		return LineType.NoLine;
	}//End of GetLineType

	private static LineType ComputeLineType(List<bool> listA, List<bool> listB) {
		bool noSolidLine = false;
		for (int i = 0; i < listA.Count; ++i) {
			if (listA[i] == false && listB[i] == false) {
				continue;
			}
			if (listA[i] && listB[i]) {
				noSolidLine = true;
			}
			else {
				if (noSolidLine == true) {
					return LineType.DashedLine;
				}
				else {
					return LineType.SolidLine;
				}
			}
		}
		return LineType.NoLine;
	}
}
