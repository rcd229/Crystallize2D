using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UILibrary {

    // Messages
    public static readonly UIFactoryRef<string, object> MessageBox = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> SessionTransition = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> SessionTimeText = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> ActivityText = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> PromotionFailedText = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<EarnedMoneyArgs, object> EarnMoney = new UIFactoryRef<EarnedMoneyArgs, object>();
    public static readonly UIFactoryRef<string, object> PositiveFeedback = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> NegativeFeedback = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> Message = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<object, object> BlackScreen = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs> ReviewResult = new UIFactoryRef<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs>();
    public static readonly UIFactoryRef<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs> ReviewSummary = new UIFactoryRef<PhraseReviewSessionResultArgs, PhraseReviewSessionResultArgs>();
    public static readonly UIFactoryRef<EmoticonInitArgs, object> Emoticon = new UIFactoryRef<EmoticonInitArgs, object>();
    public static readonly UIFactoryRef<ChatSpeechBubbleUIInitArgs, object> SpeechBubble = new UIFactoryRef<ChatSpeechBubbleUIInitArgs, object>();

    // Selection UIs
    public static readonly UIFactoryRef<PhraseSelectorInitArgs, PhraseSequence> PhraseSelector = new UIFactoryRef<PhraseSelectorInitArgs, PhraseSequence>();
    public static readonly UIFactoryRef<PhraseConstructorArgs, List<PhraseSequence>> PhraseConstructor = new UIFactoryRef<PhraseConstructorArgs, List<PhraseSequence>>();
    public static readonly UIFactoryRef<PhraseSequence, PhraseSequence> PhraseEditor = new UIFactoryRef<PhraseSequence, PhraseSequence>();
    public static readonly UIFactoryRef<PhraseSequenceElement, PhraseSequenceElement> WordSelector = new UIFactoryRef<PhraseSequenceElement, PhraseSequenceElement>();
    public static readonly UIFactoryRef<object, int> NumberEntry = new UIFactoryRef<object, int>();
    public static readonly UIFactoryRef<List<ValuedItem>, ValuedItem> ValuedMenu = new UIFactoryRef<List<ValuedItem>, ValuedItem>();
    public static readonly UIFactoryRef<List<TextImageItem>, TextImageItem> ImageTextMenu = new UIFactoryRef<List<TextImageItem>, TextImageItem>();
    public static readonly UIFactoryRef<JobPanelArgs, DaySessionArgs> Jobs = new UIFactoryRef<JobPanelArgs, DaySessionArgs>();
    public static readonly UIFactoryRef<TaskSelectorArgs, JobTaskRef> Tasks = new UIFactoryRef<TaskSelectorArgs, JobTaskRef>();
    public static readonly UIFactoryRef<List<PhraseSequence>, PhraseSequence> PhraseSequenceMenu = new UIFactoryRef<List<PhraseSequence>, PhraseSequence>();
    public static readonly UIFactoryRef<ShopInitArgs, object> ShopPanel = new UIFactoryRef<ShopInitArgs, object>();
    public static readonly UIFactoryRef<object, object> HomeShopPanel = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, HomeRef> HomeSelectionPanel = new UIFactoryRef<object, HomeRef>();
    public static readonly UIFactoryRef<List<TextMenuItem>, TextMenuItem> TextMenu = new UIFactoryRef<List<TextMenuItem>, TextMenuItem>();
    public static readonly UIFactoryRef<object, object> Equipment = new UIFactoryRef<object, object>();

    // UIs for indicating status
    public static readonly UIFactoryRef<ContextActionArgs, object> ContextActionStatus = new UIFactoryRef<ContextActionArgs, object>();
    public static readonly UIFactoryRef<object, object> ClickToContinue = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<TimeSpan, object> Clock = new UIFactoryRef<TimeSpan, object>();
    public static readonly UIFactoryRef<string, object> TaskStatus = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<object, object> MoneyState = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> KanaTable = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> DialogueConfidence = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<List<PhraseSequence>, object> NeededWords = new UIFactoryRef<List<PhraseSequence>, object>();

    // Elements
    public static readonly UIFactoryRef<UITargetedMessageArgs, object> HighlightBox = new UIFactoryRef<UITargetedMessageArgs, object>();
    public static readonly UIFactoryRef<object, object> MovementTutorial = new UIFactoryRef<object, object>();

    //Pre-start UI
    public static readonly UIFactoryRef<object, PlayerData> PlayerLoader = new UIFactoryRef<object, PlayerData>();
    public static readonly UIFactoryRef<object, bool> PrestartMainMenu = new UIFactoryRef<object, bool>();
    public static readonly UIFactoryRef<object, object> LeaderBoard = new UIFactoryRef<object, object>();

	// Start UI
	public static readonly UIFactoryRef<string, object> StartMenu = new UIFactoryRef<string, object>();
	public static readonly UIFactoryRef<string, object> SignupMenu = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<string, object> SigninMenu = new UIFactoryRef<string, object>();
    public static readonly UIFactoryRef<object, object> Settings = new UIFactoryRef<object, object>();

    // Network UIS
    public static readonly UIFactoryRef<object, object> ChatBox = new UIFactoryRef<object, object>();

    // Other UIs
    public static readonly UIFactoryRef<ExploreInitArgs, ExploreResultArgs> ContextActionController = new UIFactoryRef<ExploreInitArgs, ExploreResultArgs>();
    public static readonly UIFactoryRef<object, object> PlayerController = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<string, string> ContextActionButton = new UIFactoryRef<string, string>();
    //public static readonly UIFactoryRef<GameObject, object> ConversationCamera = new UIFactoryRef<GameObject, object>();
    public static readonly UIFactoryRef<object, PhraseReviewSessionResultArgs> Review = new UIFactoryRef<object, PhraseReviewSessionResultArgs>();
    public static readonly UIFactoryRef<object, PhraseReviewSessionResultArgs> KanaReview = new UIFactoryRef<object, PhraseReviewSessionResultArgs>();
	public static readonly UIFactoryRef<string, object> MiniMap = new UIFactoryRef<string, object>();
	public static readonly UIFactoryRef<object, object> QuestHUD = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> MainMenu = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> Help = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<string, object> Dictionary = new UIFactoryRef<string, object>();

    // Debug UIs
    public static readonly UIFactoryRef<object, object> DebugMenu = new UIFactoryRef<object, object>();
    public static readonly UIFactoryRef<object, object> SkipSessionButton = new UIFactoryRef<object, object>();

    public static readonly UIFactoryRef<object, object> SurveyPrompt = new UIFactoryRef<object, object>();

    public static ITemporaryUI<string, object> GetPositiveFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<PopInAndExitUI>("UI/PositiveFeedback");
    }

    public static ITemporaryUI<string, object> GetNegativeFeedbackInstance() {
        return GameObjectUtil.GetResourceInstance<NegativeFeedbackUI>("UI/NegativeFeedback");
    }

    public static ITemporaryUI<string, object> GetMessageInstance() {
        return GameObjectUtil.GetResourceInstance<UIFadeInAndOut>("UI/Element/Message");
    }

    // UIs that operate on events still need to be touched to hook the event
    public static void InitializeFloatingNames() { FloatingNameUI.Initialize(); }

    static GameObject _statusUI;
    public static ICloseable GetStatusUI() {
        if (!_statusUI) {
            _statusUI = GameObjectUtil.GetResourceInstance("UI/StaticUI");
            MainCanvas.main.Add(_statusUI.transform, CanvasBranch.Persistent);
        }
        return _statusUI.GetInterface<ICloseable>();
    }

}
