using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using D = System.Collections.Generic.List<LeaderBoardDataItem<int>>;

public class LeaderBoardGameData {

    public D MoneyLeaders { get; set; }
    public D WordLeaders { get; set; }
    //	public D PhraseLeaders;

    public LeaderBoardGameData(D money, D words) { //D phrases){
        MoneyLeaders = money;
        WordLeaders = words;
        //		_phrases = phrases;
    }

    public LeaderBoardGameData() {
        MoneyLeaders = new D();
        WordLeaders = new D();
    }

    public override string ToString() {
        var s = "Money\tWord\n";
        for (int i = 0; i < MoneyLeaders.Count; i++) {
            s += MoneyLeaders[i].Name + ": " + MoneyLeaders[i].Data;
            s += "\t";
            s += WordLeaders[i].Name + ": " + WordLeaders[i].Data;
            s += "\n";
        }
        return s;
    }

}
