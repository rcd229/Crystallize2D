using UnityEngine;
using System.Collections;

public class PhraseClass {

	public int ID { get; set; }
	public string Function { get; set; }
	public string Parameter { get; set; }

	public PhraseClass(){}

	public PhraseClass(int id, string function, string parameter){
		this.ID = id;
		this.Function = function;
		this.Parameter = parameter;
	}

}
