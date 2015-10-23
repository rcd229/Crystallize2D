using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using System.IO;
using Util.Serialization;

public class LoadLogData : MonoBehaviour {

    string FolderName = "PlayerData/";

    string FullPath {
        get {
            return Directory.GetParent(Application.dataPath).Parent + "/CrystallizeRemote/" + FolderName;
        }
    }

    string OutPath {
        get {
            return Directory.GetParent(Application.dataPath).Parent + "/CrystallizeRemote/";
        }
    }

    Dictionary<string, Action<UserDataEntry, LogDataEntry>> entryHandlers = new Dictionary<string, Action<UserDataEntry, LogDataEntry>>();

    List<string> titles = new List<string>();
    Dictionary<string, UserDataEntry> userData = new Dictionary<string, UserDataEntry>();

    Dictionary<string, HashSet<string>> userOnlineWithDictionary = new Dictionary<string, HashSet<string>>();

    List<KeyValuePair<Func<DateTime>, string>> chatLines = new List<KeyValuePair<Func<DateTime>, string>>();

    Dictionary<string, int[]> reviewResults = new Dictionary<string, int[]>();
    DataTable table = new DataTable();
    Dictionary<string, DataTable> reviewTables = new Dictionary<string, DataTable>();
    string reviewRatio = "";

    StreamWriter chatWriter;
    int chatlineCount = 0;

    // Use this for initialization
    void Start() {
        entryHandlers["Chat"] = HandleChat;
        entryHandlers["Review"] = HandleReview;
        entryHandlers["Position"] = HandlePosition;

        ReadLogs();
    }

    void ReadLogs() {
        var dirs = Directory.GetDirectories(FullPath);

        DataTable preSurvey;
        DataTable postSurvey;
        ReadSurveyData.ReadData(out preSurvey, out postSurvey);
        DateTime firstTime = new DateTime(2015, 9, 5);

        foreach (var d in dirs) {
            var user = new DirectoryInfo(d).Name;
            var thisUserData = new UserDataEntry(user);
            userData[user] = thisUserData;

            var playerData = Serializer.LoadFromXml<PlayerData>(d + ".xml", false);

            thisUserData.lastModified = new FileInfo(d + ".xml").LastWriteTime;

            foreach (var f in Directory.GetFiles(d)) {
                string output;
                using (var reader = new StreamReader(f)) { output = reader.ReadToEnd(); }
                var lines = output.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var l in lines) {
                    if (l == "") { continue; }

                    var entry = new LogDataEntry(l);
                    thisUserData.AddTime(entry.GetDataTime());
                    if (entryHandlers.ContainsKey(entry.Header))
                        entryHandlers[entry.Header](thisUserData, entry);
                }
            }

            int hourBreaks = numberHourBreak(thisUserData);
            thisUserData.Close();

            thisUserData.words = playerData.WordStorage.FoundWords.Count;
            thisUserData.phrases = playerData.PhraseStorage.Phrases.Count;

            var row = new DataTableRow();
            table.AddRow(row);

            column = -1;
            row.SetUserName(thisUserData.GetUsername());
            row.SetValue("User", NextColumn(), user);
            row.SetValue("Start time", NextColumn(), ((thisUserData.firstTime - thisUserData.TimeOffsetEnd) - firstTime).TotalHours);
            row.SetValue("Money", NextColumn(), playerData.Money);
            row.SetValue("Level", NextColumn(), playerData.Proficiency.GetReviewLevel());
            row.SetValue("Words", NextColumn(), playerData.WordStorage.FoundWords.Count);
            row.SetValue("Phrases", NextColumn(), playerData.PhraseStorage.Phrases.Count);
            row.SetValue("Reviews: incorrect", NextColumn(), thisUserData.incorrectReviews);
            row.SetValue("Reviews: correct", NextColumn(), thisUserData.correctReviews);
            var ratio = (float)thisUserData.correctReviews / (thisUserData.correctReviews + thisUserData.incorrectReviews);
            row.SetValue("Reviews: ratio", NextColumn(), ratio);
            row.SetValue("Is expert", NextColumn(), ratio > 0.9f ? 1 : 0);
            row.SetValue("Offset time", NextColumn(), thisUserData.TimeOffsetEnd.TotalHours);
            row.SetValue("Chat lines", NextColumn(), thisUserData.chatCount);
            row.SetValue("First chat time", NextColumn(), thisUserData.firstChatTime == default(DateTime) ? "" : (thisUserData.firstChatTime - thisUserData.firstTime).TotalMinutes.ToString());
            row.SetValue("OneOrMoreChatLines", NextColumn(), thisUserData.chatCount > 0 ? 1 : 0);
            row.SetValue("Play sessions", NextColumn(), thisUserData.playSessions.Count);
            row.SetValue("Play minutes", NextColumn(), thisUserData.totalTimePlayed.TotalMinutes);
            row.SetValue("Number of hour break", NextColumn(), hourBreaks);
            row.SetValue("Returned at least once", NextColumn(), hourBreaks > 0 ? 1 : 0);
            row.SetValue("Number of other players online with", NextColumn(), 0);
            row.SetValue("OneOrMoreOtherPlayers", NextColumn(), 0);
            row.SetValue("Within 10 Unit of Another Player", NextColumn(), 0);
            row.SetValue("First saw player", NextColumn(), "");
            var next = NextColumn();
            for (int i = 0; i < thisUserData.playSessions.Count; i++) {
                row.SetValue("Session " + i, next, thisUserData.playSessions[i].TotalMinutes);
            }

            var preRow = preSurvey.GetFirstRowWhere("User", user);
            if (preRow != null) {
                foreach (var val in preRow.values) {
                    row.SetValue("Pre-" + val.Key, 100 + preRow.columnOrder[val.Key], val.Value);
                }
            }

            var postRow = postSurvey.GetFirstRowWhere("User", user);
            if (postRow != null) {
                foreach (var val in postRow.values) {
                    row.SetValue("Post-" + val.Key, 200 + postRow.columnOrder[val.Key], val.Value);
                }
            }
        }

        var chat = (from cl in chatLines
                    orderby cl.Key()
                    select cl.Key() + "\t" + cl.Value)
                    .Aggregate((cur, next) => cur + "\n" + next);
        WriteToFile("chat", chat);

        var reviews = (from r in reviewResults
                       orderby r.Key
                       select r.Key + "\t" + r.Value[0] + "\t" + r.Value[1])
                       .Aggregate((c, n) => c + "\n" + n);
        WriteToFile("review", reviews);

        foreach (var user in reviewTables.Keys) {
            if (userData[user].totalTimePlayed.TotalMinutes > 60) {
                WriteToFile("review_" + user, ClearPerfects(reviewTables[user]).GetFileString());
            }

            var row = table.GetFirstRowWhere("User", user);
            float preScore;
            float postScore;
            int learnedCount;
            var result = GetPreScore(reviewTables[user], out preScore, out postScore, out learnedCount);
            row.SetValue("SRS pre/post data type", 299, result);
            if (result == 2) {
                row.SetValue("SRS pre correctness score", 300, preScore);
                row.SetValue("SRS post correctness score", 301, postScore);
                row.SetValue("Learned word count", 302, learnedCount);
                row.SetValue("Words learned per hour", 303, learnedCount / userData[user].totalTimePlayed.TotalHours);
            }
        }

        var timesQuitTable = new DataTable();
        var vals = from ud in userData
                   orderby ud.Value.totalTimePlayed
                   select new KeyValuePair<string, TimeSpan>(ud.Value.username, ud.Value.totalTimePlayed);
        int remaining = userData.Count;
        var remainingRow = new DataTableRow();
        timesQuitTable.AddRow(remainingRow);
        remainingRow.SetValue("Time", 0, 0);
        remainingRow.SetValue("Remaining", 1, 1f);
        remainingRow.SetValue("Progress", 2, 0);
        var maxProgress = userData.Values.Max(u => u.words + u.phrases);
        var progressHistogram = new int[maxProgress + 1];
        foreach (var val in vals) {
            remainingRow = new DataTableRow();
            timesQuitTable.AddRow(remainingRow);
            remaining--;
            remainingRow.SetValue("Time", 0, val.Value.TotalMinutes);
            remainingRow.SetValue("Remaining", 1, (float)remaining / userData.Count);

            var prog = userData[val.Key].words + userData[val.Key].phrases;
            for(int i = 0; i <= prog; i++){
                progressHistogram[i]++;
            }

            remainingRow.SetValue("Absolute Progress", 2, prog);
            remainingRow.SetValue("Progress", 2, (userData[val.Key].words + userData[val.Key].phrases) / maxProgress);
        }
        WriteToFile("players_remaining", timesQuitTable.GetFileString());

        var s = "Progress\tPercent";
        for(int i = 0; i < progressHistogram.Length; i++){
            s += "\n" + i + "\t" + ((float)progressHistogram[i] / userData.Count);
        }
        WriteToFile("progress_histogram", s);

        var plays = "Time";
        foreach (var u in userData.Values) {
            foreach (var tp in u.GetLongTimePairs()) {
                plays += "\n" + (tp.Start - u.TimeOffsetEnd - firstTime).TotalHours;
            }
        }
        WriteToFile("plays_histogram", plays);

        calculateTime();
        //calculatePosition();
        AppendData();
        WriteToFile("user", table.GetFileString());
        Debug.Log("Chat count: " + chatlineCount);
    }

    void AppendData() {
        string[] file = new string[0];
        using (var reader = new StreamReader(OutPath + "additional_data.txt", true)) {
            file = reader.ReadToEnd().Split('\n');
        }
        //var file = Serializer.LoadFromFile().Split('\n');
        var titles = file[0].Split('\t');
        for (int i = 1; i < file.Length; i++) {
            var rowStrings = file[i].Split('\t');
            var user = rowStrings[0];
            var row = table.GetFirstRowWhere("User", user);
            if (row != null) {
                for (int j = 1; j < titles.Length; j++) {
                    row.SetValue(titles[j], 500 + j, rowStrings[j]);
                }
            } else {
                Debug.Log("null row: " + user);
            }
        }
    }

    int column = -1;
    int NextColumn() {
        column++;
        return column;
    }

    void calculateTime() {
        var logonEvents = new List<PlayerConnectionEvent>();
        foreach (var user in userData.Values) {
            foreach (var session in user.sessionTimes) {
                logonEvents.Add(new PlayerConnectionEvent(session.Start - user.TimeOffsetEnd, true, user.username));
                logonEvents.Add(new PlayerConnectionEvent(session.End - user.TimeOffsetEnd, false, user.username));
            }
        }

        var sortedLogonEvents = logonEvents.OrderBy(kv => kv.time.Ticks);
        var baseTime = sortedLogonEvents.First().time;
        var outputString = "from" + baseTime + "\tusers";
        var usersOnline = 0;
        var usersOnlineTimes = new List<KeyValuePair<DateTime, int>>();
        foreach (var item in sortedLogonEvents) {
            outputString += "\n" + item.time + "\t" + usersOnline;
            if (item.isLogOn) {
                usersOnline++;
            } else {
                usersOnline--;
            }
            outputString += "\n" + item.time + "\t" + usersOnline;

            var p1 = new KeyValuePair<DateTime, int>(item.time, usersOnline);
            usersOnlineTimes.Add(p1);
        }
        Serializer.SaveToFile(OutPath + "usersOnline.txt", outputString);

        foreach (var user in userData.Values) {
            int max = 0;
            int initialMax = 0;
            for (int i = 0; i < user.sessionTimes.Count; i++) {
                max = Mathf.Max(GetMaxUsersOnline(usersOnlineTimes, user, user.sessionTimes[i]), max);
                table.ChangeValue(user.GetUsername(), "Number of other players online with", max);
                table.ChangeValue(user.GetUsername(), "OneOrMoreOtherPlayers", max > 1 ? 1 : 0);
            }
        }

        var current = new HashSet<string>();
        foreach (var e in sortedLogonEvents) {
            if (!userOnlineWithDictionary.ContainsKey(e.user)) {
                userOnlineWithDictionary[e.user] = new HashSet<string>();
            }
            
            if (e.isLogOn) {
                current.Add(e.user);
            } else {
                current.Remove(e.user);
            }

            foreach (var user in current) {
                userOnlineWithDictionary[user].UnionWith(current);
            }
        }
    }

    int GetMaxUsersOnline(List<KeyValuePair<DateTime, int>> usersOnlineGraph, UserDataEntry user, TimePair session) {
        int i = 0;
        int max = 0;
        while (usersOnlineGraph[i].Key < session.Start - user.TimeOffsetEnd) {
            i++;
            if (i >= usersOnlineGraph.Count) {
                Debug.Log("i: " + i + "; " + usersOnlineGraph.Count);
                return 0;
            }
        }

        while (usersOnlineGraph[i].Key < session.End - user.TimeOffsetEnd) {
            max = Mathf.Max(usersOnlineGraph[i].Value, max);
            i++;
        }
        return max;
    }

    void calculatePosition() {
        foreach (var user in userOnlineWithDictionary.Keys) {
            if (!userData[user].enteredOpenWorld) {
                table.ChangeValue(user, "Within 10 Unit of Another Player", "");
                continue;
            }

            //Debug.Log("user group: " + userOnlineWithDictionary[user].Count);
            foreach (var other in userOnlineWithDictionary[user]) {
                if (user == other) continue;
                if (!userData[other].enteredOpenWorld) continue;

                DateTime seenTime;
                if (WithinNMeters(10f, userData[user], userData[other], out seenTime)) {
                    table.ChangeValue(user, "First saw player", (seenTime - userData[user].firstTime).TotalMinutes);
                    table.ChangeValue(user, "Within 10 Unit of Another Player", 1);
                    continue;
                }
            }
        }
    }

    bool WithinNMeters(float meters, UserDataEntry user1, UserDataEntry user2, out DateTime seenTime) {
        var maxTimeDistance = new TimeSpan(0, 0, 1);
        var distSquared = meters * meters;
        foreach (var e1 in user1.positionData) {
            if (!e1.isOutside) continue;

            foreach (var e2 in user2.positionData) {
                if (!e2.isOutside) continue;
                if (e2.time > (e1.time + maxTimeDistance)) break;

                var positionDifference = (e1.position - e2.position).sqrMagnitude;
                var timeDifference = Math.Abs(((e1.time - user1.TimeOffsetEnd) - (e2.time - user2.TimeOffsetEnd)).Ticks);
                if (timeDifference < maxTimeDistance.Ticks && positionDifference < distSquared) {
                    Debug.Log(e1.position + ": " + (e1.time - user1.TimeOffsetEnd) + "; " + e2.position + ": " + (e2.time - user2.TimeOffsetEnd));
                    seenTime = e1.time;
                    return true;
                }
            }
        }
        seenTime = new DateTime();
        return false;
    }

    void calculateOnlineWithOthers() {
        foreach (var item in userData) {
            foreach (var item1 in userData) {
                if (item.Key != item1.Key) {
                    if (commonTime(item.Value.sessionTimes, item1.Value.sessionTimes)) {
                        item.Value.onlineWithSomeoneElse = true;
                        item1.Value.onlineWithSomeoneElse = true;
                    }
                }
            }
        }
    }

    private Boolean commonTime(List<TimePair> l1, List<TimePair> l2) {
        foreach (var t1 in l1) {
            foreach (var t2 in l2) {
                if (t1.shareTime(t2)) {
                    return true;
                }
            }
        }
        return false;
    }

    int numberHourBreak(UserDataEntry user1) {
        int number1 = 0;
        for (int i = 0; i < user1.sessionTimes.Count - 1; i++) {
            if ((user1.sessionTimes[i + 1].Start - user1.sessionTimes[i].End).TotalMinutes > 60.0) {
                number1 = number1 + 1;
            }
        }
        return number1;
    }

    void ReadPlayerData() {
        var files = Directory.GetFiles(FullPath, "*.xml").Where(s => !s.Contains("UsernamePasswordTable"));
        var content = "";
        foreach (var f in files) {
            var pd = Serializer.LoadFromXml<PlayerData>(f, false);
        }
    }

    void WriteToFile(string fileName, string content) {
        using (var writer = new StreamWriter(OutPath + fileName + ".txt", false)) {
            writer.Write(content);
        }
    }

    void HandleChat(UserDataEntry user, LogDataEntry data) {
        user.AddChatLine(data.GetDataTime(), data.Data[0]);
        chatLines.Add(new KeyValuePair<Func<DateTime>, string>(() => data.GetDataTime() - user.TimeOffsetEnd, user.username + ": " + data.Data[0]));
        chatlineCount++;
    }

    void HandlePosition(UserDataEntry user, LogDataEntry data) {
        user.AddPosition(data.GetDataTime(), data.Data);
    }

    void HandleReview(UserDataEntry user, LogDataEntry data) {
        var key = data.Data[0];
        var result = 0;
        Dictionary<string, int[]> individualResult = new Dictionary<string, int[]>();
        DateTime t1 = data.GetDataTime() - user.TimeOffsetEnd;
        foreach (var tpair in user.sessionTimes) {
            if (tpair.Start < t1 && tpair.End > t1) {
                if (data.Data[1] == "0") {
                    tpair.numberIncorrect++;
                } else {
                    tpair.numberCorrect++;
                }
            }
        }

        if (!reviewTables.ContainsKey(user.username)) {
            reviewTables[user.username] = new DataTable();
        }

        if (!reviewResults.ContainsKey(key)) {
            reviewResults[key] = new int[2];
        }

        if (data.Data[1] == "0") {
            reviewResults[key][0]++;
            user.incorrectReviews++;
            result = 0;
        } else {
            reviewResults[key][1]++;
            user.correctReviews++;
            result = 1;
        }


        var revTable = reviewTables[user.username];
        var revColumn = "Key";
        var timeName = key + "-time";
        var valName = key + "-result";
        var timeRow = revTable.GetFirstRowWhere(revColumn, timeName);
        if (timeRow == null) {
            timeRow = new DataTableRow();
            timeRow.SetValue(revColumn, 0, timeName);
            revTable.AddRow(timeRow);
        }
        var valRow = revTable.GetFirstRowWhere(revColumn, valName);
        if (valRow == null) {
            valRow = new DataTableRow();
            valRow.SetValue(revColumn, 0, valName);
            revTable.AddRow(valRow);
        }
        int count = 0;
        while (timeRow.values.ContainsKey(count.ToString())) {
            count++;
        }
        timeRow.SetValue(count.ToString(), count + 10, data.GetDataTime());
        valRow.SetValue(count.ToString(), count + 10, result);
    }

    DataTable ClearPerfects(DataTable table) {
        var newTable = new DataTable();
        foreach (var row in table.rows) {
            var key = (string)row.GetValue("Key");
            if (key.Contains("-result")) {
                if (!IsPerfect(row)) {
                    var timeKey = key.Replace("-result", "") + "-time";
                    newTable.AddRow(table.GetFirstRowWhere("Key", timeKey));
                    newTable.AddRow(row);
                }
            }
        }
        return newTable;
    }

    bool IsPerfect(DataTableRow row) {
        int count = 0;
        while (row.values.ContainsKey(count.ToString())) {
            if ((int)row.GetValue(count.ToString()) == 0) {
                return false;
            }
            count++;
        }
        return true;
    }

    int GetPreScore(DataTable table, out float preScore, out float postScore, out int learnedCount) {
        learnedCount = 0;
        float preCorrect = 0;
        float preTotal = 0;
        float postCorrect = 0;
        float postTotal = 0;
        foreach (var row in table.rows) {
            var key = (string)row.GetValue("Key");
            if (key.Contains("-result")) {
                if (!IsPerfect(row)) {
                    var l = new List<int>(row.GetOrderedValues().Where(o => o is int).Cast<int>());
                    if (l.Count > 2) {
                        int localPreCorrect = 0;
                        int localPreTotal = 0;
                        int localPostCorrect = 0;
                        int localPostTotal = 0;

                        for (int i = 0; i < 2; i++) {
                            localPreCorrect += l[i];
                            localPreTotal++;
                        }

                        for (int i = 2; i < l.Count; i++) {
                            localPostCorrect += l[i];
                            localPostTotal++;
                        }

                        var preRatio = (float)localPreCorrect / localPreTotal;
                        var postRatio = (float)localPostCorrect / localPostTotal;
                        preCorrect += preRatio;
                        postCorrect += postRatio;
                        preTotal += 1f;
                        postTotal += 1f;
                        learnedCount++;
                    }
                }
            }
        }
        if (preTotal == 0) {
            preScore = -1f;
            postScore = -1f;
            if (table.rows.Count == 0) {
                return 1;
            } else {
                return 0;
            }
        } else {
            preScore = preCorrect / preTotal;
            postScore = postCorrect / postTotal;
            return 2;
        }
    }
}

//void ReadLogData() {
//    var dirInfo = new DirectoryInfo(FullPath);
//    var allLogs = dirInfo.GetDirectories();
//    foreach (var dir in allLogs) {
//        var log = new List<LogDataClass>();
//        Logs[dir.Name] = log;
//        var allFiles = dir.GetFiles();
//        durations[dir.Name] = new TimeSpan();
//        foreach (var file in allFiles) {
//            bool isFirst = true;
//            var start = new DateTime(0);
//            foreach (string line in File.ReadAllLines(file.FullName)) {
//                var data = ParseLogData(line);
//                log.Add(data);

//                if (isFirst) {
//                    start = data.GetDataTime();
//                    isFirst = false;
//                }
//            }
//            durations[dir.Name] += log[log.Count - 1].GetDataTime() - start;
//        }
//    }
//    Debug.Log("done reading");


//    //test purpose
//    string testWrite = OutPath + "log_analysis.txt";
//    Dictionary<string, int> jobsSelected = new Dictionary<string, int>();
//    using (StreamWriter sw = new StreamWriter(testWrite, false)) {
//        sw.WriteLine("Player\tPlaytime\tMoney\tTotal words\tStrong words\tCorrect\tWrong\tJobs");
//        TimeSpan totalTime = new TimeSpan();
//        foreach (var key in Logs.Keys) {
//            var log = Logs[key];
//            var pd = Serializer.LoadFromXml<PlayerData>(FullPath + key + ".xml");

//            sw.Write(key);

//            var dur = durations[key];
//            //sw.Write("\t" + string.Format("{0}:{1:00}", dur.Hours, dur.Minutes));
//            sw.Write("\t" + string.Format("{0}", dur.TotalMinutes));
//            totalTime += durations[key];

//            sw.Write("\t" + pd.Money);

//            sw.Write("\t" + pd.Reviews.Reviews.Count);

//            sw.Write("\t" + pd.Reviews.GetReviewsForRank(2).Count());

//            int correctCount = 0;
//            int wrongCount = 0;
//            int jobs = 0;
//            HashSet<string> uniqueJobs = new HashSet<string>();
//            foreach (var v in Logs[key]) {
//                if (v.Header == "Review") {
//                    if (v.Data.Length < 2) {
//                        var s = "";
//                        foreach (var d in v.Data) {
//                            s += "\t" + d;
//                        }
//                        Debug.Log(s);
//                    } else {
//                        if (v.Data[1] == "0") {
//                            wrongCount++;
//                        } else {
//                            correctCount++;
//                        }
//                    }
//                } else if (v.Header == "JobSelected") {
//                    if (jobsSelected.ContainsKey(v.Data[0])) {
//                        jobsSelected[v.Data[0]]++;
//                    } else {
//                        jobsSelected[v.Data[0]] = 1;
//                    }

//                    uniqueJobs.Add(v.Data[0]);
//                    jobs++;
//                }
//            }
//            sw.Write("\t" + correctCount);
//            sw.Write("\t" + wrongCount);
//            sw.Write("\t" + jobs);

//            sw.WriteLine();
//        }
//        Debug.Log("Total time: " + totalTime);
//    }

//    var jobFile = OutPath + "job_analysis.txt";
//    var sortedJobs = jobsSelected.OrderBy(kv => kv.Value);
//    using (StreamWriter sw = new StreamWriter(jobFile, false)) {
//        foreach (var j in sortedJobs) {
//            sw.WriteLine(j.Key + "\t" + j.Value);
//        }
//    }

//    Debug.Log("done writing");
//}
