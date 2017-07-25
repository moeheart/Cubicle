using UnityEngine;
using System.Collections;

public class RoomBorders : MonoBehaviour {

	public GameObject floor;
	public GameObject wallXY;
	public GameObject wallZY;

	private float lengthPerUnit = Configurations.lengthPerUnit;

	public void BuildRoom(Vector3 position, Vector3 dimension) {
		Debug.Log("building room... position: " + position + "size: " + dimension);
		BuildSide(position, Direction.XZ, new Vector2(dimension[0], dimension[2]));
		BuildSide(position, Direction.XY, new Vector2(dimension[0], dimension[1]));
		BuildSide(position, Direction.ZY, new Vector2(dimension[2], dimension[1]));

		Vector3 topFacePosition = new Vector3(position.x, position.y + dimension[1] * lengthPerUnit, position.z);
		BuildSide(topFacePosition, Direction.XZ, new Vector2(dimension[0], dimension[2]));
		Vector3 backFacePosition = new Vector3(position.x, position.y, position.z + dimension[2] * lengthPerUnit);
		BuildSide(backFacePosition, Direction.XY, new Vector2(dimension[0], dimension[1]));
		Vector3 rightFacePosition = new Vector3(position.x + dimension[0] * lengthPerUnit, position.y, position.z);
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
						break;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
