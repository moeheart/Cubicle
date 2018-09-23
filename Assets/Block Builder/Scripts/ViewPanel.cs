using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ViewPanel : MonoBehaviour, IPointerClickHandler {

	public GameObject solidLine;

	public GameObject dashedLine;

	private float panelDisplayScale = 0.7f; //E.g., if this were set to 1, then a full display would fill up the whole panel

	[SerializeField]
	private ViewType viewType;

	private List<GameObject> lines = new List<GameObject>();
	
	private IntVector2 blockSize; //E.g., the max display of this panel is 3 by 5
	private Vector2 panelSize; //E.g., the length is 300 by 500, equals blockSize * lengthPerBlock
	private int lengthPerBlock = BlockBuilderConfigs.panelLengthPerBlock;

	private Color defaultColor = new Color(1f, 1f, 1f, 1f);
	private Color highlightColor = new Color(0f ,1f ,1f, 0.2f);
	private Color viewMatchColor = new Color(0.2f, 1f, 1f);

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

		this.GetComponent<Image>().color = defaultColor;
	}

	// Use this for initialization
	void Start () {
		baseGridInstance = BlockBuilderManager.baseGridInstance;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnPointerClick(PointerEventData eventData) {
		Camera.main.GetComponent<RotateCameraUsingGyro>().SwitchToView(viewType);
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
		lineGameObject.transform.localPosition = new Vector3(0,0,-1);
		LineRenderer lineRenderer = lineGameObject.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, startPosition);
		lineRenderer.SetPosition(1, endPosition);

		lines.Add(lineGameObject);

	}

	public void ChangeColorOnViewMatch() {
		Transform cameraTransform = Camera.main.transform;
		switch (viewType) {
			case ViewType.TopView:
				SetColorOnAlignWithWorldXYZ(
					cameraTransform.right, -cameraTransform.forward, cameraTransform.up);
				break;
			case ViewType.FrontView:
				SetColorOnAlignWithWorldXYZ(
					cameraTransform.right, cameraTransform.up, cameraTransform.forward);
				break;
			case ViewType.RightView:
				SetColorOnAlignWithWorldXYZ(
					-cameraTransform.forward, cameraTransform.up, cameraTransform.right);
				break;
		}
	}

	private void SetColorOnAlignWithWorldXYZ(Vector3 a, Vector3 b, Vector3 c) {
		float cos1 = Vector3.Dot(a, Vector3.right);
		float cos2 = Vector3.Dot(b, Vector3.up);
		float cos3 = Vector3.Dot(c, Vector3.forward);
		bool isAligned =  (cos1 > BlockBuilderConfigs.cosineSimilarityLowerBound
				&& cos2 > BlockBuilderConfigs.cosineSimilarityLowerBound
				&& cos3 > BlockBuilderConfigs.cosineSimilarityLowerBound);
		if (isAligned) {
			this.GetComponent<Image>().color = viewMatchColor;
		}
		else {
			this.GetComponent<Image>().color = defaultColor;
		}

	}
}
