using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml.Serialization;

//a dialogue tree represents the unlock dependency of dialogues
//When the parent dialogue is complete/chosen, the child dialogue will be unlocked.

public class DialogueTree {
	//internal representation of the tree
	public class DialogueTreeNode{
		public PhraseSequence Choice {get;set;}
		public DialogueSequence Answer{get;set;}
		public List<DialogueTreeNode> Children{get;set;}
		//whether the node will be permanently unlocked. (eg. greetings are not but quests may be)
		public bool isPermanent {get;set;}
		public List<int> record {get;set;}
		[XmlIgnore]
		public bool Visited {get;set;}
		[XmlIgnore]
		public Action Callback{get;set;}
		public DialogueTreeNode(){
			Choice = new PhraseSequence();
			Answer = new DialogueSequence();
			Children = new List<DialogueTreeNode>();
			Callback = null;
			isPermanent = false;
			record = new List<int>();

		}
	}

	public DialogueTree(){
		Root = new DialogueTreeNode();
		CurrentUnlock = new List<DialogueTreeNode>();
		HistoryUnlock = new List<DialogueTreeNode>();
	}

	public DialogueTreeNode Root {get;set;}
	//the nodes that are exposed in the current conversation. TODO set to empty when conversation ends
	public List<DialogueTreeNode> CurrentUnlock {get;set;}
	//the nodes that are exposed in history conversation whose progresses are permenant.
	public List<DialogueTreeNode> HistoryUnlock {get;set;}

	public IEnumerable<PhraseSequence> GetChoices(){
		return CurrentUnlock.Select(s => s.Choice);
	}

	//look up the Dialogue that answers the selected phrase
	//rep invar: no two phrases can be equivalent.
	public DialogueSequence PeekAnswer(PhraseSequence input){
		Debug.Assert(CurrentUnlock.Where(s => PhraseSequence.PhrasesEquivalent(s.Choice, input)).Count() == 1);
		return CurrentUnlock.Where(s => PhraseSequence.PhrasesEquivalent(s.Choice, input)).FirstOrDefault().Answer;
	}

	//execute call back will call any Callback of the node if it exists
	public DialogueSequence GetAnswerAndUnlockChildren(PhraseSequence input, bool executeCallBack = true){
		//TODO
		DialogueTreeNode node;
		if(FreeExploreProcess.isPhraseEnglish){
			node = CurrentUnlock.Where(s => s.Choice.GetText() ==  input.GetText()).FirstOrDefault();
		}else{
			Debug.Assert(CurrentUnlock.Where(s => PhraseSequence.PhrasesEquivalent(s.Choice, input)).Count() == 1);
			node = CurrentUnlock.Where(s => PhraseSequence.PhrasesEquivalent(s.Choice, input)).FirstOrDefault();
		}

		//execute callback if required
		if(executeCallBack && node.Callback != null){
			node.Callback();
		}
		//unlock children. If children is permanently unlocked, reflect that in HistoryUnlock
		int index = CurrentUnlock.IndexOf(node);
		foreach(var child in node.Children){
			if(!CurrentUnlock.Contains(child)){
				CurrentUnlock.Insert(index, child);
			}
			if(child.isPermanent){
				if(!HistoryUnlock.Contains(child)){
					HistoryUnlock.Add(child);
				}
			}
		}
		//remove parents
		if(HistoryUnlock.Contains(node)){
			HistoryUnlock.Remove(node);
		}
		node.Visited = true;
//		CurrentUnlock.Remove(node);
		return node.Answer;
	}

	public void ClearCurrent(){
		CurrentUnlock = new List<DialogueTreeNode>();
	}

	public void InitDialogueTree(){
		//as a safety measure. Should already be reset
//		Debug.Assert(CurrentUnlock.Count == 0);
		//TODO generate a snapshot of all the nodes

		//load history unlock from player data
		//TODO this works if game session is single player. Otherwise load data when interaction happens
		CurrentUnlock = new List<DialogueTreeNode>();
		CurrentUnlock.AddRange(HistoryUnlock);
		if(CurrentUnlock.Count == 0){
			CurrentUnlock.Add(Root);
		}
	}

	///traverse the tree and get to the exposed nodes from the record of tree traversal
	public List<DialogueTreeNode> GetFromRecord(List<List<int>> record){
		List<DialogueTreeNode> ret = new List<DialogueTreeNode>();
		foreach(var list in record){
			DialogueTreeNode current = Root;
			foreach(int step in list){
				current = current.Children[step];
			}
			ret.Add(current);
		}
		return ret;
	}

	public List<List<int>> ToRecord(){
		List<List<int>> ret = new List<List<int>>();
		foreach(var node in HistoryUnlock){
			ret.Add(node.record);
		}
		return ret;
	}

	//generate a record of the positions of all the nodes in the tree
	public void GenerateSnapShot(){
		GenerateSnapShot(new List<int>(), Root, -1);
	}

	void GenerateSnapShot(List<int> parent, DialogueTreeNode current, int index){
		current.record = new List<int>();
		current.record.AddRange(parent);
		if(index >= 0){
			current.record.Add(index);
		}
		for(int i = 0; i < current.Children.Count; i++){
			GenerateSnapShot(current.record, current.Children[i], i); 
		}
	}


}
