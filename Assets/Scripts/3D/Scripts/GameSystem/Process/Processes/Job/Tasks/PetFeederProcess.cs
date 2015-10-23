using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class PetFeederProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

	public const string goodJob = "Nice job!";
	public const string OkJob = "That was passable.";
	public const string badJob = "You need to work harder.";
	public const string terribleJob = "What are you doing!? Please do your work well.";

    public ProcessExitCallback OnExit { get; set; }

	GameObject target;
	JobTaskRef task;
	PetFeederTaskData taskData;
	
	int remainingCount = 0;
	int correctCount = 0;
	int totalTrials = 0;
	float score = 0;

	IEnumerable<QATaskGameData.QALine> qa;
	QATaskGameData.QALine currentQA;

	LinkedList<QATaskGameData.QALine> availableQuestions;

	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		OnExit.Raise(this, null);
	}

	public void Initialize (JobTaskRef param1)
	{
		task = param1;
		taskData = (PetFeederTaskData)(param1.Data);

		target = new SceneObjectRef(taskData.Actor).GetSceneObject();
		remainingCount = GetTaskCount ();

		availableQuestions = new LinkedList<QATaskGameData.QALine>();
		var qas = taskData.GetQAs().ToArray();
		for (int i = 0; i < qas.Length; i++){
			if(i > task.Variation + 1)
				break;
			availableQuestions.AddLast(qas[i]);
		}

		qa = taskData.GetQAs ();

		//test spawning gameobjects during the game
		var parents = new string[] {"target01", "target02", "target03", "target04"};
//		var prefabs = new PropType[] {PropType.BlueArea, PropType.RedArea, PropType.BrownArea, PropType.YellowArea};

		var prefabs = 	from index in taskData.Props
						select ResourceType.GetValues<PropType>().ToArray()[index];
		var prefabList = prefabs.Select(
			prefab => Resources.Load<GameObject>(prefab.ResourcePath)
			);
		GameObjectUtil.RandomAssignPrefabToTarget(parents, prefabList);
		//give fix phrase instead?
		for (int i = 0; i < parents.Length; i++){
			GameObject parentObj = GameObject.Find (parents[i]);
			var env = parentObj.GetComponentInChildren<EnvironmentPhrase>();
			var phrase = taskData.GetQAs().ElementAt(i).Answer;
			env.phrase.Set(phrase);
		}
		Equip();
	}

	void Equip ()
	{
		ProcessLibrary.BlackOut.GetNested(
			ProcessLibrary.EquipItem, new EquipmentArgs("KittenItem", PlayerManager.Instance.PlayerGameObject), startTask, this);
	}

	void startTask(object sender, object obj){
		//start by giving a random query
		getNewQuery ();
		//TODO testing tutorial

		ProcessLibrary.BeginConversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext(), true), HandleEndQuestion, this);
	}

	void HandleEndQuestion(object sender, object arg){
		ProcessLibrary.EndConversation.Get(new ConversationArgs(target, taskData.Dialogue), HandleQuestionExit, this);
	}
	
	void HandleQuestionExit(object sender, object arg){
		ProcessLibrary.Explore.Get(new ExploreInitArgs("KittenItem", "give the cat this", "nothing for the cat"), HandleAnswerFeedBack, this);
	}

	void HandleAnswerFeedBack (object sender, ExploreResultArgs arg)
	{
		totalTrials++;
		if (arg.Target == null) {
			HandleQuestionExit(null, null);
			Debug.Log("No object returned.");
		} else {
			if (arg.Target.GetComponentInChildren<EnvironmentPhrase>().phrase.Get().GetText() == currentQA.Answer.GetText()) {
				correctCount++;
				var ui = UILibrary.PositiveFeedback.Get("");
				ui.Complete += HandleFeedBackComplete;
			} else {
				var ui = UILibrary.NegativeFeedback.Get("");
				ui.Complete += HandleFeedBackComplete;
			}
		}
	}

	void HandleRetry (object sender, EventArgs<object> e)
	{
		ProcessLibrary.Conversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()), HandleQuestionExit, this);
	}

	void HandleFeedBackComplete (object sender, EventArgs<object> e)
	{
		remainingCount--;
		if (remainingCount <= 0)
			PlayExitDialogue ();
		else
			startTask (null, null);
	}

	void PlayExitDialogue ()
	{
		 PhraseSequence p = null;
		score = (float)correctCount / totalTrials;
        if (score >= 0.99f) {
            p = task.Job.GetPhrase(goodJob);
        } else if( score >= 0.75f){
            p = task.Job.GetPhrase(OkJob);
        } else if(score >= 0.25f){
            p = task.Job.GetPhrase(badJob);
        } else {
            p = task.Job.GetPhrase(terribleJob);
        }
		
		var d = new DialogueSequence();
		var de = d.GetNewDialogueElement<LineDialogueElement>();
		de.Line = new DialogueActorLine();
		de.Line.Phrase = p;
		d.Actors.Add(taskData.Actor);
		ProcessLibrary.Conversation.Get(new ConversationArgs(target, d), HandleExitConversationExit, this);
	}


	void HandleExitConversationExit(object sender, object obj) {
		int money = (int)(10000 * score);
		PlayerDataConnector.AddMoney(money);
		string moneyString = string.Format("You made {0} yen today.", money);
		var ui = UILibrary.MessageBox.Get(moneyString);
		ui.Complete += ui_Complete;
	}
	
	void ui_Complete(object sender, EventArgs<object> e) {
		Exit();
	}

	void getNewQuery ()
	{
		if(availableQuestions.Count == 0)
			currentQA = qa.ToArray()[UnityEngine.Random.Range (0, Math.Min(qa.Count (), task.Variation + 2))];
		else{
			currentQA = availableQuestions.ElementAt(UnityEngine.Random.Range (0, availableQuestions.Count));
			availableQuestions.Remove(currentQA);
		}
	}

	ContextData getNewContext ()
	{
		ContextData c = new ContextData ();
		Debug.Log(currentQA.Question.GetText());
		Debug.Log(currentQA.Answer.GetText());
		c.Set("need", new PhraseSequence(currentQA.Question));
		return c;
	}
	//
	int GetTaskCount ()
	{
		return 3;
	}
}
