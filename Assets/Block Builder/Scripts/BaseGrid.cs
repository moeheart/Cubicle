using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class BaseGrid : MonoBehaviour {

	public BaseGridCell cellPrefab;
	public float generationStepDelay = 0.01f;

	private IntVector2 size = BlockBuilderConfigs.gridSize;
	private BaseGridCell[,] cells;
	private const float cellLength = 1f;
	private const float cellHeight = 0.1f;
	private IntVector2 currentCoordinates = new IntVector2(0,0);

	private GameObject DrawingHandler;
	private bool isCompleted = false;
	private int id;

	private GameObject selectGridControlPanel;

	private GameObject increaseDecreasePanel;

	private Button up, left, right, down;

	private Button inc, dec;

	public static float startTime {get; private set;}

	// Use this for initialization
	void Start () {
		// TODO
		id = BlockBuilderManager.currentLevelId;
		DrawingHandler = GameObject.Find("Drawing Handler");
		selectGridControlPanel = GameObject.Find("Select Grid Control Panel");
		increaseDecreasePanel = GameObject.Find("Right Panel");
		
		up = selectGridControlPanel.transform.GetChild(0).GetComponent<Button>();
		left = selectGridControlPanel.transform.GetChild(1).GetComponent<Button>();
		right = selectGridControlPanel.transform.GetChild(2).GetComponent<Button>();
		down = selectGridControlPanel.transform.GetChild(3).GetComponent<Button>();
		inc = increaseDecreasePanel.transform.GetChild(0).GetComponent<Button>();
		dec = increaseDecreasePanel.transform.GetChild(1).GetComponent<Button>();

		up.onClick.AddListener(OnUpClick);
		left.onClick.AddListener(OnLeftClick);
		right.onClick.AddListener(OnRightClick);
		down.onClick.AddListener(OnDownClick);
		inc.onClick.AddListener(OnIncreaseClick);
		dec.onClick.AddListener(OnDecreaseClick);
		

		startTime = Time.time;
		BlockBuilderLog.Log(id, "Entered Level...!!!");
	}
	
	// Update is called once per frame
	void Update () {
		//TODO
		//Should we move this somewhere else?
		//Perhaps a separate script named InputHandler.cs...?

		//The clamp to ensure the new coord is inside the grid is handled inside ChangeCurrentCoordinates mthd
		if (isCompleted == true) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			IntVector2 newCoordinates = currentCoordinates;
			newCoordinates.z++;
			ChangeCurrentCoordinates(newCoordinates);
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			IntVector2 newCoordinates = currentCoordinates;
			newCoordinates.z--;
			ChangeCurrentCoordinates(newCoordinates);
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			IntVector2 newCoordinates = currentCoordinates;
			newCoordinates.x++;
			ChangeCurrentCoordinates(newCoordinates);
		}
		else if (Input.GetKeyDown(KeyCode.A)){
			IntVector2 newCoordinates = currentCoordinates;
			newCoordinates.x--;
			ChangeCurrentCoordinates(newCoordinates);
		}
		else if (Input.GetKeyDown(KeyCode.R)) {
			GenerateLog(1, currentCoordinates);
			AddCubeToCoordinate(currentCoordinates);
		}
		else if (Input.GetKeyDown(KeyCode.F)) {
			GenerateLog(-1, currentCoordinates);
			DeleteCubeFromCoordinate(currentCoordinates);
		}
	}

	private void GenerateLog(int op, IntVector2 currentCoordinates) {
		BaseGridCell designatedCell = 
			cells[currentCoordinates.x, currentCoordinates.z];
		int heightBeforeOp = designatedCell.height;
//		Debug.Log("DrawingHandler", DrawingHandler);
		int[,] target = BlockBuilderManager.height;
		int targetHeight = target[currentCoordinates.x, currentCoordinates.z];
		if (op == 1) {
			if (heightBeforeOp < targetHeight) {
				BlockBuilderLog.Log(id, "Correct Addition");
			}
			else {
				BlockBuilderLog.Log(id, "Incorrect Addition");
			}
		}
		else {
			if (heightBeforeOp > targetHeight) {
				BlockBuilderLog.Log(id, "Correct Deletion");
			}
			else {
				BlockBuilderLog.Log(id, "Incorrect Deletion");
			}
		}
	}

	public void Generate() {
		cells = new BaseGridCell[size.x, size.z];
		for (int x = 0; x < size.x; ++x) {
			for (int z = 0; z < size.z; ++z) {
				CreateCell(new IntVector2(x,z));
			}
		}
		HighlightCell(currentCoordinates);
	}

	public void OnCompleteBlockBuilderPuzzle() {
		isCompleted = true;
		BlockBuilderLog.Log(id, "Completed Level...!!!");
		UnhighlightCell(currentCoordinates);
	}

	private void CreateCell(IntVector2 coordinates) {
		BaseGridCell newCell = Instantiate (cellPrefab) as BaseGridCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = this.transform;
		newCell.transform.localPosition = new Vector3(
			coordinates.x - size.x * cellLength/2 + cellLength/2, 
			-cellHeight/2, 
			coordinates.z - size.z * cellLength/2 + cellLength/2);
	}

	public void GenerateCells() {
		cells = new BaseGridCell[size.x, size.z];
		for (int x = 0; x < size.x; ++x) {
			for (int z = 0; z < size.z; ++z) {
				CreateCell(new IntVector2(x,z));
			}
		}
		HighlightCell(currentCoordinates);
	}


	//Simply changes the color of the designated cell
	private void HighlightCell(IntVector2 coordinates) {
		BaseGridCell designatedCell = cells[coordinates.x, coordinates.z];
		designatedCell.Highlight();
	}

	//Simply reverts the highlighted color of the cell
	private void UnhighlightCell(IntVector2 coordinates) {
		BaseGridCell designatedCell = cells[coordinates.x, coordinates.z];
		designatedCell.Unhighlight();
	}

	private void ChangeCurrentCoordinates(IntVector2 newCoordinates) {
		newCoordinates = ClampCoordinates(newCoordinates);
		if (newCoordinates != currentCoordinates) {
			UnhighlightCell(currentCoordinates);
			HighlightCell(newCoordinates);
			currentCoordinates = newCoordinates;
		}
	}

	private IntVector2 ClampCoordinates(IntVector2 newCoordinates) {
		if (newCoordinates.x < 0) {
			newCoordinates.x = 0;
		}
		if (newCoordinates.x >= size.x) {
			newCoordinates.x = size.x - 1;
		}
		if (newCoordinates.z < 0) {
			newCoordinates.z = 0;
		}
		if (newCoordinates.z >= size.z) {
			newCoordinates.z = size.z - 1;
		}
		return newCoordinates;
	}

	private void AddCubeToCoordinate(IntVector2 coordinates) {
		BaseGridCell designatedCell = cells[coordinates.x, coordinates.z];
		designatedCell.AddCube();

		DrawingHandler.GetComponent<DrawingHandler>().DrawMultiView(cells);

		//
		//Should probably move this to some different script that handles the drawing
		//Convert it into a 3D boolean array
		/*Dictionary<Segment, LineType> topViewMap = ThreeView.GetTopView(cells);
		GameObject currentTopViewPanel = GameObject.Find("Current Top View Panel");
		currentTopViewPanel.GetComponent<ViewPanel>().DrawView(topViewMap);*/
	}

	private void DeleteCubeFromCoordinate(IntVector2 coordinates) {
		BaseGridCell designatedCell = cells[coordinates.x, coordinates.z];
		designatedCell.DeleteCube();

		DrawingHandler.GetComponent<DrawingHandler>().DrawMultiView(cells);
	}

	private void OnUpClick() {
		IntVector2 newCoordinates = currentCoordinates;
		newCoordinates.z++;
		ChangeCurrentCoordinates(newCoordinates);
	}

	private void OnLeftClick() {
		IntVector2 newCoordinates = currentCoordinates;
		newCoordinates.x--;
		ChangeCurrentCoordinates(newCoordinates);
	}

	private void OnRightClick() {
		IntVector2 newCoordinates = currentCoordinates;
		newCoordinates.x++;
		ChangeCurrentCoordinates(newCoordinates);
	}

	private void OnDownClick() {
		IntVector2 newCoordinates = currentCoordinates;
		newCoordinates.z--;
		ChangeCurrentCoordinates(newCoordinates);
	}

	private void OnIncreaseClick(){
		if (isCompleted) {
			return;
		}
		var cc = currentCoordinates;
		GenerateLog(1, cc);
		AddCubeToCoordinate(cc);
	}

	private void OnDecreaseClick() {
		if (isCompleted) {
			return;
		}
		var cc = currentCoordinates;
		GenerateLog(-1, cc);
		DeleteCubeFromCoordinate(cc);
	}

	private void OnClickExit() {

	}
}
