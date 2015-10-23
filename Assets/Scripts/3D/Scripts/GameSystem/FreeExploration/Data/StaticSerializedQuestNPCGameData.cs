using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace CrystallizeData {

    public class StaticSerializedAttributeQuestNPCGameData : StaticSerializedGameData {
        List<QuestNPCItemData> npcs = new List<QuestNPCItemData>();

        protected override void AddGameData() {
            foreach (var npc in npcs) {
                GameData.Instance.NPCs.QuestNPCs.Add(npc);
            }
            GameData.Instance.Spawn.AddSpawner(new NPCGroupSpawner());
        }

        protected override void PrepareGameData() {
            int count = 0;
            var types = (from t in Assembly.GetAssembly(typeof(GeneratedNPC)).GetTypes()
                         where Attribute.IsDefined(t, typeof(SerializedNPCsAttribute))
                         select t);
            foreach (var t in types) {
                var fields = from f in t.GetFields(BindingFlags.Static | BindingFlags.Public)
                             where typeof(GeneratedNPC).IsAssignableFrom(f.FieldType)
                             select f;
                foreach (var f in fields) {
                    var val = f.GetValue(null) as GeneratedNPC;
                    if (val != null && val.NPC != null) {
                        GameDataInitializer.AddNPC(val.NPC);
                        npcs.Add(val.NPC);
                        count++;
                    } else {
                        Debug.Log("Unable to add NPC for " + f.Name);
                    }
                }

                var collectionFields = from f in t.GetFields(BindingFlags.Static | BindingFlags.Public)
                                       where typeof(IEnumerable<GeneratedNPC>).IsAssignableFrom(f.FieldType)
                                       select f;
                foreach (var f in collectionFields) {
                    var val = f.GetValue(null) as IEnumerable<GeneratedNPC>;
                    if (val != null) {
                        foreach (var item in val) {
                            GameDataInitializer.AddNPC(item.NPC);
                            npcs.Add(item.NPC);
                            count++;
                        }
                    } else {
                        Debug.Log("Unable to add NPC for " + f.Name);
                    }
                }
            }

            var quests = SerializedQuestExtensions.GetAllQuests();
            foreach (var q in quests) {
                var instanceNPCs = from f in q.GetType().GetFieldsAndProperties<QuestNPCItemData>(BindingFlags.Public | BindingFlags.Instance)
                                   select f.GetMemberValue(q) as QuestNPCItemData;
                foreach (var npc in instanceNPCs) {
                    GameDataInitializer.AddNPC(npc);
                    npcs.Add(npc);
                    count++;
                }

                var instanceGroups = from f in q.GetType().GetFieldsAndProperties<NPCGroup>(BindingFlags.Public | BindingFlags.Instance)
                                     select f.GetMemberValue(q) as NPCGroup;
                foreach (var npc in instanceGroups) {
                    GameDataInitializer.AddNPCGroup(npc);
                    var d = npc.Dialogue;
                    count++;
                }
            }
            Debug.Log("Found " + count + " serialized NPCs.");
        }
    }

    public abstract class StaticSerializedQuestNPCGameData : StaticSerializedGameData {

        protected QuestNPCItemData NPC = new QuestNPCItemData();
        INPCSpawner spawnable;

        protected override void AddGameData() {
            GameData.Instance.NPCs.QuestNPCs.Add(NPC);
            if (spawnable != null) {
                GameData.Instance.Spawn.AddSpawner(spawnable);
            }
        }

        protected void Initialize(NPCID id, string consumerName) {
            NPC.ID = id;
            NPC.TargetName = consumerName;

            GameDataInitializer.AddNPC(NPC);
        }

        protected void AddSpawner(INPCSpawner spawner) {
            spawnable = spawner;
        }

        protected void SetEntryDialogue(DialogueSequence dialogue) {
            NPC.EntryDialogue = dialogue;
        }

        protected void SetExitDialogue(DialogueSequence dialogue) {
            NPC.ExitDialogue = dialogue;
        }

        //NPC will only appear after these quests are completed
        protected void AddQuestFlagRequirement(params Guid[] flags) {
            var flagList = NPCQuestFlag.GetIDs().ToList();
            foreach (var flag in flags) {
                if (!flagList.Where(s => s.Guid == flag).Any()) {
                    Debug.LogError("this guid is not a flag guid ");
                } else {
                    NPC.FlagPrerequisites.Add(flag);
                }
            }
        }

        //		//TODO enabling the adding of tree node.
        //		protected DialogueTree.DialogueTreeNode AddTreeNode(DialogueTree.DialogueTreeNode parent, PhraseSequence choice, DialogueSequence answer, bool permanent, Action callback = null){
        //			var node = new DialogueTree.DialogueTreeNode();
        //			node.Answer = answer;
        //			node.Choice = choice;
        //			node.isPermanent = permanent;
        //			node.Callback = callback;
        //			node.Children = new List<DialogueTree.DialogueTreeNode>();
        //			if(parent != null){
        //				parent.Children.Add(node);
        //			}
        //			else{
        //				NPC.Dialogues = new DialogueTree();
        //				NPC.Dialogues.Root = node;
        //			}
        //			return node;
        //		}
    }
}
