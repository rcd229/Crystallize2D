using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

public class ProcessInitializer {

    public static bool Running { get; set; }

    public static void Initialize() {
        LinkProcesses();
        LinkUI();
        if (GameSettings.Instance.UseTutorials) {
            LinkTutorials();
        }
        LinkEvents();

        InstantiateStaticGameObjects();
    }

    static void LinkProcesses() {
        // Highest level
        GameTimeProcess.MorningFactory.Set<MorningProcess>();
        GameTimeProcess.DayFactory.Set<DayProcess>();
        GameTimeProcess.EveningFactory.Set<EveningProcess>();
        GameTimeProcess.NightFactory.Set<NightProcess>();

        // Use the same transition process for all session types
        TimeSessionProcess.TransitionFactory.Set<SessionTransitionProcess>();
        //TimeSessionProcess.TransitionFactory.SetUI<SessionTransitionUI>(SessionTransitionUI.GetInstance);

        // Link main events for morning session
        MorningProcess.RequestPlanSelection.Set<JobSelectionProcess>();

        // Link main events for day session
        DayProcess.RequestJob.Set<JobProcessSelector>();

        // Link main events for evening session
        EveningProcess.RequestExplore.Set<TempProcess<object, TimeSessionArgs>>();

        // Conversation sub-processes
        ConversationSequence.RequestConversationCamera.Set<ConversationCameraProcess>();
        ConversationSequence.RequestPromptDialogueTurn.Set<BranchDialogueElementProcess>();
        ConversationSequence.RequestAnimationDialogueTurn.Set<AnimationDialogueElementProcess>();
        ConversationSequence.RequestMessageDialogueTurn.Set<MessageDialogueElementProcess>();
        //PromptDialogueTurnSequence.RequestPhrasePanel.Set<PhraseSelectionProcess>();
        //PhraseSelectionProcess.RequestPhraseEditor.Set<EditPhraseProcess>();

        // Library of misc reusable processes
        // Link main events for night session
        ProcessLibrary.Review.Set<ReviewProcess>();

        ProcessLibrary.MessageBox.Set<MessageBoxProcess>();
        ProcessLibrary.BlackOut.Set<BlackOutAndDoProcess>();

        ProcessLibrary.ChangeScene.Set<ChangeSceneProcess>();

        ProcessLibrary.DialogueLine.Set<LineDialogueElementProcess>();

        ProcessLibrary.Conversation.Set<ConversationSequence>();
        ProcessLibrary.BeginConversation.Set<BeginConversationProcess>();
        ProcessLibrary.ConversationSegment.Set<ConversationSegmentProcess>();
        ProcessLibrary.EndConversation.Set<EndConversationProcess>();
        ProcessLibrary.ListenForInput.Set<InputListenerProcess>();
        ProcessLibrary.Explore.Set<ExploreProcess>();

        ProcessLibrary.PlayerConversation.Set<PlayerConversationProcess>();
        ProcessLibrary.YesNoConversation.Set<PlayerYesNoProcess>();
        ProcessLibrary.TalkToActors.Set<TalkToActorsProcess>();
        ProcessLibrary.YesNo.Set<YesNoConversationProcess>();

        ProcessLibrary.EquipItem.Set((e) => e.Set());

        //Free explore processes
        ProcessLibrary.QuestConversation.Set<QuestConversationProcess>();
        ProcessLibrary.SceneJob.Set<SceneJobProcess>();
        ProcessLibrary.SceneShop.Set<SceneShopProcess>();
        ProcessLibrary.Job.Set<JobProcess>();
        ProcessLibrary.PhraseSelectionProcess.Set<PhraseSelectionSubProcess>();
    }

    static void LinkUI() {
        UILibrary.MessageBox.Set(MessageBoxUI.GetInstance);
        UILibrary.SessionTransition.Set(SessionTransitionUI.GetInstance);
        UILibrary.SessionTimeText.Set<SessionTimeTextUI>();
        UILibrary.ActivityText.Set<ActivityTextUI>();
        UILibrary.PromotionFailedText.Set<PromotionFailedTextUI>();
        UILibrary.EarnMoney.Set<EarnMoneyUI>();
        UILibrary.ReviewSummary.Set<ReviewSummaryUI>();
        UILibrary.ReviewResult.Set<ReviewSummaryUI>();

        UILibrary.Equipment.Set<EquipmentUI>();

        UILibrary.PhraseSelector.Set<PhraseSelectionPanelUI>();
        UILibrary.PhraseEditor.Set(ReplaceWordPhraseEditorUI.GetInstance);
        UILibrary.PhraseConstructor.Set<PhraseConstructorUI>();
        UILibrary.WordSelector.Set(WordSelectionPanelUI.GetInstance);
        //UILibrary.ConversationCamera.Set(ConversationCameraController.GetInstance);
        UILibrary.ContextActionButton.Set(ContextActionButtonUI.GetInstance);
        UILibrary.PositiveFeedback.Set(UILibrary.GetPositiveFeedbackInstance);
        UILibrary.NegativeFeedback.Set(UILibrary.GetNegativeFeedbackInstance);
        UILibrary.Message.Set(UILibrary.GetMessageInstance);
        UILibrary.BlackScreen.Set(BlackScreenUI.GetInstance);

        UILibrary.MoneyState.Set(MoneyPanelUI.GetInstance);
        UILibrary.HomeSelectionPanel.Set(HomeSelectionPanelUI.GetInstance);
        UILibrary.ShopPanel.Set<ShopUI>();
        UILibrary.HomeShopPanel.Set(HomeShopUI.GetInstance);
        UILibrary.SkipSessionButton.Set(SkipSessionButtonUI.GetInstance);
        UILibrary.TextMenu.Set(TextSelectionMenuUI.GetInstance);
        UILibrary.ValuedMenu.Set(ValuedSelectionMenuUI.GetInstance);
        UILibrary.ImageTextMenu.Set(TextImageMenuUI.GetInstance);
        UILibrary.NumberEntry.Set(NumberEntryUI.GetInstance);
        UILibrary.PhraseSequenceMenu.Set<PhraseSequenceMenuUI>();
        UILibrary.Jobs.Set(JobPanelUI.GetInstance);
        UILibrary.Tasks.Set(JobTaskPanelUI.GetInstance);
        UILibrary.LeaderBoard.Set(LeaderBoardUI.GetInstance);
        UILibrary.KanaTable.Set<KanaTableUI>();
        UILibrary.DialogueConfidence.Set<DialogueConfidenceUI>();
        UILibrary.NeededWords.Set<NeededWordsUI>();
        UILibrary.Dictionary.Set<FullDictionaryUI>();

        UILibrary.Review.Set<MultipleChoicePhraseReviewUI>();
        UILibrary.KanaReview.Set<MultipleChoiceKanaReviewUI>();

        UILibrary.ContextActionStatus.Set<ContextActionStatusPanelUI>();
        UILibrary.HighlightBox.Set<DragBoxTutorialUI>();
        UILibrary.MovementTutorial.Set<WASDPanelUI>();
        UILibrary.ClickToContinue.Set<ClickToContinueUI>();
        UILibrary.Emoticon.Set<EmoticonUI>();
        UILibrary.SpeechBubble.Set(ChatSpeechBubbleUI.GetInstance);

        UILibrary.PlayerController.Set(ProcessPlayerController.GetInstance);
        UILibrary.ContextActionController.Set(ContextActionController.GetInstance);
        UILibrary.Clock.Set<ClockUI>();
        UILibrary.TaskStatus.Set<TaskStatusUI>();
        UILibrary.MiniMap.Set<Map>();
        UILibrary.MainMenu.Set<MainMenuUI>();
        UILibrary.Settings.Set<SettingsMenu>();
        UILibrary.Help.Set<HelpUI>();
        UILibrary.QuestHUD.Set<QuestHUD>();

//		UILibrary.StartMenu.Set<StartMenuUI> ();
//		UILibrary.SignupMenu.Set<SignupMenuUI> ();

        UILibrary.ChatBox.Set<ChatBoxUI>();

        UILibrary.DebugMenu.Set<DebugMenu>();

        UILibrary.SurveyPrompt.Set<SurveyUI>();

        //ProcessLibrary.MovementTutorial.Set<EmptyProcess<object, object>>();
        ProcessLibrary.ReviewTutorial.Set<EmptyProcess<string, object>>();
        ProcessLibrary.ClockTutorial.Set<EmptyProcess<string, object>>();
        ProcessLibrary.PlaceTutorial.Set<EmptyProcess<object, object>>();
    }

    static void LinkTutorials() {
        //ProcessLibrary.MovementTutorial.AddTutorial<MovementTutorialProcess>("Movement");
        //ProcessLibrary.Explore.AddTutorial<OverhearTutorialProcess>(TagLibrary.Listen);
        ProcessLibrary.Explore.AddTutorial<ExploreTutorialProcess>(TagLibrary.Explore);

        ProcessLibrary.ReviewTutorial.AddTutorial<MessageBoxProcess>("Review");
        ProcessLibrary.ClockTutorial.AddTutorial<TimeTutorialProcess>("Clock");
        ProcessLibrary.PlaceTutorial.AddTutorial<PlaceTutorialProcess>("Place");

        var jobSelectionEvents = new DayEventProcessSelector<object, DaySessionArgs>();
        var p = new ProcessFactoryRef<object, DaySessionArgs>();
        p.Set<JobSelectionTutorialProcess>();
        jobSelectionEvents.AddEvent(0, true, p.Getter(null));
        MorningProcess.RequestPlanSelection.AddSelector(jobSelectionEvents);

        ProcessLibrary.DialogueLine.AddSelector(new DialogueTutorialProcessSelector());
        //ProcessLibrary.DialogueLine.AddTutorial<DragWordsTutorialProcess>(TagLibrary.DragWords);
    }

    static void LinkEvents() {
        var morningEvents = new DayEventProcessSelector<MorningSessionArgs, DaySessionArgs>();
        morningEvents.AddEvent(0, false, ProcessLibrary.MessageBox.Getter("You have decided to start a new life in Japan. You spent the last of your money on your plane ticket and work visa. Better get to work!"));
        GameTimeProcess.MorningFactory.AddSelector(morningEvents);
    }

    static void InstantiateStaticGameObjects() {
        GameObjectUtil.GetResourceInstance("DebugListener");
        GameObjectUtil.GetResourceInstance("Audio");
    }

    public static void InstantiateNewSceneObjects() {
        CrystallizeEventManager.GetInstance();
        UISystem.GetInstance();
        SpeechPanelUI.GetInstance();
        BlackScreenUI.GetInstance().SetAlpha(1f);

        UILibrary.GetStatusUI();
    }

}
