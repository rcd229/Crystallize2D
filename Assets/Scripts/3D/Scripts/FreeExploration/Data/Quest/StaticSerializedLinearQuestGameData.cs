using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystallizeData {
    public abstract class StaticSerializedLinearQuestGameData : StaticSerializedLinearQuestGameData<LinearQuestStateMachine> { }

    public abstract class StaticSerializedLinearQuestGameData<T> : StaticSerializedQuestGameData<T> where T : LinearQuestStateMachine, new() {
        List<QuestDialogueState> currentStates;

        protected abstract override void Prepare();

        /** Quest state methods **/

        //note: sequencing matters
        protected void SetStates(params string[] states) {
            stateMachine.SetStates(states);
        }

        //protected void AddState(string state) {
        //    quest.StateMachine.AddState(state);
        //}

        /** quest dialogue methods **/
        protected void AddNPCQuestDialogues(NPCID npc, params QuestDialogueState[] questDialogueStates) {
            var questDialogueData = stateMachine.Dialogues.Where(s => s.NPCID == npc).FirstOrDefault();
            if (questDialogueData == null) {
                var newData = new QuestDialogueData();
                newData.NPCID = npc;
                newData.DialogueStates.AddRange(questDialogueStates);
                stateMachine.Dialogues.Add(newData);
            } else {
                questDialogueData.DialogueStates.AddRange(questDialogueStates);
            }
        }

        protected void AddNPCQuestDialogue(NPCID npc, string questState, PhraseSequence prompt, DialogueSequence dialogue) {
            var questDialogueData = stateMachine.Dialogues.Where(s => s.NPCID == npc).FirstOrDefault();
            if (questDialogueData == null) {
                var newData = new QuestDialogueData();
                newData.NPCID = npc;
                newData.DialogueStates.Add(new QuestDialogueState(questState, dialogue, prompt));
                stateMachine.Dialogues.Add(newData);
            } else {
                questDialogueData.DialogueStates.Add(new QuestDialogueState(questState, dialogue, prompt));
            }
        }

        /// <summary>
        /// change the current npc to the specified npc
        /// this will add an entry for this npc in the questdialogue table
        /// </summary>
        protected void SetCurrentNPC(NPCID npc) {
            var questDialogueData = stateMachine.Dialogues.Where(s => s.NPCID == npc).FirstOrDefault();
            if (questDialogueData == null) {
                var newData = new QuestDialogueData();
                newData.NPCID = npc;
                currentStates = newData.DialogueStates;
                stateMachine.Dialogues.Add(newData);
            } else {
                currentStates = questDialogueData.DialogueStates;
            }
        }

        /// <summary>
        /// add a dialogue state to the current npc
        /// </summary>
        protected void AddDialogueStateToCurrent(string questState, PhraseSequence prompt, DialogueSequence dialogue, Func<ContextData> contextGetter = null) {
            currentStates.Add(new QuestDialogueState(questState, dialogue, prompt, contextGetter));
        }

        ///add a context getter to the current npc and the last state in CurrentState
        protected void AddContextGetter(Func<ContextData> contextGetter) {
            var last = currentStates.LastOrDefault();
            if (last != null) {
                last.ContextGetter = contextGetter;
            }
        }
    }
}
