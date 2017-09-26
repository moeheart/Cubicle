using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AxisDrawing: ResponseProcessing {

	private LineRenderer line;
	private bool isLineInstantiated;
	private int vertexCount;
	public GameObject linePrefab;

	protected List<Vector3> linePath;
	public static string pathStringToLog;

	private float alphaScale;

	private Vector3 mousePos;

	protected float sectionScale;

	public static List<Section> sections;

	public static string lastGradingResult; 

	void Awake(){
		lastGradingResult = "";
		isLineInstantiated = false;
		InitSections ();
		linePath = new List<Vector3> ();
		if (RevSolidGameInfo.levelOfDifficulty <= 1) { 
			//RevSolidGameInfo class might not have awaken, be careful when calling its functions
			sectionScale = 0.5f/resizedScale;
		}else if(RevSolidGameInfo.levelOfDifficulty <= 2){
			sectionScale = 0.5f/resizedScale;
		}
	}

	void OnEnable(){
		EventManager.StartListening ("OnMouseDown",OnMouseDown);
		EventManager.StartListening ("OnMouseUp",OnMouseUp);
		EventManager.StartListening ("Grading",GradingResToLog);
	}

	void OnDisable(){
		EventManager.StopListening ("OnMouseDown",OnMouseDown);
		EventManager.StopListening ("OnMouseUp",OnMouseUp);
		EventManager.StopListening ("Grading",GradingResToLog);
	}
	// Use this for initialization
	void Start () {
		linePrefab = Resources.Load ("linePrefab") as GameObject;
		StartCoroutine("RecordLinePath");

	}

	
	// Update is called once per frame
	void Update () {
		OnResize ();

		GetMousePosRelative2Screen ();
		if (!Tutorial.isInstructionPanelEnabled) {
			FreeStrokeDrawingAndGrading ();
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
			EventManager.TriggerEvent("OnMouseDown");
		}

		if (Input.GetMouseButton (0)) {
			OnGetMouse ();
		}

		if (Input.GetMouseButtonUp (0)) {
			EventManager.TriggerEvent("OnMouseUp");
		}

		if (isLineInstantiated) {
			AxisFadeOut ();
		}
	}

	void OnMouseDown(){
		if (isLineInstantiated==false) {
			InitAxis ();
		}
	}

	void OnGetMouse(){
		DrawAxis ();
	}

	void OnMouseUp(){
		if (isLineInstantiated&&linePath.Count>0) {
			DisplayScore (Grading(linePath));
			DestroyFreeStroke ();
		}
	}

	void DestroyFreeStroke(){
		pathStringToLog = GetPathString (linePath);
		DestroyAxisObject ();
		linePath.Clear ();
	}

	string GetPathString(List<Vector3> linePath){
		string pathString="";
		for (int i = 0; i < linePath.Count; i++) {
			pathString += linePath [i].x.ToString ();
			pathString += " ";
			pathString += linePath [i].y.ToString ();
			pathString += ",";
		}
		return pathString;
	}


	void DestroyAxisObject(){
		GameObject.Destroy (line.gameObject);
		isLineInstantiated = false;
	}

	void InitAxis(){ 
		line = GameObject.Instantiate (linePrefab, linePrefab.transform.position, transform.rotation).GetComponent<LineRenderer> ();
		line.startWidth = 0.05f;
		line.endWidth = 0.15f;
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
			RevSolidUIControl.BroadcastMsg ("fantastic!");
			break;
		case 0:
			RevSolidUIControl.BroadcastMsg("left edge is not the correct axis");
			break;
		case 1:
			RevSolidUIControl.BroadcastMsg("right edge is not the correct axis");
			break;
		case 2:
			RevSolidUIControl.BroadcastMsg("bottom edge is not the correct axis");
			break;
		case 3:
			RevSolidUIControl.BroadcastMsg("top edge is not the correct axis");
			break;
		case 4:
			RevSolidUIControl.BroadcastMsg("diagonal / is not the correct axis");
			break;
		case 5:
			RevSolidUIControl.BroadcastMsg("diagonal \\ is not the correct axis");
			break;
		default:
			RevSolidUIControl.BroadcastMsg("try again!");
			break;
		}
	}

	public static Sprite[] polygonSprites=new Sprite[RevSolidGameInfo.MaxPolygonNum];
	void InitSections(){
		sections = new List<Section> ();//constructor
		for(int i=0;i<RevSolidGameInfo.MaxPolygonNum;i++){
			sections.Add(new Section(i,i));//bottomleft be origin
		}
	}

	public static void ReloadSectionsWithCandidateAxes(){
		for(int i=0;i<RevSolidGameInfo.MaxPolygonNum;i++){
			sections[i].imgSprite=Resources.Load<Sprite> ("section"+i.ToString()+"_a");
		}
		for(int i=0;i<RevSolidGameInfo.MaxPanelNum;i++){
			ActiveObjControl.activeObjects[i].ChangeSpriteAccordingToSolid();
		}
	}

	public static void RecoverOriginalSections(){
		for(int i=0;i<RevSolidGameInfo.MaxPolygonNum;i++){
			sections[i].imgSprite=Resources.Load<Sprite> ("section"+i.ToString());
		}
		for(int i=0;i<RevSolidGameInfo.MaxPanelNum;i++){
			ActiveObjControl.activeObjects[i].ChangeSpriteAccordingToSolid();
		}
	}

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

		for (int i = 0; i < 6; i ++) {
			tempConvolution = Convolution(pathKernel,Section.candKernels[i]);
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
			Vector3 panelPosRelativeToScreen = ActiveObjControl.activeObjects [panelIndex].image.transform.position + new Vector3 (-wx, -wy, 0);
			x =(path[i].x-panelPosRelativeToScreen.x)/sectionScale+ Section.imgRes/2;
			y =(path[i].y-panelPosRelativeToScreen.y)/sectionScale+ Section.imgRes/2;
			//Debug.Log ((path[i].x-panelPosRelativeToScreen.x)/0.2f);
			//Debug.Log ((path[i].y-panelPosRelativeToScreen.y)/0.2f);
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

		//sectionIndex
		int panelIndex=IdentifyPanelIndexOfStroke(mid);
		//Debug.Log (panelIndex);
		int bestMatchCandNo=-2;
		if (ActiveObjControl.activeObjects [panelIndex].isKilled == false) {
			bestMatchCandNo = BestMatchCandidate (path, panelIndex);
			if (bestMatchCandNo == -1) {
				RevSolidGameInfo.Add2TotalHit (1);
				RevSolidUIControl.BroadcastHits ();

				DisableFormerQuestion(panelIndex);
				this.GetComponent<RevSolidUIControl>().ShowCheckMark (panelIndex);
			} else {
				RevSolidGameInfo.Add2FalseStrokeCount (1);
				RevSolidUIControl.BroadcastFalseStrokeCount ();
				Tutorial.IndicateCorrectAns (panelIndex);
			}
		}

		string bmcName;
		switch(bestMatchCandNo) {
		case -1:
			bmcName = "correct";
			break;
		case 0:
			bmcName = "left edge";
			break;
		case 1:
			bmcName = "right edge";
			break;
		case 2:
			bmcName = "bottom edge";
			break;
		case 3:
			bmcName = "top edge";
			break;
		case 4:
			bmcName = "diagonal /";
			break;
		case 5:
			bmcName = "diagonal \\";
			break;
		default:
			bmcName = "no match";
			break;

		}

		GetGradingResult(ActiveObjControl.activeObjects[panelIndex].polygonIndex,panelIndex,bmcName);
		EventManager.TriggerEvent("Grading");
		return bestMatchCandNo;
	}

	void GetGradingResult(int solidIndex,int panelIndex,string bmcName){
		lastGradingResult="";
		lastGradingResult += lastGradingResult.ToString ();
		lastGradingResult += " for solid ";
		lastGradingResult += solidIndex;
		lastGradingResult+=" on panelNo.";
		lastGradingResult+= panelIndex;
		lastGradingResult+= " is ";
		lastGradingResult+= bmcName;
	}

	void GradingResToLog(){
		
	}

	void DisableFormerQuestion(int panelIndex){
		ActiveObjControl.activeObjects [sections [panelIndex].polygonIndex].isKilled = true;
		StartCoroutine (ShowAnswerAndDisableFormerQuestion(panelIndex));
	}

	IEnumerator ShowAnswerAndDisableFormerQuestion(int panelIndex){
		//yield return new WaitForSeconds (1.0f);
		//this.GetComponent<Tutorial>().IndicateAxisAndStroke (panelIndex);

		//yield return new WaitForSeconds (0.5f);
		yield return null;
		ActiveObjControl.activeObjects [sections [panelIndex].polygonIndex].image.gameObject.SetActive (false);

		Tutorial.CancelAnsIndication ();
		yield return new WaitForSeconds (RevSolidGameInfo.RecoverInterval);
		this.GetComponent<ActiveObjControl>().RecoverObjects();
	}


	int IdentifyPanelIndexOfStroke(Vector3 mid){
		int panelIndex=0;

		if (RevSolidGameInfo.GetLODByInt() == 2) {
			//panel position 0-UR,1-UL,2-BL,3-BR
			if (mid.x > -thres && mid.y > -thres) {
				panelIndex = 0;
			} else if (mid.x > -thres && mid.y < thres) {
				panelIndex = 3;
			} else if (mid.x < thres && mid.y > -thres) {
				panelIndex = 1;
			} else if (mid.x < thres && mid.y < thres) {
				panelIndex = 2;
			}
		}
		return panelIndex;
	}
		
}
	