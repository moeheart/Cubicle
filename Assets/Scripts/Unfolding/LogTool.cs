using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LogTool : MonoBehaviour {

    public MeshGenerator meshgenerator;
    string content = "";
    string filePath;
    string path1 = "Assets/Logs/Unfolding/user_Level";
    string path2 = ".txt";

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
        content += "Time: " + DateTime.Now + "\n";
    }

    /// <summary>
    /// Save the log content;
    /// </summary>
    public void SaveLog()
    {

        path1 = Path.Combine(Application.persistentDataPath, "Logs/Unfolding/user_Level");
        filePath = path1 + meshgenerator.CurrentLevel + path2;

        // Write some text to the text.txt file, but we don't need it in webGL.
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.Write(content);
        writer.Close();
    }

    /// <summary>
    /// Clear the log content.
    /// </summary>
    public void ClearLog()
    {
        content = "";
    }


}
