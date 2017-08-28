using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2: MonoBehaviour {

	public float speed;
	private float edge;

	Transform transform;
	Rigidbody rb;

	public bool top1, top2, right1;

	public Text winText;
	public Text instructionText;
	public GameObject panel;
	public GameObject player;
	public GameObject retryButton;
	public GameObject isometricButton;
	public GameObject nextButton;

	public GameObject logObject;
	public GameObject target;

	void Start ()
	{
		transform = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody> ();
		edge = transform.localScale.x;

		top1 = false;
		top2 = false;
		right1 = false;

		winText.text = "";
	}

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (moveHorizontal != 0 || moveVertical != 0)
			logObject.GetComponent<PlaneExplorationLog> ().LogDetail (-moveVertical * speed, 
				moveHorizontal * speed, transform.position.ToString ());

		if (transform.position.x - speed * moveVertical >= edge / 2
		    && transform.position.x - speed * moveVertical <= 2 - edge / 2
		    && transform.position.z + speed * moveHorizontal >= edge / 2
		    && transform.position.z + speed * moveHorizontal <= 3 - edge / 2) {
			transform.position += speed * (new Vector3 (-moveVertical, 0, moveHorizontal));
			if ((transform.position.x <= 1 && transform.position.z >= 1 + edge)
			    || (transform.position.x > 1 && transform.position.z >= 2 + edge)) {
				rb.useGravity = false;
				transform.position += speed * (new Vector3 (0, -moveHorizontal, 0));
				transform.eulerAngles = new Vector3 (45, 0, 0);
			} else
				rb.useGravity = true;
		}


	}

	public void Win(){
		panel.SetActive (true);
		winText.text = "You Win!";
		instructionText.text = "";
		player.SetActive (false);
		isometricButton.SetActive (true);
		nextButton.SetActive (true);
		target.SetActive (false);
		DataUtil.UnlockCurrentRoom();
		logObject.GetComponent<PlaneExplorationLog> ().RecordResult (1);
	}

}
