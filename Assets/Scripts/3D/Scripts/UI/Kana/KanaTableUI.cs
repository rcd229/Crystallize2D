using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

[ResourcePath("UI/KanaTable")]
public class KanaTableUI : UIPanel, ITemporaryUI<object, object> {

    const string tableEntries = ""
        + "あ,い,う,え,お,"
        + "か,き,く,け,こ,"
        + "さ,し,す,せ,そ,"
        + "た,ち,つ,て,と,"
        + "な,に,ぬ,ね,の,"
        + "は,ひ,ふ,へ,ほ,"
        + "ま,み,む,め,も,"
        + "や,.,ゆ,.,よ,"
        + "ら,り,る,れ,ろ,"
        + "わ,.,.,.,を,"
        + "ん,"
        + "が,ぎ,ぐ,げ,ご,"
        + "ざ,じ,ず,ぜ,ぞ,"
        + "だ,ぢ,づ,で,ど,"
        + "ば,び,ぶ,べ,ぼ,"
        + "ぱ,ぴ,ぷ,ぺ,ぽ,"
        + "きゃ,.,きゅ,.,きょ,"
        + "しゃ,.,しゅ,.,しょ,"
        + "ちゃ,.,ちゅ,.,ちょ,"
        + "にゃ,.,にゅ,.,にょ,"
        + "ひゃ,.,ひゅ,.,ひょ,"
        + "みゃ,.,みゅ,.,みょ,"
        + "りゃ,.,りゅ,.,りょ,"
        + "っ,"
        ;

    public static int HiraganaCount() {
        return tableEntries.Replace(".", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static string[] AllHiragana() {
        return tableEntries.Replace(".", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    public GameObject kanaBlockPrefab;
    public GameObject emptyPrefab;
    public RectTransform blockParent;

    public event EventHandler<EventArgs<object>> Complete;

    public void Initialize(object param1) {
        
    }

    void Start() {
        UIUtil.GenerateChildren(tableEntries.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), blockParent, CreateChild);

        CrystallizeEventManager.Input.OnEnvironmentClick += Input_OnEnvironmentClick;
    }

    void OnDestroy() {
        CrystallizeEventManager.Input.OnEnvironmentClick -= Input_OnEnvironmentClick;
    }

    void Input_OnEnvironmentClick(object sender, EventArgs e) {
        Close();
    }

    GameObject CreateChild(string s) {
        if (s == ".") {
            var instance = Instantiate<GameObject>(emptyPrefab);
            return instance;
        } else {
            var instance = Instantiate<GameObject>(kanaBlockPrefab);
            instance.GetInterface<IInitializable<string>>().Initialize(s.ToString());
            return instance;
        }
    }

    public override void Close() {
        base.Close();
        Complete.Raise(this, new EventArgs<object>(null));
    }

}
