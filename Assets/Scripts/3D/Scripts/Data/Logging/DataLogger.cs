using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class DataLogger {

    static string LoggingDirectory = "CrystallizeLog/";
    static DataLogger _instance;

    static DataLogger Instance {
        get {
            if (_instance == null) {
                _instance = new DataLogger();
            }
            return _instance;
        }
    }

    static string GetNextLogFile() {
        var path = Application.dataPath;
        var dir = Directory.GetParent(path).Parent;
        path = dir.FullName + "/" + LoggingDirectory;
        
        if(!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        
        int i = 0;
        while (File.Exists(path + "/" + i.ToString("D4"))) {
            i++;
        }
        path += "/" + i.ToString("D4");
        //Debug.Log(path);
        return path;
    }

    public static void LogLine(string line) {
        Instance._LogLine(line);
    }

    public static void LogTimestampedData(params string[] data) {
        var s = DateTime.Now.Ticks.ToString();
        foreach (var d in data) {
            s += "\t" + d;
        }
        if (Network.connections.Length > 0) {
            CrystallizeNetwork.Client.SendLogMessageToServer(s);
            //RPCFunctions.Instance.LogMessageToServer(s);
        } else {
            Instance._LogLine(s);
        }
    }

    StreamWriter Writer { get; set; }
    //string logFile;

    DataLogger() {
        //logFile = GetNextLogFile();
        Writer = new StreamWriter(GetNextLogFile(), true);
        CrystallizeEventManager.OnQuit += HandleQuit;
    }

    void _LogLine(string line) {
        //using(var writer = new StreamWriter())
        try {
            Writer.WriteLine(line);
        } catch (Exception e) {
            Debug.Log(e.StackTrace);
        }
    }

    void HandleQuit(object sender, EventArgs args) {
        LogTimestampedData("Quit");

        if (Writer.BaseStream != null) {
            Writer.Close();
        }
    }

}
