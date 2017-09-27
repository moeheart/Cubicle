using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHints : MonoBehaviour {

    public GameObject ButtonInfo;

    public void MouseEnter()
    {
        ButtonInfo.SetActive(true);
    }

    public void MouseExit()
    {
        ButtonInfo.SetActive(false);
    }
}
