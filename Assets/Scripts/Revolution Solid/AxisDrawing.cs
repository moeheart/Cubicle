using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisDrawing: MonoBehaviour {

	private LineRenderer line;
	private bool isLineInstantiated;
	private int vertexCount;
	public GameObject linePrefab;

	protected List<Vector3> linePath;

	private float alphaScale;

	private Vector3 mousePos;
	protected float wx, wy;
	protected float sectionScale=0.2f;//depend on the gameObject empty

	public static List<Section> sections;

	void Awake(){

		wx = Camera.main.pixelRect.center.x;
		wy = Camera.main.pixelRect.center.y;

		isLineInstantiated = false;
		InitSections ();
		linePath = new List<Vector3> ();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine("RecordLinePath");
	}

	
	// Update is called once per frame
	void Update () {
		
		OnResize ();

		GetMousePosRelative2Screen ();

		FreeStrokeDrawingAndGrading ();

		RevSolidGameInfo.CheckEndOfGame ();
	}

	void OnResize(){
		if (wx != Camera.main.pixelRect.center.x || wy != Camera.main.pixelRect.center.y) {
			wx = Camera.main.pixelRect.center.x;
			wy = Camera.main.pixelRect.center.y;
		}
	}

	void GetMousePosRelative2Screen(){
		mousePos = Input.mousePosition;
		mousePos.x =mousePos.x- wx;
		mousePos.y =mousePos.y- wy;
	}

	void FreeStrokeDrawingAndGrading(){
		if (Input.GetMouseButtonDown (0)) {
			//destroy existing one and instantiate new

			if (isLineInstantiated==false) {
				InitAxis ();
			}
		}

		if (Input.GetMouseButton (0)) {
			DrawAxis ();
		}

		if (Input.GetMouseButtonUp (0)) {
			if (isLineInstantiated&&linePath.Count>0) {
				DisplayScore (Grading(linePath));
				DestroyAxis ();
				linePath.Clear ();
			}
		}

		if (isLineInstantiated) {
			AxisFadeOut ();
		}
	}

	void DestroyAxis(){
		GameObject.Destroy (line.gameObject);
		isLineInstantiated = false;
	}

	void InitAxis(){ 
		line = GameObject.Instantiate (linePrefab, linePrefab.transform.position, transform.rotation).GetComponent<LineRenderer> ();
		line.SetWidth (0.05f, 0.15f);
		vertexCount = 0;
		alphaScale = 1.0f;
		isLineInstantiated = true;
	}

	void DrawAxis(){
		//if (mousePos.x-wx && mousePos.y-wy) 
		vertexCount++;
		line.SetVertexCount (vertexCount);
		line.SetPosition (vertexCount - 1, Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x+wx, mousePos.y+wy, 0)));
	}

	void AxisFadeOut(){
		line.material.SetFloat ("_AlphaScale",Mathf.Clamp(alphaScale-=0.05f,0.0f,1.0f));
	}

	IEnumerator RecordLinePath(){
		while (true) {
			if (Input.GetMouseButton (0)&&isLineInstantiated==true) {
				linePath.Add (mousePos);
			}
			yield return new WaitForSeconds (0.001f);
		}
	}

	void DisplayScore(int bestMatchCand){
		switch(bestMatchCand) {
		case -1:
			RevSolidUIControl.BroadcastMessage("fantastic!");
			break;
		case 0:
			RevSolidUIControl.BroadcastMessage("left edge is not the correct axis");
			break;
		case 1:
			RevSolidUIControl.BroadcastMessage("right edge is not the correct axis");
			break;
		case 2:
			RevSolidUIControl.BroadcastMessage("bottom edge is not the correct axis");
			break;
		case 3:
			RevSolidUIControl.BroadcastMessage("top edge is not the correct axis");
			break;
		case 4:
			RevSolidUIControl.BroadcastMessage("diagonal / is not the correct axis");
			break;
		case 5:
			RevSolidUIControl.BroadcastMessage("diagonal \\ is not the correct axis");
			break;
		default:
			RevSolidUIControl.BroadcastMessage("try again!");
			break;
		}
	}

	public static Sprite[] polygonSprites=new Sprite[ActiveObjControl.MaxPolygonNum];
	void InitSections(){
		sections = new List<Section> ();//constructor
		for(int i=0;i<ActiveObjControl.MaxPolygonNum;i++){
			sections.Add(new Section(i,i));//bottomleft be origin
		}
	}


	//Grading methods


	public int BestMatchCandidate (List<Vector3> path,int panelIndex){
		int bestMatchCandidateNo=-2;//refer to the correct one
		int tempConvolution=0;

		int[] pathKernel =new int[36];
		Pixel2Kernel (path,panelIndex,pathKernel);

		int k=ActiveObjControl.activeObjects[panelIndex].polygonIndex;
		int maxConvolution = Convolution (pathKernel, Section.axisKernels[k]);
		if (maxConvolution >= 3) {
			bestMatchCandidateNo = -1;
		}
		/*
		Debug.Log ("-1");
		Debug.Log (maxConvolution);
		*/

		for (int i = 0; i < 6; i ++) {
			tempConvolution = Convolution(pathKernel,Section.candKernels[i]);
			/*
Debug.Log (i);
			Debug.Log (tempConvolution);
			*/

			if (tempConvolution > maxConvolution) {
				if (maxConvolution >= 1) {
					maxConvolution = tempConvolution;
					bestMatchCandidateNo = i;
				}
			}
		}
		return bestMatchCandidateNo;
	}

	void Pixel2Kernel(List<Vector3>path, int panelIndex,int[] pathKernel){
		for (int i=0;i<36;i++) {
			pathKernel [i] = 0;
		}

		float x, y;

		//path coordinate is in pixel relative to screen center
		//transfrom into 550x550 resolution
			
		for (int i = 0; i < path.Count; i++) {

			x =(path[i].x-(ActiveObjControl.activeObjects [panelIndex].image.transform.position.x-wx))/sectionScale+ Section.imgRes/2;
			y =(path[i].y-(ActiveObjControl.activeObjects [panelIndex].image.transform.position.y-wy))/sectionScale+ Section.imgRes/2;
			x = Mathf.FloorToInt(x / 100);
			y = Mathf.FloorToInt(y / 100);

			if (x >= 0 && x<6&& y >= 0&&y<6) {//for cases where mousePos is out of image
				pathKernel [(int)(y * 6 + x)] = 1;
			}
		}
		
	}

	int Convolution(int[] kernel1,int[] kernel2){
		int sum=0;
		for (int i=0;i<36;i++) {
			sum += kernel1 [i] * kernel2 [i];
		}
		return sum;
	}


	//Output grading result

	const float thres=0.0f;
	public int Grading(List<Vector3> path){
		//calculate start/end vertices & slope

		Vector3 start=path[0];
		Vector3 end=path[path.Count-1];
		Vector3 mid = path [path.Count/2];

		if (Vector3.Distance (start, end) < 50) {
			return -2;
		}

		int panelIndex=0;//sectionIndex
		//panel position 0-UR,1-UL,2-BL,3-BR
		if (mid.x > -thres && mid.y > -thres) {
			panelIndex = 0;
		} else if (mid.x > -thres && mid.y < thres) {
			panelIndex = 3;
		}else if (mid.x < thres && mid.y > -thres) {
			panelIndex = 1;
		}else if (mid.x < thres && mid.y < thres) {
			panelIndex = 2;
		}
		//Debug.Log (panelIndex);
		int bestMatchCandNo=-2;
		if (ActiveObjControl.activeObjects [sections[panelIndex].polygonIndex].isKilled == false) {
			bestMatchCandNo = BestMatchCandidate (path, panelIndex);
			if (bestMatchCandNo == -1) {
				ActiveObjControl.activeObjects [sections [panelIndex].polygonIndex].isKilled = true;
				ActiveObjControl.activeObjects [sections [panelIndex].polygonIndex].image.gameObject.SetActive (false);
				RevSolidGameInfo.Add2TotalHit (1);
				RevSolidUIControl.BroadcastHits ();
			} else {
				RevSolidGameInfo.Add2FalseStrokeCount (1);
				RevSolidUIControl.BroadcastFalseStrokeCount ();
				Tutorial.IndicateCorrectAns (panelIndex);
			}
		}
		return bestMatchCandNo;
	}
		
}
	