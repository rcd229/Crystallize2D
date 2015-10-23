using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

enum TutorialCallbacks {
    AfterBuy
}

[JobProcessType]
public class CashierProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

	public const string NiceJob = "Nice job!";
	public const string OkayJob = "That was passable.";
	public const string WorkHarder = "You need to work harder.";
	public const string BadJob = "What are you doing!? Please do your work well.";

	GameObject person;
	JobTaskRef task;
	CashierTaskData taskData;

	int remainingCount = 0;
	int correctCount_Greeting = 0;
	int correctCount_Price = 0;
	float score = 0;
	int priceTrials;
	
	ITemporaryUI<TimeSpan,object> clockUI;
	TimeSpan time;
	PhraseSequence targetPhrase;

	int numItems;
	int price;
	PhraseSequence greeting;
	ValuedItem[] nowItem;

	List<PhraseSequence> greetings;
	List<ValuedItem> prices;

	public void Initialize(JobTaskRef param1) {
		greetings = new List<PhraseSequence>();
		prices = new List<ValuedItem> ();

		task = param1;
		taskData = (CashierTaskData) (task.Data);
		remainingCount = GetTaskCount();
		numItems = task.IsPromotion ? 3 :  1 + ProgressUtil.GetAmountForTiers(task.Variation, 2);
		priceTrials = GetPriceTrials();
		nowItem = new ValuedItem[numItems];
		//test instantiating gameobject on the fly
		new TargetLoadUtil(taskData.Actor.Name, AppearanceLibrary.GetRandomAppearance()).Instantiate();
		//end of test

        //var a = DialogueActorUtil.GetNewActor(AppearanceLibrary.GetRandomAppearance());
        //a.name = taskData.Actor.Name;
		person = new SceneObjectRef(taskData.Actor).GetSceneObject();


		//get menu item lists
		var cashierPhrases = 
			from line in taskData.Lines
			where line.Tag == "all" || line.Tag == "cashier"
			select line;
		foreach (var v in cashierPhrases) {
			var item = v.Phrase;
			greetings.Add(item);
		}

		InitializeStatus();
//		greetings.Add(new PhraseSequence("?"));
		prices = taskData.ShopLists;
		TaskState.Instance.OtherStateChanged += TaskStateChangeCallBack;
		clockUI = UILibrary.Clock.Get(new TimeSpan(0));

        CoroutineManager.Instance.WaitAndDo(GetClock);
	}

    void GetClock() {
        clockUI.Initialize(new TimeSpan());
        if (!PlayerData.Instance.Tutorial.GetTutorialViewed(TagLibrary.Clock)) {
            ProcessLibrary.ClockTutorial.Get("", ClockTutorialCallback, this);
            PlayerData.Instance.Tutorial.SetTutorialViewed(TagLibrary.Clock);
        } else {
            ClockTutorialCallback(null, null);
        }
    }

    void ClockTutorialCallback(object sender, object args) {
        ProcessLibrary.MessageBox.Get("Select the correct greeting.", HandleGreetingExit, this);
    }

	//Select Greeting
	void HandleGreetingExit(object sender, object obj) {
//		GetNewGreeting();
		RandomizeTime();
		var dialogue = taskData.Dialogues[0];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(actor, dialogue), Start_Greeting, this);


	}

	void Start_Greeting(object o, object obj){
		UILibrary.PhraseSelector.Get (new PhraseSelectorInitArgs(greetings, check : false), ui_GreetComplete, this);
	}



	void ui_GreetComplete(object sender, EventArgs<PhraseSequence> e) {
		if(PhraseSequence.PhrasesEquivalent(e.Data, taskData.GetRelevantPhrase("Can I help you?"))){
			DataLogger.LogTimestampedData("Cashier01", "greet", e.Data.GetText(JapaneseTools.JapaneseScriptType.Romaji), "correct");
			correctCount_Greeting++;
			UILibrary.PositiveFeedback.Get("", Greet_Feedback_Complete, this);
		}
		else if (PhraseSequence.PhrasesEquivalent(e.Data, targetPhrase)) {
			DataLogger.LogTimestampedData("Cashier01", "greet", e.Data.GetText(JapaneseTools.JapaneseScriptType.Romaji), "correct");
			correctCount_Greeting++;
			UILibrary.PositiveFeedback.Get("", Greet_Feedback_Complete, this);
		} else {
			DataLogger.LogTimestampedData("Cashier01", "greet", e.Data.GetText(JapaneseTools.JapaneseScriptType.Romaji), "wrong");
			UILibrary.NegativeFeedback.Get("", Greet_Feedback_Complete, this);
		}
	}

	void Greet_Feedback_Complete(object sender, EventArgs<object> e) {
		UpdateStatus(correctCount_Greeting, "Greetings");
		UpdateStatus(correctCount_Price, "Answer Price");
		ContextData context = GetNewGreetingContext ();
		DialogueSequence d = taskData.Dialogues[0];
		var actor = new SceneObjectRef(d.Actors[0]).GetSceneObject();
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(actor, d, context, true), HandleGreetingConversationExit, this);

	}

	void HandleGreetingConversationExit(object sender, object obj){
//		UpdateStatus(correctCount_Greeting, "Greetings");
		if(task.IsPromotionFailed){
			CheckFailPromotion();
		}
		else{
			ProcessLibrary.MessageBox.Get("Select the correct price.", HandleMessageBoxExit, this);
		}
	}

	void HandleMessageBoxExit(object sender, object obj) {
		GetNewTargetPrice();
		var dialogue = taskData.Dialogues[1];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(actor, dialogue, GetNewPriceContext()), HandlePriceConversationExit, this);
	}


	void HandlePriceConversationExit(object sender, object obj){
		UILibrary.NumberEntry.Get(null, ui_Complete, this);
	}

	void ui_Complete(object sender, EventArgs<int> e) {
		priceTrials--;
		var dialogue = taskData.Dialogues[1];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		if (e.Data == price) {
			remainingCount--;
			correctCount_Price++;
			DataLogger.LogTimestampedData("Cashier01", "price", e.Data.ToString(), "correct");
			EventHandler<EventArgs<object>> callback = (o, arg) => {
				ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), Feedback_Complete, this);
			};
			UILibrary.PositiveFeedback.Get("", callback, this);
		} else if(priceTrials > 0){
			ProcessExitCallback<object> callback = (o, arg) => ProcessLibrary.MessageBox.Get("Wrong Answer. " + priceTrials + " more chance.", RepeatPriceTrial, this);
			ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), callback, this);

		}
		else {
			DataLogger.LogTimestampedData("Cashier01", "price", e.Data.ToString(), "wrong");
			remainingCount--;

			EventHandler<EventArgs<object>> callback = (o, arg) => 
				ProcessLibrary.EndConversation.Get(new ConversationArgs(actor, dialogue), GiveCorrectAnswer, this);
			UILibrary.NegativeFeedback.Get("", callback, this);
		}
	}
	
	void Feedback_Complete(object sender, object e) {
		UpdateStatus(correctCount_Greeting, "Greetings");
		UpdateStatus(correctCount_Price, "Answer Price");
		PlayerDataConnector.AddRepetitionToJob(task.Job, task);
		if(task.IsPromotionFailed){
			CheckFailPromotion();
			return;
		}
		if (remainingCount <= 0) {
			PlayExitDialogue();
		} else {
			priceTrials = GetPriceTrials();
			ProcessLibrary.MessageBox.Get("Select the correct greeting.", HandleGreetingExit, this);
		}
	}

	void RepeatPriceTrial(object sender, object e){
		var dialogue = taskData.Dialogues[1];
		var actor = new SceneObjectRef(dialogue.Actors[0]).GetSceneObject();
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(actor, dialogue, GetNewPriceContext(), true), HandlePriceConversationExit, this);
	}
	
	void GiveCorrectAnswer(object sender, object arg){
		ProcessLibrary.MessageBox.Get("The correct amount is: " + price, Feedback_Complete, this);
	}
	
	void PlayExitDialogue() {
		PhraseSequence p;
		score = (float)(correctCount_Greeting + correctCount_Price) / (GetTaskCount() * 2);
		if (score >= 0.99f) {
			p = task.Job.GetPhrase(NiceJob);
		} else if( score >= 0.75f){
			p = task.Job.GetPhrase(OkayJob);
		} else if(score >= 0.25f){
			p = task.Job.GetPhrase(WorkHarder);
		} else {
			p = task.Job.GetPhrase(BadJob);
		}
		
		var d = new DialogueSequence(taskData.Actor.Name, p);
		ProcessLibrary.Conversation.Get(new ConversationArgs(person, d), HandleExitConversationExit, this);
	}
	
	void HandleExitConversationExit(object sender, object obj) {
		Exit();
	}

	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		TaskState.Instance.OtherStateChanged -= TaskStateChangeCallBack;
		OnExit.Raise(this, null);
	}
	
	void GetNewTargetPrice(){
		nowItem = new ValuedItem[numItems];
		var priceArray = prices.ToArray ();
		int wordRange = Math.Min(priceArray.Length, 1 + ProgressUtil.GetAmountForTiers(task.Variation, 0, 0, 1, 1, 2, 2));
		if(task.IsPromotion){
			wordRange = priceArray.Length;
		}
		for (int i = 0; i < numItems; i++) {
			nowItem[i] = priceArray[UnityEngine.Random.Range(0, wordRange)];
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
		//TODO
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

	ContextData GetNewGreetingContext() {
		var c = new ContextData();
//		c.UpdateElement("greeting", greeting);
		c.Set("greeting", targetPhrase);
		return c;
	}
	int GetTaskCount(){
		return task.IsPromotion ? 5 : 3;
	}

	int GetPriceTrials(){
		return task.IsPromotion ? 0 : Math.Max(0, 2 - task.Variation + numItems);
	}

	void RandomizeTime() {
		var hours = UnityEngine.Random.Range(0, 24);
		if (hours > 18) {
			hours = UnityEngine.Random.Range(0, 24);
		}
		time = new TimeSpan(hours, UnityEngine.Random.Range(0, 60), 0);
		clockUI.Initialize(time);
		
		targetPhrase = GetPhraseForTime(time);
	}

	PhraseSequence GetPhraseForTime(TimeSpan time) {
		if (time.Hours < 11) {
			return taskData.GetRelevantPhrase("Good Morning");
		} else if (time.Hours < 18) {
			return taskData.GetRelevantPhrase("Hello");
		} else {
			return taskData.GetRelevantPhrase("Good Evening");
		}
	}

	void InitializeStatus(){
		TaskState.Instance.SetState("Greetings", string.Format("{0}/{1}", 0, GetTaskCount()));
		TaskState.Instance.SetState("Answer Price", string.Format("{0}/{1}", 0, GetTaskCount()));
	}

	void UpdateStatus(int count, string label) {
//		PlayerDataConnector.AddRepetitionToJob(task.Job, task);
		TaskState.Instance.SetStateWithoutRaise(label, string.Format("{0}/{1}", count, GetTaskCount()));
	}

	void TaskStateChangeCallBack(object o, object e){
		UpdateStatus(correctCount_Greeting, "Greetings");
		UpdateStatus(correctCount_Price, "Answer Price");
	}

	void CheckFailPromotion(){
		if(task.IsPromotionFailed){
			Exit ();
		}
	}
}
