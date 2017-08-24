using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewCameraMovement : MonoBehaviour {
    public GameObject center;
    public float rotSpeed;
    private Transform CenterTransform;

    void Start()
    {
        CenterTransform = center.transform;
    }

    void Update()
    {

        /* keyboard control */
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        CenterTransform.eulerAngles += new Vector3(moveVertical, -moveHorizontal, 0) * rotSpeed;

        //print("vertical: " + CenterTransform.eulerAngles.x.ToString() + " horizontal:" + CenterTransform.eulerAngles.y.ToString());

        /* mouse control */
        // zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 100)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= 20)
                Camera.main.orthographicSize += 0.5F;
        }
        // zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5F;
        }

    }
}
