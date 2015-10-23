using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IQuestCompleteMessage {
    bool IsComplete { get; }
}

public class QuestCompleteMessage : IQuestCompleteMessage {
    public bool IsComplete { get { return true; } }
}
