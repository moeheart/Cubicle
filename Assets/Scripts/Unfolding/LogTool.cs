using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LogTool : MonoBehaviour {

    public MeshGenerator meshgenerator;

    private string content = "";
    private string filePath;
    private string path1 = "Assets/Logs/Unfolding/user_Level";
    private string path2 = ".txt";
    private float StartTime;
    private float EndTime;

    void Start()
    {
        content = "Game Start\n";
        StartTime = Time.time;
    }

    /// <summary>
    /// Add the clicked line to log.
    /// </summary>
    /// <param name="p1">The first point of the line.</param>
    /// <param name="p2">The second point of the line.</param>
	public void LineClick(Vector3 p1, Vector3 p2)
    {
        content += "Type: ClickLine, ";

        content += "Line: (" + p1.x + "," + p1.y + "," + p1.z + ":" + p2.x + "," + p2.y + "," + p2.z + "), ";
        content += "Time: " + DateTime.Now + "\n";
    }

    /// <summary>
    /// Add the reverted line to log.
    /// </summary>
    /// <param name="p1">The first point of the line.</param>
    /// <param name="p2">The second point of the line.</param>
    public void LineRevert(Vector3 p1, Vector3 p2)
    {
        content += "Type: Stepback, ";
        content += "Line: (" + p1.x + "," + p1.y + "," + p1.z + ":" + p2.x + "," + p2.y + "," + p2.z + "), ";
        content += "Time: " + DateTime.Now + "\n";
    }

    public void Submit(int score)
    {
        content += "Type: Submit, ";
        content += "Score: " + score + ", ";

        // Everytime the user submits the result, we calculate the time cost of this play.  
        EndTime = Time.time;
        float duration = EndTime - StartTime;
        content += "TimeCost: " + duration + "s, ";

        content += "Time: " + DateTime.Now + "\n";
    }

    /// <summary>
    /// Save the log content;
    /// </summary>
    public void SaveLog()
    {
        path1 = Path.Combine(Application.dataPath, "Logs/Unfolding/user_Level");
        filePath = path1 + meshgenerator.CurrentLevel + path2;

        // Write some text to the text.txt file, but we don't need it in webGL.
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.Write(content);
        writer.Close();

        ClearLog();
    }

    /// <summary>
    /// Clear the log content.
    /// </summary>
    public void ClearLog()
    {
        content = "";
    }

    /// <summary>
    /// Keep log when the user quits the game.
    /// </summary>
    public void QuitGame()
    {
        content += "Quit\n";
    }

    /// <summary>
    /// Keep log when clicking the replay button.
    /// </summary>
    public void Replay()
    {
        content += "Replay\n";
        // Reset the timer when replaying the game.
        StartTime = Time.time;
    }
}
