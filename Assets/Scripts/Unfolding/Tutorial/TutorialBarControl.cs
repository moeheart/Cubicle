using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBarControl : MonoBehaviour {

    public GameObject TutorialCanvas;
    public GameObject UserCanvas;
    public MeshGenerator meshgenerator;
    public GameObject gameObj;
    public GameObject GoalImage;
    public GameObject[] Texts = new GameObject[5];

    public int TotalPageNum = 5;
    public int MinPageNum = 1;
    private int CurrentPageNum = 0;

    Text txt;
    // Use this for initialization
    void Start () {
        // Deactive the gameobject first.
        gameObj.SetActive(false);

        CurrentPageNum = MinPageNum;
        txt = GetComponent<Text>();
        txt.text = CurrentPageNum + "/" + TotalPageNum;
    }
	
	public void RightClick()
    {
        int previousNum = CurrentPageNum;

        CurrentPageNum++;
        CurrentPageNum = CurrentPageNum > TotalPageNum ? TotalPageNum : CurrentPageNum;
        txt.text = CurrentPageNum + "/" + TotalPageNum;

        if(previousNum != CurrentPageNum)
        {
            LoadText(previousNum);
        }

    }

    public void LeftClick()
    {
        int previousNum = CurrentPageNum;

        CurrentPageNum--;
        CurrentPageNum = CurrentPageNum < MinPageNum ? MinPageNum : CurrentPageNum;
        txt.text = CurrentPageNum + "/" + TotalPageNum;

        if (previousNum != CurrentPageNum)
        {
            LoadText(previousNum);
        }
    }

    private void LoadText(int previousPageNum)
    {
        Texts[previousPageNum - 1].SetActive(false);
        Texts[CurrentPageNum - 1].SetActive(true);

        // Welcomint session.
        if(CurrentPageNum == MinPageNum)
        {
            gameObj.SetActive(false);
        }
        // Moving session
        if(CurrentPageNum == MinPageNum + 1)
        {
            if(previousPageNum - CurrentPageNum < 0)
                gameObj.SetActive(true);
        }
        // Practicing session
        if(CurrentPageNum == MinPageNum + 2)
        {
            if(previousPageNum - CurrentPageNum > 0)
                GoalImage.SetActive(false);
        }
        // Unfolding session.
        if(CurrentPageNum == MinPageNum + 3)
        {
            if (previousPageNum - CurrentPageNum > 0)
                gameObj.SetActive(true);
            meshgenerator.ReGenerate(0);
            GoalImage.SetActive(true);
        }
        // Last session.
        if(CurrentPageNum == MinPageNum + 4)
        {
            gameObj.SetActive(false);
            GoalImage.SetActive(false);
            StartCoroutine("WaitAndJump");
        }
    }

    private int GetLevelByRoomid(int _roomid)
    {
        return _roomid;
    }

    public void SkipTutorial()
    {
        TutorialCanvas.SetActive(false);
        UserCanvas.SetActive(true);
        gameObj.SetActive(true);
        int roomid = DataUtil.GetCurrentRoomId();
        int Level = GetLevelByRoomid(roomid);
        meshgenerator.ReGenerate(Level);
    }

    IEnumerator WaitAndJump()
    {
        yield return new WaitForSeconds(3.0f);
        TutorialCanvas.SetActive(false);
        UserCanvas.SetActive(true);
        gameObj.SetActive(true);
        int roomid = DataUtil.GetCurrentRoomId();
        int Level = GetLevelByRoomid(roomid);
        meshgenerator.ReGenerate(Level);
    }
}
