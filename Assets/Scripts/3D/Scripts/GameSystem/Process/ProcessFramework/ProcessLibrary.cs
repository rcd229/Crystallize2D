using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ProcessLibrary {

    public static readonly ProcessFactoryRef<string, object> MessageBox = new ProcessFactoryRef<string, object>();
    public static readonly ProcessFactoryRef<IProcessGetter, object> BlackOut = new ProcessFactoryRef<IProcessGetter, object>();

    public static readonly ProcessFactoryRef<SceneData, object> ChangeScene = new ProcessFactoryRef<SceneData, object>();

    public static readonly ProcessFactoryRef<ConversationArgs, object> Conversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<DialogueState, DialogueState> DialogueLine = new ProcessFactoryRef<DialogueState, DialogueState>();
    public static readonly ProcessFactoryRef<ConversationArgs, object> BeginConversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<ConversationArgs, DialogueState> ConversationSegment = new ProcessFactoryRef<ConversationArgs, DialogueState>();
    public static readonly ProcessFactoryRef<ConversationArgs, object> EndConversation = new ProcessFactoryRef<ConversationArgs, object>();
    public static readonly ProcessFactoryRef<YesNoArgs, bool> YesNo = new ProcessFactoryRef<YesNoArgs, bool>();
    public static readonly ProcessFactoryRef<InputListenerArgs, InputListenerArgs> ListenForInput = new ProcessFactoryRef<InputListenerArgs, InputListenerArgs>();
    public static readonly ProcessFactoryRef<ExploreInitArgs, ExploreResultArgs> Explore = new ProcessFactoryRef<ExploreInitArgs, ExploreResultArgs>();
    public static readonly ProcessFactoryRef<DaySessionArgs, object> Job = new ProcessFactoryRef<DaySessionArgs, object>();

    public static readonly ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs> PlayerConversation = new ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs>();
    public static readonly ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs> YesNoConversation = new ProcessFactoryRef<PlayerConversationInitArgs, PlayerConversationExitArgs>();
    public static readonly ProcessFactoryRef<TalkToActorsInitArgs, TalkToActorExitArgs> TalkToActors = new ProcessFactoryRef<TalkToActorsInitArgs, TalkToActorExitArgs>();

    public static readonly ProcessFactoryRef<EquipmentArgs, object> EquipItem = new ProcessFactoryRef<EquipmentArgs, object>();

    //public static readonly ProcessFactoryRef<object, object> MovementTutorial = new ProcessFactoryRef<object, object>();
    public static readonly ProcessFactoryRef<string, object> ReviewTutorial = new ProcessFactoryRef<string, object>();
    public static readonly ProcessFactoryRef<object, object> PlaceTutorial = new ProcessFactoryRef<object, object>();
    public static readonly ProcessFactoryRef<string, object> ClockTutorial = new ProcessFactoryRef<string, object>();

    public static readonly ProcessFactoryRef<object, object> Review = new ProcessFactoryRef<object, object>();

	//Free exploration processes
	public static readonly ProcessFactoryRef<QuestArgs, object> QuestConversation = new ProcessFactoryRef<QuestArgs, object>();
    public static readonly ProcessFactoryRef<IJobRef, object> SceneJob = new ProcessFactoryRef<IJobRef, object>();
    public static readonly ProcessFactoryRef<Shop, object> SceneShop = new ProcessFactoryRef<Shop, object>();
    public static readonly ProcessFactoryRef<PhraseSelectorInitArgs, PhraseSequence> PhraseSelectionProcess = new ProcessFactoryRef<PhraseSelectorInitArgs, PhraseSequence>();

}
