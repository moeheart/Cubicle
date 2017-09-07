using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewPoint : MonoBehaviour {

	public Camera cam;
	public GameObject topCam;
	private float timeHit;

	public Material chosenMaterial;
	public Material normalMaterial;

	public GameObject mainCam;
	private GameObject prePoint;

	private int selectedNum;
	private int trueNum;
	private float wrongTime;

	public Text resultText;

	public GameObject logObject;
	public GameObject solidsObject;
	public GameObject exitButton;

	private GameObject curPoint;

	void Start(){

		wrongTime = 0;
		selectedNum = -1;
		timeHit = 0.0f;
		trueNum = mainCam.GetComponent<ViewPointCameraController> ().camDir;

	}


	void Update()  
	{  

		timeHit += Time.deltaTime;  
		if (timeHit > 0.2f)  
		{  
			if (Input.GetMouseButton(0))  
			{  
				timeHit = 0f;  
				RaycastHit hit;
				bool isHit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100f);  

				// debug
				Vector3 lineOrigin = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
				Debug.DrawRay (lineOrigin, cam.transform.forward * 20, Color.green);

				if (isHit)  
				{  
					//					print (hit.collider.gameObject.name.Substring(5));
					try{

						if(prePoint)
							prePoint.GetComponent<Renderer>().material = normalMaterial;

						selectedNum = int.Parse(hit.collider.gameObject.name.Substring(5));
						curPoint = hit.collider.gameObject;
						curPoint.GetComponent<Renderer>().material = chosenMaterial;

						//						print(selectedNum);

						prePoint = hit.collider.gameObject;
						print(selectedNum);

					}catch{
					}
				}

			}

			if (Input.GetKeyDown(KeyCode.Return)) {
				//				print (selectedNum == trueNum);
				logObject.GetComponent<ViewPointLog> ().RecordResult (selectedNum, selectedNum == trueNum);
				if (!(selectedNum == trueNum)) {
					resultText.text = "Try Again!";
					wrongTime += 1;
					if (curPoint != null) {
						curPoint.GetComponent<Renderer> ().material = normalMaterial;
						curPoint = null;
						int rot = UnityEngine.Random.Range (1, 8);
						Vector3 curAngle = topCam.transform.eulerAngles;
						topCam.transform.eulerAngles = 
							new Vector3 (curAngle.x, curAngle.y + rot * 45, curAngle.z);
					}
					solidsObject.GetComponent<Generation> ().InitializeRecord ();
				}
				else {
					int level = solidsObject.GetComponent<Generation> ().level;
					int levelNum = solidsObject.GetComponent<Generation> ().levelNum;
					if (level < levelNum - 1) {
						resultText.text = "Correct! Try Next!";
						mainCam.GetComponent<ViewPointCameraController> ().GenerateDir ();
						solidsObject.GetComponent<Generation> ().level++;
						solidsObject.GetComponent<Generation> ().Initialize ();
						trueNum = mainCam.GetComponent<ViewPointCameraController> ().camDir;
						if (curPoint != null)
							curPoint.GetComponent<Renderer> ().material = normalMaterial;
					} else {
						double pScore = Math.Pow ((100.0f - wrongTime) / 100.0f, 5);
						double fScore = Math.Round (pScore * 100.0f, 0);
						resultText.text = "Completed! Your score: " + fScore.ToString();
						exitButton.SetActive (true);
						DataUtil.UnlockCurrentRoom ();
					}
				}

			}
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}
}
