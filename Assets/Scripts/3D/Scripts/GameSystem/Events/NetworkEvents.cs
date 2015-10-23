using UnityEngine;
using System;
using System.Collections;

public class NetworkEvents {

    public event EventHandler OnConnectedToNetwork;
    public void RaiseConnectedToNetwork(object sender, System.EventArgs args) { OnConnectedToNetwork.Raise(sender, args); }

    public event EventHandler<NetworkSpeechBubbleRequestedEventArgs> OnNetworkSpeechBubbleRequested;
    public void RaiseNetworkSpeechBubbleRequested(object sender, NetworkSpeechBubbleRequestedEventArgs args) { OnNetworkSpeechBubbleRequested.Raise(sender, args); }

    public event EventHandler<NetworkEmoteArgs> OnNetworkEmoteRequested;
    public void RaiseNetworkEmoteRequested(object sender, NetworkEmoteArgs args) { OnNetworkEmoteRequested.Raise(sender, args); }

    public event EventHandler OnNetworkPlayerFeedbackRequested;
    public void RaiseNetworkPlayerFeedbackRequested(object sender, EventArgs args) { OnNetworkPlayerFeedbackRequested.Raise(sender, args); }

    public event EventHandler<PartnerObjectiveCompleteEventArgs> OnSendQuestStateRequested;
    public void RaiseSendQuestStateRequested(object sender, PartnerObjectiveCompleteEventArgs args) { OnSendQuestStateRequested.Raise(sender, args); }

    public event EventHandler<TextEventArgs> OnEnglishLineInput;
    public void RaiseEnglishLineInput(object sender, TextEventArgs args) { OnEnglishLineInput.Raise(sender, args); }

}
