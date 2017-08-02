using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ViewPanel : MonoBehaviour {

	public GameObject solidLine;

	[SerializeField]
	private float panelDisplayScale = 0.8f; //E.g., if this were set to 1, then a full display would fill up the whole panel

	[SerializeField]
	private ViewType viewType;

	private List<GameObject> lines = new List<GameObject>();
	
	private IntVector2 blockSize; //E.g., the max display of this panel is 3 by 5
	private Vector2 panelSize; //E.g., the length is 300 by 500, equals blockSize * lengthPerBlock
	private int lengthPerBlock = Configuration.panelLengthPerBlock;

	private Color defaultColor;
	private Color highlightColor = new Color(0,1,1,0.2f);
	void Awake () {
		if (viewType == ViewType.TopView) {
			blockSize.x = Configuration.gridSize.x;
			blockSize.z = Configuration.gridSize.z;
		}

		if (viewType == ViewType.FrontView) {
			blockSize.x = Configuration.gridSize.x;
			blockSize.z = Configuration.maxHeight;
		}

		if (viewType == ViewType.RightView) {
			blockSize.x = Configuration.gridSize.z;
			blockSize.z = Configuration.maxHeight;
			//Debug.Log("panelSize" + panelSize);
		}

		panelSize.x = blockSize.x * lengthPerBlock;
		panelSize.y = blockSize.z * lengthPerBlock;



		this.GetComponent<RectTransform>().sizeDelta = new Vector2(panelSize.x, panelSize.y);

		defaultColor = this.GetComponent<Image>().color;
	}

	// Use this for initialization
	void Start () {

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

		GameObject lineGameObject = Instantiate(solidLine) as GameObject;
		lineGameObject.transform.SetParent(this.transform, false);
		LineRenderer lineRenderer = lineGameObject.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, startPosition);
		lineRenderer.SetPosition(1, endPosition);

		if (lineType == LineType.DashedLine) {
			float distance = Vector3.Distance(startPosition, endPosition);
			lineRenderer.material.mainTextureScale = new Vector2(distance, 1);
		}

		lines.Add(lineGameObject);

	}

}
