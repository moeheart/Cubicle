using UnityEngine;
using System.Collections;

public class RoomBorders : MonoBehaviour {

	public GameObject floor;
	public GameObject wallXY;
	public GameObject wallZY;

	private float lengthPerUnit = Configurations.lengthPerUnit;
	private float borderThickness = Configurations.borderThickness;

	public void BuildRoom(Vector3 position, Vector3 dimension) {
		Debug.Log("building room... position: " + position + "size: " + dimension);

		Vector3 bottomFacePosition = new Vector3(position.x, position.y + borderThickness/2, position.z);
		BuildSide(bottomFacePosition, Direction.XZ, new Vector2(dimension[0], dimension[2]));

		Vector3 frontFacePosition = new Vector3(position.x, position.y, position.z + borderThickness/2);
		BuildSide(frontFacePosition, Direction.XY, new Vector2(dimension[0], dimension[1]));

		Vector3 leftFacePosition = new Vector3(position.x + borderThickness/2, position.y, position.z);
		BuildSide(leftFacePosition, Direction.ZY, new Vector2(dimension[2], dimension[1]));

		Vector3 topFacePosition = new Vector3(position.x, 
			position.y + dimension[1] * lengthPerUnit - borderThickness/2, position.z);
		BuildSide(topFacePosition, Direction.XZ, new Vector2(dimension[0], dimension[2]));

		Vector3 backFacePosition = new Vector3(position.x, position.y, 
			position.z + dimension[2] * lengthPerUnit - borderThickness/2);
		BuildSide(backFacePosition, Direction.XY, new Vector2(dimension[0], dimension[1]));

		Vector3 rightFacePosition = new Vector3(position.x + dimension[0] * lengthPerUnit - borderThickness/2, 
			position.y, position.z);
		BuildSide(rightFacePosition, Direction.ZY, new Vector2(dimension[2], dimension[1]));
	}

	private void BuildSide(Vector3 position, Direction direction, Vector2 size) {
		IntVector2 dimension = new IntVector2((int)size.x, (int)size.y);
		for (int i = 0; i < dimension.x; ++i) {
			for (int j = 0; j < dimension.z; ++j) {
				GameObject border;
				string name;
				Vector3 pos;
				switch (direction) {
					case Direction.XZ:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y, position.z + j * lengthPerUnit);
						name = pos + "-XZ";

						if (GameObject.Find(name) == null) {
							border = Instantiate(floor) as GameObject;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;

					case Direction.XY:
						pos = new Vector3(position.x + i * lengthPerUnit, 
							position.y + j * lengthPerUnit, position.z);
						name = pos + "-XY";

						if (GameObject.Find(name) == null) {
							border = Instantiate(wallXY) as GameObject;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;

					case Direction.ZY:
						pos = new Vector3(position.x, 
							position.y + j * lengthPerUnit, position.z + i * lengthPerUnit);
						name = pos + "-ZY";

						if (GameObject.Find(name) == null) {
							border = Instantiate(wallZY) as GameObject;
							border.transform.parent = this.transform;
							border.transform.position = pos;
							border.name = name;
						}
						else {
							Destroy(GameObject.Find(name));
						}
						break;
				}
			}
		}
	}

	public void FindOverlapArea(Room room1, Room room2, out Vector3 position, out Vector2 size, out Direction direction) {
		position = Vector3.zero;
		size = Vector2.zero;
		direction = Direction.XZ;

		Vector2 intersectionPoint;
		Vector2 intersectionSize;

		if (room1.position.y == room2.position.y + room2.dimension.y 
				|| room1.position.y + room1.dimension.y == room2.position.y) {
			//room1's bottom and room2's top
			direction = Direction.XZ;
			Vector2 pos1 = new Vector2(room1.position.x, room1.position.z);
			Vector2 pos2 = new Vector2(room2.position.x, room2.position.z);
			Vector2 size1 = new Vector2(room1.dimension.x, room1.dimension.z);
			Vector2 size2 = new Vector2(room2.dimension.x, room2.dimension.z);
			
			FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);

			if (room1.position.y == room2.position.y + room2.dimension.y) {
				position = new Vector3(intersectionPoint[0], room1.position.y, intersectionPoint[1]);
				size = intersectionSize;
				return;
			}

			else {
				position = new Vector3(intersectionPoint[0], room2.position.y, intersectionPoint[1]);
				size = intersectionSize;
				return;
			}

		}

		if (room1.position.x == room2.position.x + room2.dimension.x 
				|| room1.position.x + room1.dimension.x == room2.position.x) {
			//room1's left and room2's right
			direction = Direction.ZY;
			Vector2 pos1 = new Vector2(room1.position.z, room1.position.y);
			Vector2 pos2 = new Vector2(room2.position.z, room2.position.y);
			Vector2 size1 = new Vector2(room1.dimension.z, room1.dimension.y);
			Vector2 size2 = new Vector2(room2.dimension.z, room2.dimension.y);

			FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);

			if (room1.position.x == room2.position.x + room2.dimension.x) {
				position = new Vector3(room1.position.x, intersectionPoint[1], intersectionPoint[0]);
				size = intersectionSize;
				return;
			}

			else {
				position = new Vector3(room2.position.x, intersectionPoint[1], intersectionPoint[0]);
				size = intersectionSize;
				return;
			}
		}

		if (room1.position.z == room2.position.z + room2.dimension.z
				|| room1.position.z + room1.dimension.z == room2.dimension.z) {
			//room1's front and room2's back
			direction = Direction.XY;
			Vector2 pos1 = new Vector2(room1.position.x, room1.position.y);
			Vector2 pos2 = new Vector2(room2.position.x, room2.position.y);
			Vector2 size1 = new Vector2(room1.dimension.x, room1.dimension.y);
			Vector2 size2 = new Vector2(room2.dimension.x, room2.dimension.y);

			FindOverlapOfRectangle(pos1, pos2, size1, size2, out intersectionPoint, out intersectionSize);

			if (room1.position.z == room2.position.z + room2.dimension.z) {
				position = new Vector3(intersectionPoint[0], intersectionPoint[1], room1.position.z);
				size = intersectionSize;
				return;
			}

			else {
				position = new Vector3(intersectionPoint[0], intersectionPoint[1], room2.position.z);
				size = intersectionSize;
				return;
			}
		}
	}

	private void FindOverlapOfRectangle(Vector2 pos1, Vector2 pos2, Vector2 size1, Vector2 size2, 
			out Vector2 outPos, out Vector2 outSize) {
		float left = Mathf.Max(pos1.x, pos2.x);
		float right = Mathf.Min(pos1.x + size1.x , pos2.x + size2.x);
		float bottom = Mathf.Max(pos1.y, pos2.y);
		float top = Mathf.Min(pos1.y + size1.y, pos2.y + size2.y);
		outPos = new Vector2(left, bottom);
		outSize = new Vector2(right - left, top - bottom);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
