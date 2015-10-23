using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class VolunteerProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public ProcessExitCallback OnExit { get; set; }

	GameObject target;
	GameObject player;
	JobTaskRef task;
	VolunteerTaskData taskData;
	
	int remainingCount = 0;
	int correctCount = 0;
	int totalTrials = 0;
	float score = 0;

	IEnumerable<QATaskGameData.QALine> qa;
	QATaskGameData.QALine currentQA;
	List<PhraseSequence> menuOptions;
	PhraseSequence currentAnswer;
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
		taskData = (VolunteerTaskData)(param1.Data);

		target = new SceneObjectRef(taskData.Dialogue.GetActor(0)).GetSceneObject();
		player = new SceneObjectRef(taskData.AnswerDialogue.GetActor(0)).GetSceneObject();
		remainingCount = GetTaskCount ();

		availableQuestions = new LinkedList<QATaskGameData.QALine>();
		var qas = taskData.GetQAs().ToArray();
		for (int i = 0; i < qas.Length; i++){
			if(i > task.Variation + 1)
				break;
			availableQuestions.AddLast(qas[i]);
		}
		qa = taskData.GetQAs ();
		menuOptions = new List<PhraseSequence> ();
		foreach (var line in qa) {
			menuOptions.Add(line.Answer);
		}
		ProcessLibrary.MessageBox.Get("Point people to places according to their need", StartTask, this);
	}

	void StartTask(object obj, object arg){
		//start by giving a random query
		getNewQuery ();
		StartQuestion();
	}

	void StartQuestion(){
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()),HandAskQuestion , this);
	}

	void HandAskQuestion(object s, object a){
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()),HandleQuestionExit , this);
	}
	
	void HandleQuestionExit(object sender, object arg){
		//provides a menu to select possible responses
		var ui = UILibrary.PhraseSelector.Get(new PhraseSelectorInitArgs(menuOptions));
//		var ui = UILibrary.PhraseSequenceMenu.Get (menuOptions);
		ui.Complete += TalkBackToAsker;
	}

	void TalkBackToAsker(object sender, EventArgs<PhraseSequence> e){
		currentAnswer = e.Data;
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(player, taskData.AnswerDialogue, getAnswerContext(currentAnswer)), HandleAnswerFeedBack, this);
	}

	void HandleAnswerFeedBack (object sender, object e)
	{
		totalTrials++;
		if (currentAnswer.GetText() == currentQA.Answer.GetText()) {
			var ui = UILibrary.PositiveFeedback.Get("");
			correctCount++;
			ui.Complete += HandleFeedBackComplete;
		} 
		else {
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += HandleFeedBackComplete;
		}
	}

	void HandleFeedBackComplete (object sender, EventArgs<object> e){
		ProcessLibrary.EndConversation.Get(new ConversationArgs(player, taskData.AnswerDialogue), HandleFeedBackExit, this);
	}

	void HandleFeedBackExit (object sender, object e)
	{
		remainingCount--;
		if (remainingCount <= 0)
			PlayExitDialogue ();
		else
			StartTask (null, null);
	}

	void PlayExitDialogue ()
	{
		var s = "";
		score = (float)correctCount / totalTrials;
		if (score >= 0.99f) {
			s = "You did a good job.";
		} else if( score >= 0.75f){
			s = "You can improve yourself on this.";
		} else if(score >= 0.25f){
			s = "You did not do well.";
		} else {
			s = "You did terribly. Do better next time";
		}
		ProcessLibrary.MessageBox.Get(s,HandleExitConversationExit,this);
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
		c.Set("need", new PhraseSequence(currentQA.Question));
		return c;
	}

	ContextData getAnswerContext(PhraseSequence s)
	{
		ContextData c = new ContextData ();
		c.Set("place", new PhraseSequence(s));
		return c;
	}

	int GetTaskCount ()//
	{
		return 3;
	}
}
