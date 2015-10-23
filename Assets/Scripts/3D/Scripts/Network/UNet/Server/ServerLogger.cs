using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Util.Serialization;

public class ServerLogger {

    string DirectoryPath {
        get {
            return Directory.GetParent(Application.dataPath).Parent.FullName + ServerData.RelativePath;
        }
    }

    Dictionary<string, RemoteDataLogger> playerDataLoggers = new Dictionary<string, RemoteDataLogger>();

    public void SaveMessage(string name, string data) {
        var path = DirectoryPath;
        path += name;
        RemoteDataLogger logger;
        if (!playerDataLoggers.ContainsKey(name)) {
            logger = new RemoteDataLogger(path);
            playerDataLoggers.Add(name, logger);
        } else {
            logger = playerDataLoggers[name];
        }
        logger.Log(data);
    }

    public void Flush() {
        foreach (var key_val in playerDataLoggers) {
            key_val.Value.HandleFlush();
        }
    }

    public void Close() {
        foreach (var key_val in playerDataLoggers) {
            key_val.Value.HandleQuit();
        }
    }

}
