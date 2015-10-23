using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IQuestReward {
    string RewardDescription { get; }
    void GrantReward();
}

public interface IHasQuestReward {
    IQuestReward GetReward(QuestRef quest);
}

public class QuestReward : IQuestReward {

    public static QuestReward Money(int amount) {
        return new QuestReward("Reward: ¥" + amount, () => PlayerDataConnector.AddMoney(amount));
    }

    public static QuestReward Clothing(BuyableClothes clothes) {
        return new QuestReward("Reward: " + clothes.Name, () => clothes.AfterBuyItem());
    }

    public static QuestReward Furniture(BuyableFurniture furnature) {
        return new QuestReward("Reward: " + furnature.Name, () => furnature.AfterBuyItem());
    }

    public string RewardDescription { get; private set; }

    Action grant;

    public QuestReward(string description, Action grant) {
        RewardDescription = description;
        this.grant = grant;
    }

    public void GrantReward() {
        grant.Raise();
    }
}
