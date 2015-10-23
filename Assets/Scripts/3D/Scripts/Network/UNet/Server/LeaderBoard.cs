using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LeaderBoard {

    private Dictionary<string, int> playerWords = new Dictionary<string, int>();
    private Dictionary<string, int> playerMoney = new Dictionary<string, int>();

    public LeaderBoard() {

    }

    public void Update(string name, int words, int money) {
        playerWords[name] = words;
        playerMoney[name] = money;
    }

    public LeaderBoardGameData GetTopN(int count) {
        List<LeaderBoardDataItem<int>> top_money = (from item in playerMoney.ToList()
                                                    orderby item.Value descending
                                                    select new LeaderBoardDataItem<int>(item.Key, item.Value)
                                                     ).Take(count).ToList();
        List<LeaderBoardDataItem<int>> top_word = (from item in playerWords.ToList()
                                                   orderby item.Value descending
                                                   select new LeaderBoardDataItem<int>(item.Key, item.Value)
                                                      ).Take(count).ToList();
        return new LeaderBoardGameData(top_money, top_word);
    }

}
