using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JapaneseTools {
	public class WordFrequency {

		const string FileName = "WordFrequencyList";

		static string[] _words;

		static string[] Words { 
			get {
				if(_words == null){
					var asset = Resources.Load<TextAsset>(FileName);
					_words = asset.text.Split(new [] { '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
					Debug.Log(_words.Length + " words found.");
				}
				return _words;
			}
		}

		public static string[] GetFrequentWords(int count){
			var arr = new string[count];
			System.Array.Copy (Words, 0, arr, 0, count); 
			return arr;
		}

	}
}