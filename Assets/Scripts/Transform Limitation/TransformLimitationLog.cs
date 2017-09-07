using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class TransformLimitationLog : MonoBehaviour {

	private string logIdPath = "Assets/Logs/Transform Limitation/_IdLog.txt";
	private string logFilePath = "Assets/Logs/Transform Limitation/_TransformLimitationLogs.txt";
	private string logDetailPath = "Assets/Logs/Transform Limitation/";

	/* id,timestamp,trial_num,block_num,basic_step,difficulty,start_model,target_model,result,tot_time */

	private string logString;
	private string iniString;

	private FileStream fs;
	private FileStream fsDetail;
	private uint id;

	private System.DateTime startTime;


	void Start(){
		// FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);

		// int x = 0;
		// _fs.WriteByte ((byte)x);
		// _fs.Close();
		// _fs.Dispose();
	}


	public void RecordInitialization(int trial_num, int block_num, int basic_step, int difficulty, string start_model, string target_model, string method){

		// // get id
		// FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Read);

		// id = (uint)(_fs.ReadByte ());
		// _fs.Close();
		// _fs.Dispose();

		// get filestream
		fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);


		// get start time
		startTime = System.DateTime.Now;

		// initialize log
		logString = "\n" + startTime.ToString("yyyyMMddHHmmssms") + "," + trial_num.ToString () + "," + block_num.ToString () +
			"," + basic_step.ToString () + "," + difficulty.ToString() + "," + start_model + "," + target_model + "," +
			method + ",";

		// initialize detail log file
		string detailFileName = logDetailPath + startTime.ToString("yyyyMMddHHmmssms") + ".txt";
		fsDetail = new FileStream(detailFileName, FileMode.Create, FileAccess.Write);
		string detailInitialization = "timestamp,operation";
		byte[] map = Encoding .UTF8.GetBytes(detailInitialization.ToString());
		fsDetail.Write(map, 0, map.Length);
		
	}


	public void LogDetail(string operation){

		System.DateTime curTime = System.DateTime.Now;

		string detail = "\n" + curTime.ToString("yyyyMMddHHmmssms") + "," + operation;
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

		// // update id
		// FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);

		// _fs.WriteByte ((byte)(id + 1));
		// _fs.Close();
		// _fs.Dispose();
	}

}
