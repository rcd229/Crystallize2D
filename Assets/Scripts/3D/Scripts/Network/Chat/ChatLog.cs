using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChatLog {

    static ChatLog _instance;
    public static ChatLog Instance {
        get {
            if (_instance == null) {
                _instance = new ChatLog();
            }
            return _instance;
        }
    }

    public event EventHandler<EventArgs<string>> ChatLogChanged;

    List<string> lines = new List<string>();

    public IEnumerable<string> ChatLines { get { return lines; } }

    public void EnterLine(string line, int mode) {
        if (CrystallizeNetwork.Connected) {
            CrystallizeNetwork.Client.SendChatToServer(line, mode);
        } else {
            AddLine(-1, line, mode);
        }
    }

    public void OpenSpeechBubble(PhraseSequence phrase) {
        if (CrystallizeNetwork.Connected) {
            CrystallizeNetwork.Client.SendSpeechBubbleToAll(phrase);
        }
    }

    public void AddLine(int playerID, string line, int mode) {
        var moddedLine = line;
        if (mode == (int)ChatMode.Shout) {
            moddedLine = "<color=yellow>" + line + "</color>";
        }

        if (CrystallizeNetwork.Connected && CrystallizeNetwork.ConnectionID == playerID) {
            playerID = -1;
        }

        if ((CrystallizeNetworkScene.Instance && CrystallizeNetworkScene.Instance.DistanceToPlayer(playerID) < 30f)
            || mode == (int)ChatMode.Shout
            || playerID == -1) {
            lines.Add(moddedLine);
            ChatLogChanged.Raise(this, new EventArgs<string>(line));
        }
    }

}
