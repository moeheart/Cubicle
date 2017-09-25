using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour {
	public float radius = 1.5f;

	private GameObject hintPanel;
	private bool canOperate;

	// Use this for initialization
	void Start () {
		hintPanel = GameObject.Find("Hint Panel");
	}
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		canOperate = false;
		foreach (Collider hitCollider in hitColliders) {
			Vector3 direction = hitCollider.transform.position - this.transform.position;
			if (Vector3.Dot(transform.forward, direction) > .5f) {
				if (hitCollider.gameObject.GetComponent<TriggerDevice>() != null) {
					canOperate = true;
					hintPanel.SetActive(true);
				}
			}
		}
		if (canOperate == false) {
			hintPanel.SetActive(false);
		}
		if (canOperate && Input.GetKeyDown(KeyCode.E)) {
			foreach (Collider hitCollider in hitColliders) {
				Vector3 direction = hitCollider.transform.position - this.transform.position;
				if (Vector3.Dot(transform.forward, direction) > .5f) {
					if (hitCollider.gameObject.GetComponent<TriggerDevice>() != null) {
						hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
	}
}
