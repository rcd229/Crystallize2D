using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

public class DataWriter  : MonoBehaviour{

	void Start(){
		var phrases = PhraseSetCollectionGameData.Default.Phrases.Where((p) => !p.HasContextData).ToList();
		int numQuestions = phrases.Count;
		StreamWriter writer = new StreamWriter("PhraseQuestion", false);
		for(int i = 0; i < numQuestions; i++){
			var needed = phrases[i];
            var question = CollectionExtensions.RandomSubsetWithValue(phrases, needed, 4);
            int neededPosition = CollectionExtensions.GetNeededIndex(question, needed);
			StringBuilder sb = new StringBuilder();
			//write the question
			sb.Append(needed.GetText(JapaneseTools.JapaneseScriptType.Romaji));
			sb.Append("\t");
			//write the answer choices
			foreach(var q in question){
				sb.Append(q.Translation);
				sb.Append("\t");
			}
			//write the position of the correct answer
			sb.Append(neededPosition);

			writer.WriteLine(sb.ToString());
		}
		writer.Close();
		Debug.Log("Done writing");


	}
}
