using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Container;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    public MeshGenerator meshGenerator;
    public ScoreManager WinScoreManager;
    public ScoreManager LoseScoreManager;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public GameObject UserCanvas;
    LogTool logtool;

    private Vector3 InitialPos;
    private Quaternion InitialRot;

    private string path;
    private string path1 = "Assets/Resources/Unfolding/_Results/Level";
    private string path2 = ".txt";

    private List<int> FinalLevelofStage = new List<int> { 2, 4, 6 };

    [HideInInspector]
    public bool unfolding = false;
    bool unfoldA = false;
    bool unfoldB = false;

    List<Vector3> WaitingLinesStartingPoint;
    List<Vector3> WaitingLinesEndingPoint;

    List<Vector3> PreviousStartingPoint;
    List<Vector3> PreviousEndingPoint;
    List<bool> PreviousUnfoldingA;
    List<bool> PreviousUnfoldingB;
    List<Vector3> PreviousStartingNormalA;
    List<Vector3> PreviousStartingNormalB;
    bool foldback = false;

    // Use this for initialization
    void Start () {
        InitialPos = Camera.main.transform.position;
        InitialRot = Camera.main.transform.rotation;

        //TargetPositions = new List<Vector3>();
        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);

        WaitingLinesStartingPoint = new List<Vector3>();
        WaitingLinesEndingPoint = new List<Vector3>();

        PreviousStartingPoint = new List<Vector3>();
        PreviousEndingPoint = new List<Vector3>();
        PreviousUnfoldingA = new List<bool>();
        PreviousUnfoldingB = new List<bool>();
        PreviousStartingNormalA = new List<Vector3>();
        PreviousStartingNormalB = new List<Vector3>();

        logtool = GetComponent<LogTool>();

        path = path1 + meshGenerator.CurrentLevel + path2;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("Line") == null)
        {
            WaitingLinesStartingPoint.Clear();
            WaitingLinesEndingPoint.Clear();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("World Scene");
        }
        if (!unfolding && WaitingLinesStartingPoint.ToArray().Length != 0)
        {
            Vector3 midPoint = (WaitingLinesStartingPoint[0] + WaitingLinesEndingPoint[0]) / 2;

            PreviousStartingPoint.Add(WaitingLinesStartingPoint[0]);
            PreviousEndingPoint.Add(WaitingLinesEndingPoint[0]);

            WaitingLinesStartingPoint.RemoveAt(0);
            WaitingLinesEndingPoint.RemoveAt(0);
            ClickAtLine(midPoint);
        }

        if (!unfolding && Input.GetMouseButtonDown(0)) {
            RaycastHit hitObject = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitObject) && hitObject.transform.tag == "Line")
            {
                Vector3 startingPoint = hitObject.transform.GetComponent<LineRenderer>().GetPosition(0);
                Vector3 endingPoint = hitObject.transform.GetComponent<LineRenderer>().GetPosition(1);

                //Destroy the Line.
                hitObject.transform.GetComponent<Glow>().destroyLine();

                //Instead of destroying the line, we change its color to grey.
                //hitObject.transform.GetComponent<BoxCollider>().enabled = false;
                //hitObject.transform.GetComponent<Glow>().ChangeToGray();

                Vector3 midPoint = (startingPoint + endingPoint) / 2;

                foldback = false;
                PreviousStartingPoint.Clear();
                PreviousEndingPoint.Clear();
                PreviousUnfoldingA.Clear();
                PreviousUnfoldingB.Clear();
                PreviousStartingNormalA.Clear();
                PreviousStartingNormalB.Clear();

                ClickAtLine(midPoint);

                logtool.LineClick(startingPoint, endingPoint);

                PreviousStartingPoint.Add(startingPoint);
                PreviousEndingPoint.Add(endingPoint);
            }
        }
    }

    public void StepBack()
    {
        if (!unfolding && !foldback)
        {
            RecoverLineInfo(PreviousStartingPoint[0], PreviousEndingPoint[0]);
            meshGenerator.ReCreateLine(PreviousStartingPoint[0], PreviousEndingPoint[0]);
            logtool.LineRevert(PreviousStartingPoint[0], PreviousEndingPoint[0]);

            if (PreviousUnfoldingA[0] || PreviousUnfoldingB[0])
            {
                if (PreviousStartingPoint.ToArray().Length == 2)
                {
                    RecoverLineInfo(PreviousStartingPoint[1], PreviousEndingPoint[1]);
                    if (PreviousUnfoldingA[0])
                    {
                        Vector3 ckMidPoint = (PreviousStartingPoint[0] + PreviousEndingPoint[0]) / 2;
                        // Vector3 rmMidPoint = (PreviousStartingPoint[1] + PreviousEndingPoint[1]) / 2;
                        FoldingBack(ckMidPoint, PreviousStartingPoint[1], PreviousEndingPoint[1], 0, PreviousStartingNormalA[0]);
                    }
                    if (PreviousUnfoldingB[0])
                    {
                        Vector3 ckMidPoint = (PreviousStartingPoint[0] + PreviousEndingPoint[0]) / 2;
                        // Vector3 rmMidPoint = (PreviousStartingPoint[1] + PreviousEndingPoint[1]) / 2;
                        FoldingBack(ckMidPoint, PreviousStartingPoint[1], PreviousEndingPoint[1], 1, PreviousStartingNormalB[0]);
                    }
                }
                else
                {
                    // TODO: We still need to consider the situation that unfolding happens sequentially.
                }
            }
            foldback = true;
        }   
    }

    private void FoldingBack(Vector3 ckMidPoint, Vector3 rmStartingPoint, Vector3 rmEndingPoint, int index, Vector3 TargetNormal)
    {
        List<int> faceIndices = meshGenerator.GetFaceIndicesOfLine(ckMidPoint);
        int FaceIndex = faceIndices[index];

        Vector3 rmMidPoint = (rmStartingPoint + rmEndingPoint) / 2;
        /*Vector3 rmStartingPoint = meshGenerator.model.faces[FaceIndex].lineStartingPoint[0];
        Vector3 rmEndingPoint = meshGenerator.model.faces[FaceIndex].lineEndingPoint[0];
        Vector3 rmMidPoint = meshGenerator.model.faces[FaceIndex].linesMidpoints[0];*/

        RecoverLastLine(rmMidPoint);
        meshGenerator.DeleteDashedLineIndex(FaceIndex);

        int TargetIndex = meshGenerator.GetTheOtherFaceIndex(rmMidPoint, FaceIndex);
        // Probem: The current Normal is the same as the TargetNormal now.
        // Solution: we define the TargetNormal by ourselves.
        Vector3 currentNormal = meshGenerator.GetNormalofFace(FaceIndex);

        int NumOfConnectedFaces = meshGenerator.model.faces[FaceIndex].ConnectedFaces.ToArray().Length;
        for (int i = -1; i < NumOfConnectedFaces; i++)
        {
            int currentIndex;
            if (i == -1)
            {
                currentIndex = FaceIndex;
                meshGenerator.model.faces[FaceIndex].Neighbors.Remove(currentIndex);
                meshGenerator.model.faces[currentIndex].Neighbors.Remove(FaceIndex);
            }
            else
                currentIndex = meshGenerator.model.faces[FaceIndex].ConnectedFaces[i];

            meshGenerator.model.faces[TargetIndex].ConnectedFaces.Remove(currentIndex);
        }

        unfolding = true;
        StartUnfolding(FaceIndex, currentNormal, TargetNormal, rmStartingPoint, rmEndingPoint);
        //StartUnfolding(indexB, currentNormalofB, TargetNormalofB, rmStartingPoint, rmEndingPoint);
    }

    private void ClickAtLine(Vector3 midPoint)
    {
        //Get two faces' indices of the selected line.
        List<int> faceIndices = meshGenerator.GetFaceIndicesOfLine(midPoint);

        int indexA = faceIndices[0];
        int indexB = faceIndices[1];

        //Delete the lines in the two faces. Then check if one of these faces has only one line left, if so, it needs to unfold.
        unfoldA = DeleteAndCheckLinesInFace(indexA, midPoint);
        unfoldB = DeleteAndCheckLinesInFace(indexB, midPoint);
        unfolding = unfoldA || unfoldB;
        Debug.Log(unfolding);

        PreviousUnfoldingA.Add(unfoldA);
        PreviousUnfoldingB.Add(unfoldB);

        if (unfoldA)
        {
            Vector3 rmStartingPoint = meshGenerator.model.faces[indexA].lineStartingPoint[0];
            Vector3 rmEndingPoint = meshGenerator.model.faces[indexA].lineEndingPoint[0];
            Vector3 rmMidPoint = meshGenerator.model.faces[indexA].linesMidpoints[0];

            DeleteLastLine(rmStartingPoint, rmEndingPoint);
            meshGenerator.AddDashedLineIndex(indexA);

            if (GameObject.FindGameObjectWithTag("Line") == null)
            {
                WaitingLinesStartingPoint.Clear();
                WaitingLinesEndingPoint.Clear();
                return;
            }

            int TargetIndexofA = meshGenerator.GetTheOtherFaceIndex(rmMidPoint, indexA);
            Vector3 currentNormalofA = meshGenerator.GetNormalofFace(indexA);
            Vector3 TargetNormalofA = meshGenerator.GetNormalofFace(TargetIndexofA);

            int NumOfConnectedFaces = meshGenerator.model.faces[indexA].ConnectedFaces.ToArray().Length;
            for (int i = -1; i < NumOfConnectedFaces; i++)
            {
                int currentIndex;
                if (i == -1)
                {
                    currentIndex = indexA;
                    meshGenerator.model.faces[TargetIndexofA].Neighbors.Add(currentIndex);
                    meshGenerator.model.faces[currentIndex].Neighbors.Add(TargetIndexofA);
                }
                else
                    currentIndex = meshGenerator.model.faces[indexA].ConnectedFaces[i];

                meshGenerator.model.faces[TargetIndexofA].ConnectedFaces.Add(currentIndex);
            }


            PreviousStartingNormalA.Add(currentNormalofA);
            StartUnfolding(indexA, currentNormalofA, TargetNormalofA, rmStartingPoint, rmEndingPoint);
        }
        if (unfoldB)
        {

            Vector3 rmStartingPoint = meshGenerator.model.faces[indexB].lineStartingPoint[0];
            Vector3 rmEndingPoint = meshGenerator.model.faces[indexB].lineEndingPoint[0];
            Vector3 rmMidPoint = meshGenerator.model.faces[indexB].linesMidpoints[0];

            DeleteLastLine(rmStartingPoint, rmEndingPoint);
            meshGenerator.AddDashedLineIndex(indexB);

            if (GameObject.FindGameObjectWithTag("Line") == null)
            {
                WaitingLinesStartingPoint.Clear();
                WaitingLinesEndingPoint.Clear();
                return;
            }

            int TargetIndexofB = meshGenerator.GetTheOtherFaceIndex(rmMidPoint, indexB);
            Vector3 currentNormalofB = meshGenerator.GetNormalofFace(indexB);
            Vector3 TargetNormalofB = meshGenerator.GetNormalofFace(TargetIndexofB);

            int NumOfConnectedFaces = meshGenerator.model.faces[indexB].ConnectedFaces.ToArray().Length;
            for (int i = -1; i < NumOfConnectedFaces; i++)
            {
                int currentIndex;
                if (i == -1)
                {
                    currentIndex = indexB;
                    meshGenerator.model.faces[TargetIndexofB].Neighbors.Add(currentIndex);
                    meshGenerator.model.faces[currentIndex].Neighbors.Add(TargetIndexofB);
                }
                else
                    currentIndex = meshGenerator.model.faces[indexB].ConnectedFaces[i];

                meshGenerator.model.faces[TargetIndexofB].ConnectedFaces.Add(currentIndex);
            }

            PreviousStartingNormalB.Add(currentNormalofB);
            StartUnfolding(indexB, currentNormalofB, TargetNormalofB, rmStartingPoint, rmEndingPoint);
        }
    }

    private void StartUnfolding(int faceIndex, Vector3 startingNormal, Vector3 endingNormal,  Vector3 startingPoint, Vector3 endingPoint)
    {
        //Debug.Log(Vector3.Dot(startingNormal, endingNormal));
        //float angle = Mathf.Acos(Vector3.Dot(startingNormal, endingNormal));

        //int vertexOffset = meshGenerator.model.faces[faceIndex].offset;
        //int vertexSize = meshGenerator.model.faces[faceIndex].vertices.ToArray().Length;

        Quaternion rotation = new Quaternion();
        rotation.SetFromToRotation(startingNormal, endingNormal);

        //Debug.Log("Starting Normal is " + startingNormal);
        //Debug.Log("Endign Normal is " + endingNormal);
        //Debug.Log("Starting Point is " + startingPoint);
        //Debug.Log("Ending Point is " + endingPoint);

        meshGenerator.StartUnfolding(faceIndex, rotation, startingNormal, endingNormal, startingPoint, endingPoint);
    }

    private bool DeleteAndCheckLinesInFace(int faceIndex, Vector3 midPoint)
    {
        Face face = meshGenerator.model.faces[faceIndex];

        if (face.linesMidpoints.ToArray().Length <= 0)
            return false;

        int index = face.linesMidpoints.IndexOf(midPoint);

        face.linesMidpoints.Remove(midPoint);
        face.lineStartingPoint.RemoveAt(index);
        face.lineEndingPoint.RemoveAt(index);
        Debug.Log(face.linesMidpoints.ToArray().Length);
        if (face.linesMidpoints.ToArray().Length != 1)
            return false;
        else
            return true;   
    }

    private void RecoverLineInfo(Vector3 startingPoint, Vector3 endingPoint)
    {
        Vector3 midPoint = (startingPoint + endingPoint) / 2;
        //Get two faces' indices of the selected line.
        List<int> faceIndices = meshGenerator.GetFaceIndicesOfLine(midPoint);
        int faceA = faceIndices[0];
        int faceB = faceIndices[1];

        meshGenerator.model.faces[faceA].linesMidpoints.Add((startingPoint + endingPoint) / 2);
        meshGenerator.model.faces[faceA].lineStartingPoint.Add(startingPoint);
        meshGenerator.model.faces[faceA].lineEndingPoint.Add(endingPoint);
        meshGenerator.model.faces[faceB].linesMidpoints.Add((startingPoint + endingPoint) / 2);
        meshGenerator.model.faces[faceB].lineStartingPoint.Add(startingPoint);
        meshGenerator.model.faces[faceB].lineEndingPoint.Add(endingPoint);
    }

    private void RecoverLastLine(Vector3 midPoint)
    {
        foreach (GameObject line in GameObject.FindGameObjectsWithTag("Line"))
        {
            Vector3 startingPoint = line.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 endingPoint = line.GetComponent<LineRenderer>().GetPosition(1);
            if (midPoint == (startingPoint + endingPoint) / 2)
            {
                line.GetComponent<BoxCollider>().enabled = true;
                line.GetComponent<Glow>().ChangeToSolidLine();

                meshGenerator.DeleteDashedLineMidpoints(midPoint);
            }

        }
    }

    private void DeleteLastLine(Vector3 rmStartingPoint, Vector3 rmEndingPoint)
    {
        foreach(GameObject line in GameObject.FindGameObjectsWithTag("Line"))
        {
            Vector3 startingPoint = line.GetComponent<LineRenderer>().GetPosition(0);
            Vector3 endingPoint = line.GetComponent<LineRenderer>().GetPosition(1);
            if (startingPoint == rmStartingPoint && endingPoint == rmEndingPoint)
            {
                //Destroy(line);
                line.GetComponent<BoxCollider>().enabled = false;
                line.GetComponent<Glow>().ChangeToDashedLine();

                meshGenerator.AddDashedLineMidpoints((startingPoint + endingPoint) / 2);
            }
                
        }

        //You need to consider a series of unfolding, which means when you delete the last line, a new unfolding happens.
        //But this can be done by deleting the line after the previous unfolding finished.

        //Solution:Push the midPoint to an List, waiting to be processed.
        WaitingLinesStartingPoint.Add(rmStartingPoint);
        WaitingLinesEndingPoint.Add(rmEndingPoint);
    }

    public void Grading()
    {
        int grade = CheckResult();
        Debug.Log("Your grade is: " + grade);
        UserCanvas.SetActive(false);

        logtool.Submit(grade);
        logtool.SaveLog();

        if (grade == 100)
        {
            WinCanvas.SetActive(true);
            WinScoreManager.SetScore(grade);
            DataUtil.UnlockCurrentRoom();
        }
        else
        {
            LoseCanvas.SetActive(true);
            LoseScoreManager.SetScore(grade);
        }
    }

    private int CheckResult()
    {
        float TotalGrade = 0;
        float CurrentGrade = 0;

        path = path1 + meshGenerator.CurrentLevel + path2;
        StreamReader reader = new StreamReader(path, false);
        char[] delimiterChars = { ';' };

        int NumofFaces = meshGenerator.NumofFaces;
        for(int i = 0; i < NumofFaces; i++)
        {
            //Read header.
            reader.ReadLine();
            string currentLine = reader.ReadLine();

            string[] results = currentLine.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            /*if (meshGenerator.model.faces[i].Neighbors.ToArray().Length != results.Length)
                return false;*/
            foreach(string result in results)
            {
                /*if (!meshGenerator.model.faces[i].Neighbors.Contains(int.Parse(result)))
                    return false;*/
                TotalGrade += 1;
                if (meshGenerator.model.faces[i].Neighbors.Contains(int.Parse(result)))
                    CurrentGrade += 1;
            }
            
        }
        reader.Close();
        return (int)((CurrentGrade / TotalGrade) * 100);
    }

    public void ShowResult()
    {
        int NumofFaces = meshGenerator.NumofFaces;
        for(int i = 0; i < NumofFaces; i++)
        {
            Debug.Log("Face " + i + " contains:");
            string CurrentNeighbor = "";
            foreach(int j in meshGenerator.model.faces[i].Neighbors)
            {
                CurrentNeighbor += j.ToString();
                CurrentNeighbor += ";";
            }
            Debug.Log(CurrentNeighbor);
        }
    }

    public void SaveResult()
    {
        path = path1 + meshGenerator.CurrentLevel + path2;
        StreamWriter writer = new StreamWriter(path, false);

        int NumofFaces = meshGenerator.NumofFaces;
        for (int i = 0; i < NumofFaces; i++)
        {
            writer.WriteLine("Face " + i + " contains:");
            string CurrentNeighbor = "";
            foreach (int j in meshGenerator.model.faces[i].Neighbors)
            {
                CurrentNeighbor += j.ToString();
                CurrentNeighbor += ";";
            }

            writer.WriteLine(CurrentNeighbor);
        }

        writer.Close();
    }

    public void Replay()
    {
        meshGenerator.ReGenerate(meshGenerator.CurrentLevel);

        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);
        UserCanvas.SetActive(true);

        // Camera.main.transform.position = InitialPos;
        // Camera.main.transform.rotation = InitialRot;
    }
    
    public void Proceed()
    {
        int currentLevel = meshGenerator.CurrentLevel;
        if (FinalLevelofStage.Contains(currentLevel))
        {
            SceneManager.LoadScene("World Scene");
        }
        else
        {
            WinCanvas.SetActive(false);
            UserCanvas.SetActive(true);
            meshGenerator.ReGenerate(++currentLevel);
            // Camera.main.transform.position = InitialPos;
            // Camera.main.transform.rotation = InitialRot;
        }
    }

}
