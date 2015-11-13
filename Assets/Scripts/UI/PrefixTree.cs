using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PrefixTree<T> where T : Prefixable  {
	public class Node{
		public string letter;
		public T data;
		public Dictionary<string, Node> edges;
		public Node(string letter){
			this.letter = letter;
			edges = new Dictionary<string, Node>();
			data = default(T);
		}
	}
	Node root = new Node("");

	public PrefixTree(IEnumerable<T> data){
        int count = 0;
        foreach (var datum in data){
			add (datum);
            count++;
		}
	}

	public void add(T data){
		if(data.getPrefixableText().Length == 0) {
			return;
		}
		add (data, root, 0);
	}

	void add(T data, Node current, int index){
		//add data to the node if this is the path describing its prefixable text
		if(index == data.getPrefixableText().Length - 1){
			current.data = data;
		}else{
			string letter = data.getPrefixableText().Substring(index, 1);
			if(!current.edges.ContainsKey(letter)){
				current.edges.Add(letter, new Node(letter));
			}
			add (data, current.edges[letter], index + 1);
		}
	}

	public List<T> withPrefix(string prefix){
		List<T> retValue = new List<T>();
		if(prefix.IsEmptyOrNull()){
			return retValue;
		}
		Node node = locatePrefix(prefix, 0, root);
		if(node == null){
			return retValue;
		}
		findAll(node, retValue);
		return retValue;
	}

	//find all data at and under this node
	void findAll(Node parent, List<T> list){
		if(parent.data != null){
			list.Add(parent.data);
		}
		foreach (var child in parent.edges.Values){
			findAll(child, list);
		}
	}

	Node locatePrefix(string prefix, int index, Node current){
		if(index == prefix.Length){
			return current;
		}else{
			string letter = prefix.Substring(index, 1);
			if(current.edges.ContainsKey(letter)){
				return locatePrefix(prefix, index + 1, current.edges[letter]);
			}
			return null;
		}
	}
}
