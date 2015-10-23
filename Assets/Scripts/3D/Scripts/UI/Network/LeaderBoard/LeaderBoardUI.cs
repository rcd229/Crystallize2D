using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class LeaderBoardUI : UIPanel, ITemporaryUI<object, object> {

	const string ResourcePath = "UI/LeaderBoard";
	public static LeaderBoardUI GetInstance() {
		return GameObjectUtil.GetResourceInstance<LeaderBoardUI>(ResourcePath);
	}

	private LeaderBoardGameData debugData;

	private LeaderBoardGameData DebugData{
		get{
			if(debugData == null){
				var ml = new List<LeaderBoardDataItem<int>>();
				var wl = new List<LeaderBoardDataItem<int>>();
				ml.Add(new LeaderBoardDataItem<int>("test1", 1));
				ml.Add(new LeaderBoardDataItem<int>("test2", 10));
				ml.Add(new LeaderBoardDataItem<int>("test3", 100000));
				wl.Add(new LeaderBoardDataItem<int>("test4", 2));
				wl.Add(new LeaderBoardDataItem<int>("test5", 20));
				wl.Add(new LeaderBoardDataItem<int>("test6", 200000));
				debugData = new LeaderBoardGameData(ml,	wl);		
			}
			return debugData;
		}
	}

	public UIButton CloseButton;

	public event EventHandler<EventArgs<object>> Complete;
	public Text LoadingText;

//	public GameObject indiceField;
//	public GameObject moneyField;
//	public GameObject wordField;

	public Text indiceText;
	public Text wordPersonText;
	public Text wordCountText;
	public Text moneyPersonText;
	public Text moneyCountText;
//	GameObject phraseField;

//	string indiceString;
	string wordPersonString;
	string wordCountString;
	string moneyPersonString;
	string moneyCountString;
	string loadString;
	bool isDebug = GameSettings.Instance.Local;

	IEnumerator WaitAndInitialize(){
		yield return null;

		while(!isDebug && !Network.isClient){
			yield return null;
		}
		if(!isDebug){
			Debug.Log("leader board data sent");
            CrystallizeNetwork.Client.RequestLeaderboardFromServer(HandleLeaderBoardResponse);
			//RPCFunctions.Instance.RequestLeaderboardFromServer();
			//RPCFunctions.Instance.LeaderBoardResponse += HandleLeaderBoardResponse;
		}
	}
	public void Initialize(object param) {
		Debug.Log("leader board initialized");
		CoroutineManager.Instance.StartCoroutine(WaitAndInitialize());
		CloseButton.OnClicked += HandleOnClicked;
		Complete += HandleComplete;
//		indiceString = indiceText.text;
		wordPersonString = wordPersonText.text;
		wordCountString = wordCountText.text;
		moneyPersonString = moneyPersonText.text;
		moneyCountString = moneyCountText.text;
		loadString = LoadingText.text;

//		indiceField = gameObject.transform.Find("LeaderBoardData/Indices").gameObject;
//		moneyField = gameObject.transform.Find("LeaderBoardData/money").gameObject;
//		wordField = gameObject.transform.Find("LeaderBoardData/words").gameObject;
        if (isDebug) {
            HandleLeaderBoardResponse(DebugData);
        }
	}

	void HandleOnClicked (object sender, EventArgs e)
	{
		Complete.Raise(this, null);
	}

	void HandleComplete (object sender, EventArgs<object> e)
	{
		GameObject.Destroy(this.gameObject);
	}

	void HandleLeaderBoardResponse (LeaderBoardGameData data)
	{
		Debug.Log("callback invoked");
		foreach(var v in data.MoneyLeaders){
			Debug.Log("received " + data + " " + v.Name);
		}
		loadString = "Leader Board";
		moneyPersonString = "";
		moneyCountString = "";
		foreach(var money in data.MoneyLeaders){
			if(moneyCountString == ""){
				moneyPersonString = money.Name;
				moneyCountString = ": " + money.Data.ToString();
			}
			else{
				moneyPersonString = moneyPersonString + '\n' + money.Name;
				moneyCountString = moneyCountString + '\n' + ": " + money.Data.ToString();
			}
		}
		wordPersonString = "";
		wordCountString = "";
		foreach(var word in data.WordLeaders){
			if(wordCountString == ""){
				wordPersonString = word.Name;
				wordCountString = ": " + word.Data.ToString();
			}
			else{
				wordPersonString = wordPersonString + '\n' + word.Name;
				wordCountString = wordCountString + '\n' + ": " + word.Data.ToString();
			}
		}

	}

	void Update(){
		LoadingText.text = loadString;
//		if(isDebug){
//			populateField<int>(new int[]{1,2, 3, 4, 6}, moneyText);
//			populateField<int>(new int[]{1,2, 3, 4, 5}, wordText);
//			//			populateField<int>(new int[]{1,2, 3, 4}, phraseField);
//			return;
//		}
		moneyCountText.text = moneyCountString;
		moneyPersonText.text = moneyPersonString;
		wordCountText.text = wordCountString;
		wordPersonText.text = wordPersonString;
	}

	//for debug






}
