using UnityEngine;
using System;
using System.Collections;

public class UIEvents : GameEvents{

    public event EventHandler<PhraseEventArgs> OnBeginDragWord;
    public void RaiseBeginDragWord(object sender, PhraseEventArgs args) { OnBeginDragWord.Raise(sender, args); }
    public event EventHandler<PhraseEventArgs> OnDropWord;
    public void RaiseOnDropWord(object sender, PhraseEventArgs args) { OnDropWord.Raise(sender, args); }

    public event EventHandler<UIRequestEventArgs> OnUIRequested;
    public void RaiseUIRequest(object sender, UIRequestEventArgs args) { OnUIRequested.Raise(sender, args); }
    public event EventHandler OnUIInteraction;
    public void RaiseUIInteraction(object sender, EventArgs args) { OnUIInteraction.Raise(sender, args); }

    public event EventHandler<SpeechBubbleRequestedEventArgs> OnSpeechBubbleRequested;
	public void RaiseSpeechBubbleRequested(object sender, SpeechBubbleRequestedEventArgs args) { OnSpeechBubbleRequested.Raise(sender, args); }
    public event EventHandler<SpeechBubbleRequestedEventArgs> OnSpeechBubbleOpen;
    public void RaiseSpeechBubbleOpen(object sender, SpeechBubbleRequestedEventArgs args) { OnSpeechBubbleOpen.Raise(sender, args); }

    public event EventHandler<PhraseClickedEventArgs> BeforeWordClicked;
    public void RaiseBeforeWordClicked(object sender, PhraseClickedEventArgs args) { BeforeWordClicked.Raise(sender, args); }
    public event EventHandler<PhraseClickedEventArgs> OnWordClicked;
    public void RaiseWordClicked(object sender, PhraseClickedEventArgs args) { OnWordClicked.Raise(sender, args); }
    public event EventHandler<PhraseClickedEventArgs> OnWordDragged;
    public void RaiseWordDragged(object sender, PhraseClickedEventArgs args) { OnWordDragged.Raise(sender, args); }

    public event EventHandler<PhraseClickedEventArgs> BeforePhraseClicked;
    public void RaiseBeforePhraseClicked(object sender, PhraseClickedEventArgs args) { BeforePhraseClicked.Raise(sender, args); }
    public event EventHandler<PhraseClickedEventArgs> OnPhraseClicked;
    public void RaisePhraseClicked(object sender, PhraseClickedEventArgs args) { OnPhraseClicked.Raise(sender, args); }
    public event EventHandler<PhraseClickedEventArgs> OnPhraseDragged;
    public void RaisePhraseDragged(object sender, PhraseClickedEventArgs args) { OnPhraseDragged.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnPhraseDropped;
    public void RaisePhraseDropped(object sender, PhraseEventArgs args) { OnPhraseDropped.Raise(sender, args); }

    public event EventHandler OnInteractiveDialogueOpened;
    public void RaiseInteractiveDialogueOpened(object sender, EventArgs args) { OnInteractiveDialogueOpened.Raise(sender, args); }
    public event EventHandler OnInteractiveDialogueClosed;
    public void RaiseInteractiveDialogueClosed(object sender, EventArgs args) { OnInteractiveDialogueClosed.Raise(sender, args); }

    public event EventHandler OnUpdateUI;
    public void RaiseUpdateUI(object sender, EventArgs args) { OnUpdateUI.Raise(sender, args); }
    public event EventHandler OnProgressEvent;
    public void RaiseOnProgressEvent(object sender, EventArgs args) { OnProgressEvent.Raise(sender, args); }

    public event EventHandler<PhraseEventArgs> OnBasePhraseSelected;
    public void RaiseBasePhraseSelected(object sender, PhraseEventArgs args) { OnBasePhraseSelected.Raise(sender, args); }
    
    public event EventHandler<PhraseEventArgs> OnWordSelected;
    public void RaiseWordSelected(object sender, PhraseEventArgs args) { OnWordSelected.Raise(sender, args); }

    public event EventHandler<FloatingNameEventArgs> OnFloatingNameRequested;
    public void RaiseFloatingNameRequested(object sender, FloatingNameEventArgs args) { OnFloatingNameRequested.Raise(sender, args); }

    public event EventHandler OnGoHomeClicked;
    public void RaiseGoHomeClicked(object sender, EventArgs args) { OnGoHomeClicked.Raise(sender, args); }

    public event EventHandler<HUDPartArgs> OnHUDPartStateChanged;
    public void RaiseHUDPartStateChanged(object sender, HUDPartArgs args) { OnHUDPartStateChanged.Raise(sender, args); }

    public event EventHandler OnTutorialEvent;
    public void RaiseTutorialEvent(object sender, EventArgs args) { OnTutorialEvent.Raise(sender, args); }

}
