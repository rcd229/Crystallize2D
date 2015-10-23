using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class HelpData {

    public static IEnumerable<HelpData> GetValues() {
        return from f in typeof(HelpData).GetFields(BindingFlags.Static | BindingFlags.Public)
               where f.FieldType == typeof(HelpData)
               select f.GetValue(null) as HelpData;
    }

    public static readonly HelpData LearnWords = new HelpData("Learn words and phrases",
        "You can learn words by dragging them from speech bubbles to the inventory at the bottom or clicking on them.",
        "Entire phrases can by learned with the star in speech bubbles.",
        "When your inventory is full, you will need to review words at home before you can learn more new words.");
    public static readonly HelpData Confidence = new HelpData("Confidence",
        "Your confidence will decrease when you <b>see words that you don't know</b> or <b>you say the wrong thing</b> in conversations.",
        "You can restore your confidence by <b>waiting</b> or <b>doing reviews</b>.",
        "You can also increase your maximum confidence by <b>doing reviews</b>.");
    public static readonly HelpData Home = new HelpData("Home",IconType.Home,
        "You can go to your home by clicking the <b>home</b> button in the lower left of the screen.",
        "You can review words and change clothes at home.");
    public static readonly HelpData Review = new HelpData("Review",
        "You can review words at home. You can go home by clicking the <b>home</b> button in the bottom left of the screen.",
        "Reviewing words can recover your confidence and open open inventory slots for learning new words.");
    public static readonly HelpData Money = new HelpData("Money and shops", IconType.ShoppingCart,
        "Money will allow you to buy things from shops.",
        "You can purchase words, items and other useful things.",
        "Looks for characters with the shopping icon to buy things.");
    public static readonly HelpData Equipment = new HelpData("Clothes", 
        "You can change your clothes at home.",
        "Different clothes may give you bonuses such as increasing the money you earn doing jobs or confidence");
    public static readonly HelpData Job = new HelpData("Jobs", IconType.Briefcase,
        "You can earn money by doing jobs.",
        "Look for characters with the job icon to do jobs.",
        "You may need to learn certain words before you can do a job.");
    public static readonly HelpData Quests = new HelpData("Quests", IconType.Scroll,
        "Some people have have special tasks for you to do called quests.",
        "Doing quests will help you improve your relationship with the person and have other benefits as well.");

    public string Title { get; private set; }
    public string Content { get; private set; }
    public IconType Icon {get; private set;}

    public HelpData(string title, params string[] content) {
        this.Title = title;
        this.Content = "";
        foreach (var s in content) {
            this.Content += s + "\n\n";
        }
    }

    public HelpData(string title, IconType icon, params string[] content) : this(title, content) {
        this.Icon = icon;
    }
}
