using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


public class PlaneExplorationLog : MonoBehaviour {


	private string logIdPath = "Assets/Logs/Plane Exploration/_IdLog.txt";
	private string logFilePath = "Assets/Logs/Plane Exploration/_PlaneExplorationLogs.txt";
	private string logDetailPath = "Assets/Logs/Plane Exploration/";

	/* id,timestamp,trial_num,level,tot_time,result */

	private string logString;
	private string iniString;

	private FileStream fs;
	private FileStream fsDetail;
	private uint id;

	private System.DateTime startTime;
	private System.DateTime lastTime;


	public void RecordInitialization(int trial_num, int level){

		logDetailPath = Path.Combine(Application.dataPath, "Logs/Plane Exploration/");
		logFilePath = Path.Combine(logDetailPath, "_PlaneExplorationLogs.txt");
		logIdPath = Path.Combine(logDetailPath, "_IdLog.txt");

		// // get id
		// FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Read);

		// id = (uint)(_fs.ReadByte ());
		// _fs.Close();
		// _fs.Dispose();

		// get filestream
		fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);


		// get start time
		startTime = System.DateTime.Now;
		lastTime = System.DateTime.Now;

		// initialize log
		// logString = "\n" + id.ToString () + "," + startTime + "," + trial_num.ToString () + "," + level.ToString() + ",";
		logString = "\n" + startTime.ToString("yyyyMMddHHmmssms") + "," + trial_num.ToString () + "," + level.ToString() + ",";

		// initialize detail log file
		// string detailFileName = logDetailPath + id.ToString() + ".txt";
		string detailFileName = logDetailPath + startTime.ToString("yyyyMMddHHmmssms") + ".txt";
		fsDetail = new FileStream(detailFileName, FileMode.Create, FileAccess.Write);
		string detailInitialization = "timestamp,x_operation,z_operation,position";
		byte[] map = Encoding .UTF8.GetBytes(detailInitialization.ToString());
		fsDetail.Write(map, 0, map.Length);

	}


	public void LogDetail(float x_operation, float z_operation, string position){

		System.DateTime curTime = System.DateTime.Now;

		if ((curTime-lastTime).TotalSeconds >= 0.5) {
			string detail = "\n" + curTime.ToString("yyyyMMddHHmmssms") + "," + x_operation.ToString () + "," + z_operation.ToString () + "," + position;
			byte[] map = Encoding.UTF8.GetBytes (detail.ToString ());
			fsDetail.Write (map, 0, map.Length);
			lastTime = curTime;
		}

	}


	public void RecordResult(int result){ // -1:die 0:inadequate 1:right 

		System.DateTime curTime = System.DateTime.Now;
		logString += (curTime-startTime).ToString() + "," + result.ToString();

		CommitResult ();

	}


	void CommitResult(){

		// write
		byte[] map = Encoding .UTF8.GetBytes(logString.ToString());
		fs.Write(map, 0, map.Length);

		// release
		fs.Close();
		fs.Dispose();

		// release detail file
		fsDetail.Close();
		fsDetail.Dispose ();

		// // update id
		// FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);

		// _fs.WriteByte ((byte)(id + 1));
		// _fs.Close();
		// _fs.Dispose();
	}
}
