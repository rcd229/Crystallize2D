using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace JapaneseTools {
	public class KanaConverter  {

		const string HiraganaSokuon = "っ";
		const string KatakanaSokuon = "ッ";

		static KanaConverter instance;
		//static string filePath = "/assets/Packages/KanaConverter/Core/KanaTable.txt";

		public static KanaConverter Instance {
			get {
				if (instance == null) {
					instance = new KanaConverter ();
				}
				return instance;
			}
		}

		Dictionary<string, string> romajiToHiragana = new Dictionary<string, string> ();
		Dictionary<string, string> hiraganaToRomaji = new Dictionary<string, string> ();
		Dictionary<string, string> katakanaToHiragana = new Dictionary<string, string> ();
		Dictionary<string, string> hiraganaToKatakana = new Dictionary<string, string> ();

		public KanaConverter(){
			Load ();
		}

		void Load(){
			//Debug.Log (Directory.GetCurrentDirectory () + filePath);
			/*using (var reader = new StreamReader(Directory.GetCurrentDirectory() + filePath)) {
				var text = reader.ReadToEnd();
				var kanaSets = text.Split(new char[]{'\t', '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
				foreach(var kanaSet in kanaSets){
					var elements = kanaSet.Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
					if(elements.Length == 3){
						var romaji = elements[0];
						var hiragana = elements[1];
						var katakana = elements[2];
						romajiToHiragana[romaji] = hiragana;
						hiraganaToRomaji[hiragana] = romaji;
						katakanaToHiragana[katakana] = hiragana;
						hiraganaToKatakana[hiragana] = katakana;
					} else {
						Debug.Log(kanaSet + " invalid");
					}
				}
				//Debug.Log(kanaSets.Length);
			}*/

			var text = Resources.Load<TextAsset> ("KanaTable");
			var kanaSets = text.text.Split(new char[]{'\t', '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach(var kanaSet in kanaSets){
				var elements = kanaSet.Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
				if(elements.Length == 3){
					var romaji = elements[0];
					var hiragana = elements[1];
					var katakana = elements[2];
					romajiToHiragana[romaji] = hiragana;
					hiraganaToRomaji[hiragana] = romaji;
					katakanaToHiragana[katakana] = hiragana;
					hiraganaToKatakana[hiragana] = katakana;
				} else {
					Debug.Log(kanaSet + " invalid");
				}
			}
		}

		public string ConvertToRomaji(string text){
			if (text == null) {
				return string.Empty;
			}

			var romajiText = "";
//            var tmpText = "";
            bool tsu = false;
			for (int i = 0; i < text.Length; i++) {
				var charString = text[i].ToString();
                if (charString == "っ") {
                    tsu = true;
                    continue;
                }

				if(i < text.Length - 1){
					var substring = text.Substring(i, 2);
					if(hiraganaToRomaji.ContainsKey(substring)){
                        romajiText += AddSegment(hiraganaToRomaji[substring], ref tsu);
						i++;
						continue;
					} else if(katakanaToHiragana.ContainsKey(substring)){
						romajiText += AddSegment(hiraganaToRomaji[katakanaToHiragana[substring]], ref tsu);
						i++;
						continue;
					} 
				}

				if(hiraganaToRomaji.ContainsKey(charString)){
					romajiText += AddSegment(hiraganaToRomaji[charString], ref tsu);
				} else if(katakanaToHiragana.ContainsKey(charString)){
					romajiText += AddSegment(hiraganaToRomaji[katakanaToHiragana[charString]],ref tsu);
				} else {
					romajiText += AddSegment(text[i].ToString(), ref tsu);
				}
			}

            //romajiToHiragana["っ"] = "っ";
            //hiraganaToRomaji["っ"] = "";

			return romajiText;
		}

        string AddSegment(string text, ref bool tsu) {
            if (tsu) {
                text = text[0] + text;
            }
            tsu = false;
            return text;
        }

		public string ConvertToHiragana(string text){
			if (text == null) {
				return string.Empty;
			}
			
			var hiraganaText = "";
			for (int i = 0; i < text.Length; i++) {
				if(i < text.Length - 2){
					if(!text[i].IsVowel() && text[i] == text[i+1] && text[i] != 'n'){
						hiraganaText += HiraganaSokuon;
						continue;
					}

					var substring = text.Substring(i, 3);
					if(romajiToHiragana.ContainsKey(substring)){
						hiraganaText += romajiToHiragana[substring];
						i += 2;
						continue;
					}
				}

				if(i < text.Length - 1){
					var substring = text.Substring(i, 2);
					if(romajiToHiragana.ContainsKey(substring)){
						hiraganaText += romajiToHiragana[substring];
						i++;
						continue;
					} else if(katakanaToHiragana.ContainsKey(substring)){
						hiraganaText += katakanaToHiragana[substring];
						i++;
						continue;
					} 
				}

				var charString = text[i].ToString();
				if(romajiToHiragana.ContainsKey(charString)){
					hiraganaText += romajiToHiragana[charString];
				} else if(katakanaToHiragana.ContainsKey(charString)){
					hiraganaText += katakanaToHiragana[charString];
				} else {
					hiraganaText += text[i];
				}
			}
			
			return hiraganaText;
		}

		public string ConvertToKatakana(string text){
			if (text == null) {
				return string.Empty;
			}
			
			var katakanaText = "";
			for (int i = 0; i < text.Length; i++) {
				if(i < text.Length - 2){
					var substring = text.Substring(i, 3);
					if(romajiToHiragana.ContainsKey(substring)){
						katakanaText += hiraganaToKatakana[romajiToHiragana[substring]];
						i += 2;
						continue;
					}
				}
				
				if(i < text.Length - 1){
					var substring = text.Substring(i, 2);
					if(romajiToHiragana.ContainsKey(substring)){
						katakanaText += hiraganaToKatakana[romajiToHiragana[substring]];
						i++;
						continue;
					} else if(hiraganaToKatakana.ContainsKey(substring)){
						katakanaText += hiraganaToKatakana[substring];
						i++;
						continue;
					} 
				}
				
				var charString = text[i].ToString();
				if(romajiToHiragana.ContainsKey(charString)){
					katakanaText += hiraganaToKatakana[romajiToHiragana[charString]];
				} else if(hiraganaToKatakana.ContainsKey(charString)){
					katakanaText += hiraganaToKatakana[charString];
				} else {
					katakanaText += text[i];
				}
			}
			
			return katakanaText;
		}

	}
}