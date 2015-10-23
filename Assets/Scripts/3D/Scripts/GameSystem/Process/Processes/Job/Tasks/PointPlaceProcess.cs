using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

[JobProcessType]
public class PointPlaceProcess : IProcess<JobTaskRef, JobTaskExitArgs> {

    public ProcessExitCallback OnExit { get; set; }
	public const string thanks = "thank you";

	GameObject target;
	JobTaskRef task;
	PointPlaceTaskData taskData;
	
	int remainingCount = 0;
	int correctCount = 0;
	int totalTrials = 0;
//	float score = 0;

	//learning point is question
	IEnumerable<QATaskGameData.QALine> qa;
	QATaskGameData.QALine currentQA;
	LinkedList<QATaskGameData.QALine> availableQuestions;

	const string tagToUse = "place_to_point";
	const string targetName = "AskerTarget";

	public void ForceExit() {
		Exit();
	}
	
	void Exit() {
		OnExit.Raise(this, null);
	}

	public void Initialize (JobTaskRef param1)
	{
		task = param1;
		taskData = (PointPlaceTaskData) (task.Data);
		Transform actorParent = GameObject.Find(targetName).transform;
		target = DialogueActorUtil.GetNewActor(AppearanceLibrary.GetRandomAppearance(), actorParent);
		target.name = taskData.Actor.Name;
		//find and put environment targets that contains the phrase
		foreach(var streetTarget in taskData.StreetTarget){
			GameObject st = GameObject.Find(streetTarget);
			PropType.GetPropForTarget(PropType.ExclaimationMark, st.transform);
			st.tag = tagToUse;
			var env_phrase = st.GetComponentInChildren<EnvironmentPhrase>();
			env_phrase.phrase = new ScenePhraseSequence();
			env_phrase.phrase.Set(task.Job.GetPhrase(streetTarget));
		}
		//TODO temporary. Disable the region restriction
		var area = GameObject.FindObjectOfType<SceneAreaSettings>();
		if (area != null){
			area.gameObject.SetActive(false);
		}

        SceneAreaManager.Instance.Get(TagLibrary.Area03);
//		GameObject.Find("Targets").transform.Find("Area01").gameObject.SetActive(false);

//		target = new SceneObjectRef(taskData.Actor).GetSceneObject();
		target.GetComponent<FollowPlayer>().enabled = true;
		remainingCount = GetTaskCount ();

		availableQuestions = new LinkedList<QATaskGameData.QALine>();
		var qas = taskData.GetQAs().ToArray();
		for (int i = 0; i < qas.Length; i++){
			if(i > task.Variation + 1 && !task.IsPromotion)
				break;
			availableQuestions.AddLast(qas[i]);
		}
		qa = taskData.GetQAs ();
		InitializeStatus();
		ProcessLibrary.MessageBox.Get("go and point to the correct place", StartTask, this);
	}
	
	void StartTask(object obj, object arg){
		//start by giving a random query
		getNewQuery ();
		StartQuestion();
	}

	void StartQuestion(){
		StartQuestion(null, null);
	}
	void StartQuestion(object obj, object e){
		ProcessLibrary.BeginConversation.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()),HandAskQuestion , this);
	}
	
	void HandAskQuestion(object s, object a){
		ProcessLibrary.ConversationSegment.Get(new ConversationArgs(target, taskData.Dialogue, getNewContext()),HandleEndAskingQuestion , this);
	}

	void HandleEndAskingQuestion(object a, object b){
		ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(target, taskData.Dialogue, false),HandleQuestionExit , this);
	}
	
	void HandleQuestionExit(object sender, object arg){
		ProcessLibrary.Explore.Get(new ExploreInitArgs(null, tagToUse, "point to this place", "not a valid place"), HandleAnswerFeedBack, this);
	}

	void HandleAnswerFeedBack (object sender, ExploreResultArgs e)
	{
		totalTrials++;
		var text = e.Target.GetComponentInChildren<EnvironmentPhrase>().phrase.Get();
		if (PhraseSequence.PhrasesEquivalent(text, currentQA.Answer)) {
			var ui = UILibrary.PositiveFeedback.Get("");
			DataLogger.LogTimestampedData("PointPlace01", 
			                              "get : " + text.GetText(JapaneseTools.JapaneseScriptType.Romaji),
			                              "want : " + currentQA.Answer.GetText(JapaneseTools.JapaneseScriptType.Romaji),
			                              "correct : true");
			correctCount++;
			UpdateStatus(correctCount);
			ui.Complete += HandleFeedBackComplete;
		} 
		else {
			DataLogger.LogTimestampedData("PointPlace01", 
			                              "get : " + text.GetText(JapaneseTools.JapaneseScriptType.Romaji),
			                              "want : " + currentQA.Answer.GetText(JapaneseTools.JapaneseScriptType.Romaji),
			                              "correct : false");
			var ui = UILibrary.NegativeFeedback.Get("");
			ui.Complete += HandleRetry;
		}
	}

	void HandleRetry (object sender, EventArgs<object> e)
	{
		UpdateStatus(correctCount);
//		checkPromotionFailure();
		HandleQuestionExit(null, null);
	}

	void HandleFeedBackComplete (object sender, EventArgs<object> e)
	{
		ProcessLibrary.BlackOut.GetNested(ProcessLibrary.BeginConversation, new ConversationArgs(target, null), AfterConversation, this);

//		HandleConversationExited(null, null);
	}

	void AfterConversation(object obj, object e){
		PhraseSequence afterChat = taskData.GetAfterChat(currentQA.Answer);
		var d = new DialogueSequence(taskData.Actor.Name, afterChat);
		ProcessLibrary.ConversationSegment.Get(ConversationArgs.OpenSegmentArgs(target, d, null), QuitAfterChat, this);
	}

	void QuitAfterChat(object obj, object e){
		PhraseSequence afterChat = taskData.GetAfterChat(currentQA.Answer);
		var d = new DialogueSequence(taskData.Actor.Name, afterChat);
		ProcessLibrary.EndConversation.Get(ConversationArgs.ExitArgs(target, d), HandleConversationExited, this);
	}

	void HandleConversationExited (object obj, object e){
		remainingCount--;

		if (remainingCount <= 0)
			PlayExitDialogue ();
		else
			StartTask (null, null);
	}

	void PlayExitDialogue ()
	{
//		var s = "";
//		score = (float)correctCount / totalTrials;
//		if (score >= 0.99f) {
//			s = "Thank you very much! You are so helpful.";
//		} else if( score >= 0.75f){
//			s = "Hm... thanks anyway.";
//		} else if(score >= 0.25f){
//			s = "You don't seem to know many places.";
//		} else {
//			s = "Don't just point to me random places!";
//		}
		var p = task.Job.GetPhrase(thanks);
		var d = new DialogueSequence(taskData.Actor.Name, p);
		ProcessLibrary.Conversation.Get(new ConversationArgs(target, d, null, false, true), HandleExitConversationExit, this);
	}


	void HandleExitConversationExit(object sender, object obj) {
//		PlayerDataConnector.AddRepetitionToJob(task.Job, task);
		Exit();
	}

	void getNewQuery ()
	{
		if(availableQuestions.Count == 0){
			if(task.IsPromotion){
				currentQA = qa.ToArray()[UnityEngine.Random.Range (0, qa.Count ())];
			}else{
				currentQA = qa.ToArray()[UnityEngine.Random.Range (0, Math.Min(qa.Count (), task.Variation + 2))];
			}
		}
		else{
			currentQA = availableQuestions.ElementAt(UnityEngine.Random.Range (0, availableQuestions.Count));
			availableQuestions.Remove(currentQA);
		}
	}

	ContextData getNewContext ()
	{
		ContextData c = new ContextData ();
		c.Set("place", currentQA.Question);
		return c;
	}

	int GetTaskCount ()
	{
		//min 3, max 8
		return task.IsPromotion ? 6 : Math.Min(3 + task.Variation, 6);
	}

	void InitializeStatus(){
		TaskState.Instance.SetState("Point Places", string.Format("{0}/{1}", 0, GetTaskCount()));
	}
	void UpdateStatus(int count) {
		PlayerDataConnector.AddRepetitionToJob(task.Job, task);		
		TaskState.Instance.SetState("Point Places", string.Format("{0}/{1}", count, GetTaskCount()));
	}


	void checkPromotionFailure(){
		if(task.IsPromotionFailed){
			Exit ();
		}
	}
}
