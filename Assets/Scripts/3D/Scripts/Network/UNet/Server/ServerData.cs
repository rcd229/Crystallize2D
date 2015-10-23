using UnityEngine;
using System;
using System.Collections;
using System.IO;
using Util.Serialization;

public class ServerData {

    public const string RelativePath = "/CrystallizeRemote/PlayerData/";
    public const string XmlExtension = ".xml";

    public static string DirectoryPath {
        get {
            return Directory.GetParent(Application.dataPath).Parent.FullName + ServerData.RelativePath;
        }
    }

    static string UsernamePasswordFilePath { get { return DirectoryPath + "UsernamePasswordData" + XmlExtension; } }
    static UsernamePasswordTable UsernamePasswordTable { get; set; }

    static ServerData() {
        UsernamePasswordTable = LoadUsernamePasswordTable();
    }

    //check if the current logging directory does not already have the name. Case Insensitive
    public static bool CheckNameValid(string name) {
        //check for invalid length
        if (name.Length > 15) {
            return false;
        }

        //check for valid file names
        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1) {
            return false;
        }

        //check for duplicate names
        var path = DirectoryPath;
        if (!Directory.Exists(path)) {
            return true;
        }

        var dirInfo = new DirectoryInfo(path);
        foreach (var subDir in dirInfo.GetDirectories()) {
            if (subDir.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
        }

        foreach (var xmlFile in dirInfo.GetFiles()) {
            if (xmlFile.Name.Equals(name + XmlExtension, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
        }

        return true;
    }

    public static void SavePlayerData(string name, string data) {
        var path = DirectoryPath;
        var file = name + XmlExtension;

        Serializer.SaveWithBackups(path + file, data);
    }

    public static string LoadPlayerData(string name) {
        var path = DirectoryPath;
        var file = name + XmlExtension;
        string data;
        try {
            var reader = new StreamReader(path + file);
            data = reader.ReadToEnd();
        } catch {
            data = "";
        }
        return data;
    }

    public static void AddUsernamePasswordEntry(UsernamePasswordPair pair) {
        UsernamePasswordTable.AddEntry(pair);
        SaveUsernamePasswordTable();
    }

    public static bool IsUsernamePasswordValid(UsernamePasswordPair pair) {
        return UsernamePasswordTable.CheckPassword(pair);
    }

    static void SaveUsernamePasswordTable() {
        var data = Serializer.SaveToXmlString<UsernamePasswordTable>(UsernamePasswordTable);
        Serializer.SaveWithBackups(UsernamePasswordFilePath, data);
    }

    static UsernamePasswordTable LoadUsernamePasswordTable() {
        if (!File.Exists(UsernamePasswordFilePath)) {
            return new UsernamePasswordTable();
        } else {
            return Serializer.LoadFromXml<UsernamePasswordTable>(UsernamePasswordFilePath);
        }
    }
}
