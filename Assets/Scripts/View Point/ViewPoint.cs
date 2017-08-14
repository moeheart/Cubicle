using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewPoint : MonoBehaviour {

	public Camera cam;  
	private float timeHit;

	public Material chosenMaterial;
	public Material normalMaterial;

	public GameObject mainCam;
	private GameObject prePoint;

	private int selectedNum;
	private int trueNum;

	public Text resultText;


	void Start(){

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
						hit.collider.gameObject.GetComponent<Renderer>().material = chosenMaterial;

//						print(selectedNum);

						prePoint = hit.collider.gameObject;

					}catch{
					}
				}
					
			}

			if (Input.GetButtonDown ("Submit")) {
//				print (selectedNum == trueNum);
				if (!(selectedNum == trueNum))
					resultText.text = "Try Again!";
				else {
					resultText.text = "You Win!";
					DataUtil.UnlockCurrentRoom();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene("World Scene");
		}
	}  
}
