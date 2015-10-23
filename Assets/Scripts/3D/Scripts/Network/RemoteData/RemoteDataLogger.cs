using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class RemoteDataLogger {

    string GetNextLogFile(string directory) {
		string path = directory;
		if(!Directory.Exists(directory)){
			Directory.CreateDirectory(directory);
        }
        
        int i = 0;
		while (File.Exists(directory + "/" + i.ToString("D4"))) {
            i++;
        }
		path += "/" + i.ToString("D4");
        //Debug.Log(path);
		return path;
    }

    public void LogLine(string line) {
        _LogLine(line);
    }

	public void Log(string data){
		try {
			Writer.WriteLine(data);
		} catch (Exception e) {
			Debug.Log(e.StackTrace);
		}
	}

    public void LogTimestampedData(params string[] data) {
        var s = DateTime.Now.Ticks.ToString();
        foreach (var d in data) {
            s += "\t" + d;
        }
        _LogLine(s);
    }

    StreamWriter Writer { get; set; }
	string LoggingDirectory;
    //string logFile;

    public RemoteDataLogger(string directory) {
        //logFile = GetNextLogFile();
		LoggingDirectory = directory;
		Writer = new StreamWriter(GetNextLogFile(LoggingDirectory), true);
//        CrystallizeEventManager.OnQuit += HandleQuit;
    }

    void _LogLine(string line) {
        //using(var writer = new StreamWriter())
        try {
            Writer.WriteLine(line);
        } catch (Exception e) {
            Debug.Log(e.StackTrace);
        }
    }

    public void HandleQuit() {
        LogTimestampedData("Quit");

        if (Writer.BaseStream != null) {
            Writer.Close();
        }
    }

	public void HandleFlush(){
		Writer.Flush();
	}

}
