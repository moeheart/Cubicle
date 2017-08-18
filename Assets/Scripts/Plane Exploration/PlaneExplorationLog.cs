using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


public class PlaneExplorationLog : MonoBehaviour {


	private string logIdPath = "Assets/Logs/Plane Exploration/_IdLog.txt";
	private string logFilePath = "Assets/Logs/Plane Exploration/_PlaneExplorationLogs.txt";
	private string logDetailPath = "Assets/Logs/PlaneExploration/";

	/* id,timestamp,trial_num,level,tot_time,result */

	private string logString;
	private string iniString;

	private FileStream fs;
	private FileStream fsDetail;
	private uint id;

	private System.DateTime startTime;


	//	void Start(){
	//		FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);
	//
	//		int x = 0;
	//		_fs.WriteByte ((byte)x);
	//		_fs.Close();
	//		_fs.Dispose();
	//	}


	public void RecordInitialization(int trial_num, int level){

		// get id
		FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Read);

		id = (uint)(_fs.ReadByte ());
		_fs.Close();
		_fs.Dispose();

		// get filestream
		fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);


		// get start time
		startTime = System.DateTime.Now;

		// initialize log
		logString = "\n" + id.ToString () + "," + startTime + "," + trial_num.ToString () + "," + level.ToString() + ",";

		// initialize detail log file
		string detailFileName = logDetailPath + id.ToString() + ".txt";
		fsDetail = new FileStream(detailFileName, FileMode.Create, FileAccess.Write);
		string detailInitialization = "timestamp,operation";
		byte[] map = Encoding .UTF8.GetBytes(detailInitialization.ToString());
		fsDetail.Write(map, 0, map.Length);

	}


	public void LogDetail(string operation, string position){

		System.DateTime curTime = System.DateTime.Now;

		string detail = "\n" + curTime + "," + operation + "," + position;
		byte[] map = Encoding .UTF8.GetBytes(detail.ToString());
		fsDetail.Write(map, 0, map.Length);

	}


	public void RecordResult(bool result){

		System.DateTime curTime = System.DateTime.Now;
		logString += (curTime-startTime).ToString() + "," + result.ToString ();

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

		// update id
		FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);

		_fs.WriteByte ((byte)(id + 1));
		_fs.Close();
		_fs.Dispose();
	}
}
