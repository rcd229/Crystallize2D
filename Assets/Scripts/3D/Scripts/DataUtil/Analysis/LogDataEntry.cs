using UnityEngine;
using System;
using System.Collections;

public class LogDataEntry {
    public string TimeStamp;
    public string Header;
    public string[] Data;
    public LogDataEntry(string line) {
        var split = line.Split(new char[] { '\t' }, 3);
        TimeStamp = split[0];
        Header = split[1];

        if (Header == "Chat") {
            Data = new string[] { split[2] };
        }
        /*else if (Header == "Position")
        {
            Data = new string[3];
            Data[0] = split[2].Substring(1, split[2].IndexOf(',') - 1);
            string secondSlice = split[2].Substring(split[2].IndexOf(' ') + 1);
            Data[1] = secondSlice.Substring(0, secondSlice.IndexOf(','));
            string thirdSlice = secondSlice.Substring(split[2].IndexOf(' ') + 1);
            //Data[2] = thirdSlice.Substring(0, secondSlice.IndexOf(')') - 1);
        }*/
        else {
            if (split.Length > 2) {
                Data = split[2].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            } else {
                Data = new string[] { "" };
            }
        }
    }

    public LogDataEntry(string timeStamp, string header, string data) {
        TimeStamp = timeStamp;
        Header = header;
        Data = data.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    public DateTime GetDataTime() {
        long ticks;
        if (long.TryParse(TimeStamp, out ticks)) {
            return new DateTime(ticks);
        } else {
            Debug.LogError("cannot parse date time");
            return DateTime.Now;
        }
    }
}
