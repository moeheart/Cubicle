using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ViewPanel : MonoBehaviour {

	public GameObject solidLine;

	public GameObject dashedLine;

	private float panelDisplayScale = 0.7f; //E.g., if this were set to 1, then a full display would fill up the whole panel

	[SerializeField]
	private ViewType viewType;

	private List<GameObject> lines = new List<GameObject>();
	
	private IntVector2 blockSize; //E.g., the max display of this panel is 3 by 5
	private Vector2 panelSize; //E.g., the length is 300 by 500, equals blockSize * lengthPerBlock
	private int lengthPerBlock = BlockBuilderConfigs.panelLengthPerBlock;

	private Color defaultColor;
	private Color highlightColor = new Color(0,1,1,0.2f);

	private BaseGrid baseGridInstance;

	void Awake () {
		if (viewType == ViewType.TopView) {
			blockSize.x = BlockBuilderConfigs.gridSize.x;
			blockSize.z = BlockBuilderConfigs.gridSize.z;
		}

		if (viewType == ViewType.FrontView) {
			blockSize.x = BlockBuilderConfigs.gridSize.x;
			blockSize.z = BlockBuilderConfigs.maxHeight;
		}

		if (viewType == ViewType.RightView) {
			blockSize.x = BlockBuilderConfigs.gridSize.z;
			blockSize.z = BlockBuilderConfigs.maxHeight;
			//Debug.Log("panelSize" + panelSize);
		}

		panelSize.x = blockSize.x * lengthPerBlock;
		panelSize.y = blockSize.z * lengthPerBlock;



		this.GetComponent<RectTransform>().sizeDelta = new Vector2(panelSize.x, panelSize.y);

		defaultColor = this.GetComponent<Image>().color;
	}

	// Use this for initialization
	void Start () {
		baseGridInstance = BlockBuilderManager.baseGridInstance;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeColorOnCompare(bool flag) {
		if (flag == true) {
			this.GetComponent<Image>().color = highlightColor;
		}
		else {
			this.GetComponent<Image>().color = defaultColor;
		}
	}

	public void DrawView(Dictionary<Segment, LineType> lineMap) {
		foreach (GameObject line in lines) {
			Destroy(line);
		}
		foreach (KeyValuePair<Segment, LineType> entry in lineMap) {
			if (entry.Value == LineType.NoLine) {
				continue;
			}
			DrawSegment(entry.Key, entry.Value);
		}
	}
	
	//Draw a line on the panel
	private void DrawSegment(Segment segment, LineType lineType) {

		IntVector2 pointA = segment.p1;
		IntVector2 pointB = segment.p2;
		Vector2 startPosition = new Vector2(pointA.x * lengthPerBlock, pointA.z * lengthPerBlock);
		Vector2 endPosition = new Vector2(pointB.x * lengthPerBlock, pointB.z * lengthPerBlock);

		startPosition -= panelSize/2;
		endPosition -= panelSize/2;
		startPosition *= panelDisplayScale;
		endPosition *= panelDisplayScale;

		GameObject lineGameObject = null;
		if (lineType == LineType.SolidLine) {
			lineGameObject = Instantiate(solidLine) as GameObject;
		}
		else if (lineType == LineType.DashedLine) {
			lineGameObject = Instantiate(dashedLine) as GameObject;
		}
		lineGameObject.transform.SetParent(this.transform, false);
		LineRenderer lineRenderer = lineGameObject.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, startPosition);
		lineRenderer.SetPosition(1, endPosition);

		lines.Add(lineGameObject);

	}

	public void ChangeColorOnViewMatch() {
		Vector3 gridEulerAngles = BlockBuilderManager.baseGridInstance.transform.localEulerAngles;
		Vector3 deltaAngle;
		switch (viewType) {
			case ViewType.TopView:
				deltaAngle = new Vector3(
					Mathf.DeltaAngle(BlockBuilderConfigs.topViewAngle.x, gridEulerAngles.x),
					Mathf.DeltaAngle(BlockBuilderConfigs.topViewAngle.y, gridEulerAngles.y),
					Mathf.DeltaAngle(BlockBuilderConfigs.topViewAngle.z, gridEulerAngles.z)
				);
				SetColorBasedOnDeltaAngle(deltaAngle);
				break;
			case ViewType.FrontView:
				deltaAngle = new Vector3(
					Mathf.DeltaAngle(BlockBuilderConfigs.frontViewAngle.x, gridEulerAngles.x),
					Mathf.DeltaAngle(BlockBuilderConfigs.frontViewAngle.y, gridEulerAngles.y),
					Mathf.DeltaAngle(BlockBuilderConfigs.frontViewAngle.z, gridEulerAngles.z)
				);
				SetColorBasedOnDeltaAngle(deltaAngle);
				break;
			case ViewType.RightView:
				deltaAngle = new Vector3(
					Mathf.DeltaAngle(BlockBuilderConfigs.rightViewAngle.x, gridEulerAngles.x),
					Mathf.DeltaAngle(BlockBuilderConfigs.rightViewAngle.y, gridEulerAngles.y),
					Mathf.DeltaAngle(BlockBuilderConfigs.rightViewAngle.z, gridEulerAngles.z)
				);
				SetColorBasedOnDeltaAngle(deltaAngle);
				break;
		}
	}

	private void SetColorBasedOnDeltaAngle(Vector3 deltaAngle) {
		float mag = Mathf.Abs(deltaAngle.x) + Mathf.Abs(deltaAngle.y) + Mathf.Abs(deltaAngle.z);
		mag /= 540;
		Debug.Log(deltaAngle);
		if (mag > 0.08) {
			this.GetComponent<Image>().color = defaultColor;
		}
		else {
			this.GetComponent<Image>().color = new Color(mag, 1f, 1f);
		}

	}

}
