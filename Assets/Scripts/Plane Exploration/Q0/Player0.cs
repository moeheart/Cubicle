using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player0 : MonoBehaviour {

	public float speed;
	private float edge;

	Transform transform;
	Rigidbody rb;
	

	public Text winText;
	public GameObject panel, player, retryButton, isometricButton, target, tutObject;

	private int modelStage;


	void Start ()
	{
		modelStage = 1;

		transform = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody> ();
		edge = transform.localScale.x;

		winText.text = "";

	}

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		modelStage = tutObject.GetComponent<TutorialStage> ().modelStage;
		
		if (transform.position.x - speed * moveVertical >= edge / 2
			&& transform.position.x - speed * moveVertical <= 2 - edge / 2
			&& transform.position.z + speed * moveHorizontal >= edge / 2
			&& transform.position.z + speed * moveHorizontal <= 3 - edge / 2) {

				transform.position += speed * (new Vector3 (-moveVertical, 0, moveHorizontal));

				if(modelStage == 2 || modelStage == 3){
					if (transform.position.z >= 1 + edge && transform.position.z <= 2 - edge) {
						rb.useGravity = false;
						transform.position += speed * (new Vector3 (0, -moveHorizontal, 0));
						transform.eulerAngles = new Vector3 (45, 0, 0);
						} else
						rb.useGravity = true;
				}else if(modelStage == 4){
					if (transform.position.z >= 1 + edge && transform.position.z <= 2 - edge
						&& transform.position.x <= 1) {
						rb.useGravity = false;
						transform.position += speed * (new Vector3 (0, -moveHorizontal, 0));
						transform.eulerAngles = new Vector3 (45, 0, 0);
						} else
						rb.useGravity = true;
				}else{
					;
				}
						
		}

	}

}
