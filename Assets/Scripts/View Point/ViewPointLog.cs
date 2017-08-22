using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class ViewPointLog : MonoBehaviour {

	private string logIdPath = "Assets/Logs/View Point/_IdLog.txt";
	private string logFilePath = "Assets/Logs/View Point/_ViewPointLogs.txt";

	/* timestamp,trial_num,model_num,view_point,
	 * model0,model1,model2,model3,model4,model5,model6,model7,
	 * select_point,tot_time,result */

	private string logString;
	private string iniString;

	private FileStream fs;
	private uint id;

	private System.DateTime startTime;


	public void RecordInitialization(int trial_num, int model_num, int view_point, string model){

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
		logString = "\n" + id.ToString () + "," + startTime + "," + trial_num.ToString () + "," + model_num.ToString () +
		"," + view_point.ToString () + "," + model;
	}

	public void RecordResult(int select_point, bool result){

		System.DateTime curTime = System.DateTime.Now;
		logString += select_point.ToString () + "," + (curTime-startTime).ToString()
			+ "," + result.ToString ();

		CommitResult ();

	}

	void CommitResult(){

		// write
		byte[] map = Encoding .UTF8.GetBytes(logString.ToString());
		fs.Write(map, 0, map.Length);

		// release
		fs.Close();
		fs.Dispose();

		// update id
		FileStream _fs = new FileStream(logIdPath, FileMode.Open, FileAccess.Write);

		_fs.WriteByte ((byte)(id + 1));
		_fs.Close();
		_fs.Dispose();
	}

}