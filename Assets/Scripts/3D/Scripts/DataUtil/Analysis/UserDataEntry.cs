using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerConnectionEvent {
    public DateTime time;
    public bool isLogOn;
    public string user;

    public PlayerConnectionEvent(DateTime time, bool isLogOn, string user) {
        this.time = time;
        this.isLogOn = isLogOn;
        this.user = user;
    }
}

public class PlayerPositionEvent {
    public DateTime time;
    public bool isOutside;
    public Vector3 position;

    public PlayerPositionEvent(DateTime time, bool isOutside, Vector3 position) {
        this.time = time;
        this.isOutside = isOutside;
        this.position = position;
    }
}

public class UserDataEntry {

    static Vector3 homeCenter = new Vector3(-3.3f, 0, 19.8f);
    static Vector3 openWorldStart = new Vector3(155.3f, -1.3f, -1.4f);

    public string username = "";
    public DateTime created = new DateTime();
    public DateTime lastModified = new DateTime();
    public TimeSpan totalTimePlayed = new TimeSpan();
    public List<TimeSpan> playSessions = new List<TimeSpan>();
    public int correctReviews = 0;
    public int incorrectReviews = 1;
    public int words = 0;
    public int phrases = 0;
    public int chatCount = 0;
    public bool enteredOpenWorld = false;
    public List<TimePair> sessionTimes = new List<TimePair>();
    public Boolean onlineWithSomeoneElse = false;
    public List<PlayerPositionEvent> positionData = new List<PlayerPositionEvent>();

    public TimeSpan TimeOffsetEnd { get { return lastTime - lastModified; } }

    public DateTime firstTime = default(DateTime);
    public DateTime firstChatTime = default(DateTime);
    DateTime lastTime = default(DateTime);
    DateTime sessionStart = default(DateTime);

    public UserDataEntry(string name) {
        this.username = name;
    }

    public string GetUsername() {
        return username;
    }

    public void AddTime(DateTime time) {
        if (firstTime == default(DateTime)) {
            firstTime = time;
        }

        if (lastTime == default(DateTime)) {
            lastTime = time;
            sessionStart = time;
        } else {
            if ((time - lastTime).TotalMinutes < 10.0) {
                totalTimePlayed += time - lastTime;
            } else {
                playSessions.Add(lastTime - sessionStart);

                TimePair tpair = new TimePair(sessionStart, lastTime);
                sessionTimes.Add(tpair);

                sessionStart = time;
            }
            lastTime = time;
        }
    }

    public void AddChatLine(DateTime time, string line) {
        if (firstChatTime == default(DateTime)) {
            firstChatTime = time;
        }
        chatCount++;
    }

    public void AddPosition(DateTime time, string[] positionString) {
        var pos = new Vector3();
        var floats = positionString[0].Replace("(", "").Replace(")", "").Split(',');
        pos.x = float.Parse(floats[0]);
        pos.y = float.Parse(floats[1]);
        pos.z = float.Parse(floats[2]);

        if ((openWorldStart - pos).sqrMagnitude < 1f) {
            enteredOpenWorld = true;
        }

        bool isOutside = false;
        if (enteredOpenWorld && (pos - homeCenter).sqrMagnitude > 5f * 5f) {
            isOutside = true;
        }

        var kvp = new PlayerPositionEvent(time, isOutside, pos);
        positionData.Add(kvp);
    }

    public void Close() {
        playSessions.Add(lastTime - sessionStart);
        TimePair tpair = new TimePair(sessionStart, lastTime);
        sessionTimes.Add(tpair);
    }

    public List<TimePair> GetLongTimePairs() {
        var newList = new List<TimePair>();
        TimePair lastTimePair = null;
        var oneHour = new TimeSpan(1, 0, 0);
        foreach (var tp in sessionTimes) {
            if (lastTimePair == null) {
                newList.Add(tp);
            } else if (tp.Start - lastTimePair.End > oneHour) {
                newList.Add(tp);
            }
            lastTimePair = tp;
        }
        return newList;
    }
}
