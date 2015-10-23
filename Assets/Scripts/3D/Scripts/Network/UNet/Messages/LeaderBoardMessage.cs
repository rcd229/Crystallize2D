using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LeaderBoardMessage : MessageBase {

    public string name = "";
    public int words = 0;
    public int money = 0;

    public LeaderBoardMessage() { }

    public LeaderBoardMessage(string name, int words, int money) {
        this.name = name;
        this.words = words;
        this.money = money;
    }

}
