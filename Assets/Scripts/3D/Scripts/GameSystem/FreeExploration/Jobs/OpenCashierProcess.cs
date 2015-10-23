using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OpenCashierProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

	const string CashierBossName = "CashierBoss";

	CashierTaskData taskData;
	JobTaskRef taskref;
	
	int numItems;
	int price;
	ValuedItem[] nowItem;
	List<ValuedItem> prices;
	int currentCount;

	List<GameObject> instances;
	List<GameObject> arrows;
	GameObject actor;
	GameObject boss;
	
	#region IInitializable implementation
	public void Initialize (JobTaskRef param1)
	{
		taskref = param1;
		taskData = (CashierTaskData)param1.Data;
		numItems = 1 + ProgressUtil.GetAmountForTiers(taskref.Variation, 4);
		nowItem = new ValuedItem[numItems];
		prices = taskData.ShopLists;
		currentCount = GetTaskCount();
		instances = new List<GameObject>();
		arrows = new List<GameObject>();
		boss = GetBoss();
		GenerateActors();
		BossConversation();
	}

	void BossConversation(){
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(boss, taskData.Dialogues[0], null, true), BossConversationEnd, this);
	}

	void BossConversationEnd(object sender, object arg){
		ProcessLibrary.EndConversation.Get(new ConversationArgs(boss, taskData.Dialogues[0]), BeginExplore, this);
	}

	void BeginExplore(object sender, object arg){
		GetNewTargetPrice();
		boss.name = CashierBossName;
		ProcessLibrary.Explore.Get(new ExploreInitArgs(null, TagLibrary.JobInteractable, "click to talk to him", "not now"), HandleExitExplore, this);  
	}

	void HandleExitExplore(object sender, ExploreResultArgs obj) {
		
		if (obj.Target == null) {
			BeginExplore(null, null);
			Debug.Log("No object returned.");
		} else {
			actor = obj.Target;
			var dialogue = taskData.Dialogues[1];
			ProcessLibrary.BeginConversation.Get(new ConversationArgs(actor, dialogue, GetNewPriceContext(), true), HandlePriceAsked, this);
		}
	}
	
	void HandlePriceAsked(object sender, object arg){
		UILibrary.NumberEntry.Get(null, ui_Complete, this);
	}
	
	void ui_Complete(object sender, EventArgs<int> e){
		var dialogue = taskData.Dialogues[1];
		if (e.Data == price) {
			EventHandler<EventArgs<object>> callback = (o, arg) => {
				ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), Feedback_Complete, this);
			};
			UILibrary.PositiveFeedback.Get("", callback, this);
		}
		else {
			EventHandler<EventArgs<object>> callback = (o, arg) => 
				ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), Feedback_Complete, this);
			UILibrary.NegativeFeedback.Get("", callback, this);
		}
		currentCount--;
	}
	
	void Feedback_Complete(object sender, object arg){
		if(currentCount > 0){
			BeginExplore(null, null);
		}else{
			Exit();
		}
	}
	
	#endregion
	#region IProcess implementation
	public void ForceExit ()
	{
		Exit();
	}
	public ProcessExitCallback OnExit {get;set;}
	#endregion
	
	void Exit(){
		foreach(var go in instances){
			if(go){
				GameObject.Destroy(go);
			}
		}
		foreach(var go in arrows){
			if(go){
				GameObject.Destroy(go);
			}
		}
		OnExit.Raise(this, null);
	}
	
	void GetNewTargetPrice(){
		nowItem = new ValuedItem[numItems];
		Debug.Log("numitems " + numItems);
		var priceArray = prices.ToArray ();
		int wordRange = Math.Min(priceArray.Length, 1 + ProgressUtil.GetAmountForTiers(taskref.Variation, 0, 0, 1, 1, 2, 2));
		for (int i = 0; i < numItems; i++) {
			nowItem[i] = priceArray[UnityEngine.Random.Range(0, wordRange)];
			Debug.Log(nowItem[i].Text);
		}
		price = 0;
		foreach (var item in nowItem) {
			price += item.Value;
		}
	}
	
	ContextData GetNewPriceContext() {
		var c = new ContextData();
		//TODO How to make this more flexible/dynamic?
		
		PhraseSequence contextPhrase = new PhraseSequence();
		if(nowItem.Length > 0){
			var firstItem = nowItem[0];
			BuildPriceContextString(firstItem, contextPhrase);
			if(nowItem.Length > 1){
				for(int i = 1; i < nowItem.Length - 1; i++){
					contextPhrase.PhraseElements.AddRange(taskData.andPhrase.PhraseElements);
					contextPhrase.Translation += " ";
					BuildPriceContextString(nowItem[i], contextPhrase);
				}
				//add the and phrase
				contextPhrase.PhraseElements.AddRange(taskData.andPhrase.PhraseElements);
				contextPhrase.Translation += " and ";
				BuildPriceContextString(nowItem[nowItem.Length - 1], contextPhrase);
			}
			
		}
		c.Set("item", contextPhrase);
		return c;
	}
	
	void BuildPriceContextString(ValuedItem item, PhraseSequence phrase){
		phrase.PhraseElements.AddRange(item.Text.PhraseElements);
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, "("));
		phrase.PhraseElements.AddRange(item.ValueText.PhraseElements);
		phrase.PhraseElements.AddRange(taskData.yenPhrase.PhraseElements);
		phrase.Add(new PhraseSequenceElement(PhraseSequenceElementType.Text, ")"));
		phrase.Translation += item.Text.Translation;
		phrase.Translation += " (" + item.ValueText.Translation+" yen)";
	}

	int GetTaskCount(){
		return 3;
	}

	void GenerateActors() {
		var count = GetNPCCount();
		var targets = SceneAreaUtil.ScatterTargets(taskref.Data.AreaName, count, GetBoss().transform.position);
		for (int i = 0; i < targets.Count; i++){
			var app = AppearanceLibrary.GetRandomAppearance();
			var npc = DialogueActorUtil.GetNewActor(app, "customer" + i);
			npc.tag = TagLibrary.JobInteractable;
			instances.Add(npc);
		}
		instances = GameObjectUtil.RandomAssignInstancesToTargets(targets, instances);

		var prefab = Resources.Load<GameObject>("Tutorial/DownArrow");
		foreach (var go in instances) {
			var indicator = GameObject.Instantiate<GameObject>(prefab);
			indicator.transform.position = go.transform.position + 2.5f * Vector3.up;
			indicator.transform.parent = go.transform;
			indicator.AddComponent<IndicatorComponent>().Initialize("", null, new MapIndicator(MapResourceType.Standard, Color.yellow), false);
			arrows.Add(indicator);
		}
	}

	int GetNPCCount(){
		return GetTaskCount();
	}

	public GameObject GetBoss() {
		return GameObject.Find(CashierBossName);
	}
}
