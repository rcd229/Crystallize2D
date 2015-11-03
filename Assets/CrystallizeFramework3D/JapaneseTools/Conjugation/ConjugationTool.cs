using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JapaneseTools {
    public enum JapaneseScriptType {
        Kanji = 0,
        Kana = 1,
        Romaji = 2
    }

    public class ConjugationTool {

        const int PlainPresentIndicative = 100;
        const int PolitePresentIndicative = 110;
        
        const int PlainPresentNegative = 120;
        const int PolitePresentNegative = 130;
        
        const int PlainPastIndicitive = 140;
        const int PolitePastIndicitive = 150;
        
        const int PlainPresentPresumptive = 160;
        const int PolitePresentPresumptive = 170;

        const int PlainPastPresumptive = 180;
        const int PolitePastPresumptive = 190;
        
        const int Want = 200;

        const int Te = 210;

        const int AdjectivePlain = 100;
        const int AdjectiveNa = 110;
        const int AdjectiveI = 120;

        const int DefaultVerbForm = PolitePresentIndicative;

        static Dictionary<int, string> group1VowelEndings = new Dictionary<int, string>();
        static Dictionary<int, string> verbEndings = new Dictionary<int, string>();
        static Dictionary<char, string> teEndings = new Dictionary<char, string>();
        static Dictionary<string, Dictionary<int, string>> irregularVerbs = new Dictionary<string, Dictionary<int, string>>();
        static Dictionary<int, string> adjectiveEndings = new Dictionary<int, string>();
        static Dictionary<string, List<string>> additionalForms = new Dictionary<string, List<string>>();
        static Dictionary<string, string> endingMapping = new Dictionary<string, string>();

        static ConjugationTool() {
            group1VowelEndings[PlainPresentIndicative] = "u";
            group1VowelEndings[PolitePresentIndicative] = "i";
            
            group1VowelEndings[PlainPresentNegative] = "a";
            group1VowelEndings[PolitePresentNegative] = "i";

            group1VowelEndings[PlainPastIndicitive] = "i";
            group1VowelEndings[PolitePastIndicitive] = "i";

            group1VowelEndings[PlainPresentPresumptive] = "u";
            group1VowelEndings[PolitePresentPresumptive] = "i";

            group1VowelEndings[PlainPastPresumptive] = "i";
            group1VowelEndings[PolitePastPresumptive] = "i";

            verbEndings[PlainPresentIndicative] = "";
            verbEndings[PolitePresentIndicative] = "ます";

            verbEndings[PlainPresentNegative] = "ない";
            verbEndings[PolitePresentNegative] = "ません";

            verbEndings[PlainPastIndicitive] = "た";
            verbEndings[PolitePastIndicitive] = "ました";

            verbEndings[PlainPresentPresumptive] = "だろう";
            verbEndings[PolitePresentPresumptive] = "ましょう";

            verbEndings[PlainPastPresumptive] = "だろう";
            verbEndings[PolitePastPresumptive] = "ましょう";
            verbEndings[Want] = "たい";

            AddIrregularVerb("為る", "する", "します", "しない", "しません", "した", "しました", "しよう", "しましょう", "したろう", "しましたろう");
            AddIrregularVerb("する", "する", "します", "しない", "しません", "した", "しました", "しよう", "しましょう", "したろう", "しましたろう");
            AddIrregularVerb("来る", "くる", "きます", "こない", "きません", "きた", "きました", "こよう", "きましょう", "きただろう", "きたでしょう");
            AddIrregularVerb("くる", "くる", "きます", "こない", "きません", "きた", "きました", "こよう", "きましょう", "きただろう", "きたでしょう");
            AddIrregularVerb("だ", "だ", "です", "じゃない", "ではありません", "だった", "でした", "だろう", "でしょう", "だろう", "でしょう");

            adjectiveEndings[AdjectiveNa] = "な";
            adjectiveEndings[AdjectiveI] = "く";

            teEndings['う'] = "tte";
            teEndings['く'] = "ite";
            teEndings['ぐ'] = "ide";
            teEndings['す'] = "shite";
            teEndings['つ'] = "tte";
            teEndings['ぬ'] = "nde";
            teEndings['ぶ'] = "nde";
            teEndings['む'] = "nde";
            teEndings['る'] = "tte";

            additionalForms["何"] = new List<string>();
            additionalForms["何"].Add("なに");
            additionalForms["何"].Add("なん");

            endingMapping["si"] = "shi";
        }

        static void AddIrregularVerb(string key, string plPrI, string poPrI, string plPrN, string poPrN, string plPaI, string poPaI, 
            string plPrP, string poPrP, string plPaP, string poPaP) {
            var dict = new Dictionary<int, string>();
            dict[PlainPresentIndicative] = plPrI;
            dict[PolitePresentIndicative] = poPrI;
            dict[PlainPresentNegative] = plPrN;
            dict[PolitePresentNegative] = poPrN;
            dict[PlainPastIndicitive] = plPaI;
            dict[PolitePastIndicitive] = poPaI;
            dict[PlainPresentPresumptive] = plPrP;
            dict[PolitePresentPresumptive] = poPrP;
            dict[PlainPastPresumptive] = plPaP;
            dict[PolitePastPresumptive] = poPaP;
            irregularVerbs[key] = dict;
        }

        public static int[] GetAdjectiveForms() {
            return new int[] { AdjectivePlain, AdjectiveNa, AdjectiveI };
        }

        public static int[] GetVerbForms() {
            return new int[] { PlainPresentIndicative, PolitePresentIndicative, PlainPresentNegative, PolitePresentNegative, PlainPresentPresumptive, PolitePresentPresumptive,
              PlainPastIndicitive, PolitePastIndicitive, PlainPastPresumptive, PolitePastPresumptive, Want, Te };
        }

        public static string ConjugateGroup1Verb(string verb, int form) {
            var finalKana = verb[verb.Length - 1].ToString();
            var finalRomaji = KanaConverter.Instance.ConvertToRomaji(finalKana);
            if (form == Te) {
                finalRomaji = teEndings[verb[verb.Length - 1]];
                return verb.Substring(0, verb.Length - 1) + KanaConverter.Instance.ConvertToHiragana(finalRomaji);
            }
            
            if (!group1VowelEndings.ContainsKey(form)) {
                return ConjugateGroup1Verb(verb, DefaultVerbForm);
            }

            if (finalKana == "つ") {
                var ending = group1VowelEndings[form];
                if (ending == "i") {
                    finalKana = "chi";
                } else if (ending == "u") {
                    finalKana = "tsu";
                }  else {
                    finalKana = finalRomaji.Substring(0, finalRomaji.Length - 2) + ending;
                }
            } else if (finalKana == "す") {
                var ending = group1VowelEndings[form];
                if (ending == "i") {
                    finalKana = "shi";
                } else if (ending == "u") {
                    finalKana = "su";
                } else {
                    finalKana = finalRomaji.Substring(0, finalRomaji.Length - 2) + ending;
                }
            } else {
                finalKana = finalRomaji.Substring(0, finalRomaji.Length - 1) + group1VowelEndings[form];
            }
            finalKana = KanaConverter.Instance.ConvertToHiragana(finalKana);
            if (form == PlainPastIndicitive) {
                return verb.Substring(0, verb.Length - 1) + "った";
            } else {
                return verb.Substring(0, verb.Length - 1) + finalKana + verbEndings[form];
            }
        }

        public static string ConjugateGroup2Verb(string verb, int form) {
            if (form == PlainPresentIndicative) {
                return verb;
            }
            
            if (form == Te) {
                return verb.Substring(0, verb.Length - 1) + "て";
            }
            
            if (!verbEndings.ContainsKey(form)) {
                return ConjugateGroup2Verb(verb, DefaultVerbForm);
            }
            return verb.Substring(0, verb.Length - 1) + verbEndings[form];
        }

        public static string ConjugateIrregularVerb(string verb, int form) {
            if (irregularVerbs.ContainsKey(verb)) {
                if (form == Te) {
                    var conj = irregularVerbs[verb][PolitePresentIndicative];
                    return conj.Substring(0, conj.Length - 2) + "て";
                }

                if (irregularVerbs[verb].ContainsKey(form)) {
                    return irregularVerbs[verb][form];
                } else {
                    return ConjugateIrregularVerb(verb, DefaultVerbForm);
                }
            } else {
                Debug.LogWarning("Verb not found in irregular verbs!");
                return verb;
                //throw new UnityException("Verb not found in irregular verbs!");
            }
        }

        public static string GetForm(DictionaryDataEntry entry, int form, JapaneseScriptType scriptType = JapaneseScriptType.Kanji) {
            if (entry == null) {
                return null;
            }

            if (additionalForms.ContainsKey(entry.Kanji)) {
                //if (scriptType == JapaneseScriptType.Kanji) {
                //    return entry.Kanji;
                //}
                var formString = additionalForms[entry.Kanji].GetSafely(form);
                if (scriptType == JapaneseScriptType.Romaji) {
                    return KanaConverter.Instance.ConvertToRomaji(formString);
                } else {
                    return formString;
                }
            }

            switch (entry.GetPartOfSpeech()) {
                case PartOfSpeech.GodanVerb:
                case PartOfSpeech.IchidanVerb:
                case PartOfSpeech.Copula:
                case PartOfSpeech.SuruVerb:
                    switch (scriptType) {
                        case JapaneseScriptType.Romaji:
                            return KanaConverter.Instance.ConvertToRomaji(GetVerbForm(entry, form, scriptType));
                        default:
                            return GetVerbForm(entry, form, scriptType);
                    }

                case PartOfSpeech.Adjective:

				if (scriptType == JapaneseScriptType.Romaji) {
                        return KanaConverter.Instance.ConvertToRomaji(GetAdjectiveForm(entry, form, scriptType));
                    } else {
                        return GetAdjectiveForm(entry, form, scriptType);
                    }

                default:
                    switch (scriptType) {
                        case JapaneseScriptType.Kana:
                            return entry.Kana;
                        case JapaneseScriptType.Romaji:
                            return KanaConverter.Instance.ConvertToRomaji(entry.Kana);

                        default:
                            return entry.Kanji;
                    }
            }
        }

        public static PhraseSequenceElement[] GetForms(DictionaryDataEntry entry) {
            if (additionalForms.ContainsKey(entry.Kanji)) {
                var forms = new PhraseSequenceElement[additionalForms[entry.Kanji].Count];
                for (int i = 0; i < forms.Length; i++) {
                    forms[i] = new PhraseSequenceElement(entry.ID, i);
                }
                return forms;
            }

            switch (entry.GetPartOfSpeech()) {
                case PartOfSpeech.GodanVerb:
                case PartOfSpeech.IchidanVerb:
                case PartOfSpeech.Copula:
                case PartOfSpeech.SuruVerb:
                    return GetVerbForms(entry);

                case PartOfSpeech.Adjective:
                    return GetAdjectiveForms(entry);

                default:
                    return new PhraseSequenceElement[] { new PhraseSequenceElement(entry.ID, 0) };
            }
        }

        public static string GetAdjectiveForm(DictionaryDataEntry entry, int form, JapaneseScriptType scriptType) {
            if (form == AdjectiveI) {
                return entry.Kana.Substring(0, entry.Kana.Length - 1) + adjectiveEndings[AdjectiveI];
            }

            if (form == AdjectiveNa) {
                return entry.Kana + adjectiveEndings[AdjectiveNa];
            }

            return entry.Kana;
        }

        public static string GetVerbForm(DictionaryDataEntry entry, int form, JapaneseScriptType scriptType) {
            if (form == Want) {
                var s = GetVerbForm(entry, PolitePresentIndicative, scriptType);
                s = s.Substring(0, s.Length - 2);
                return s + verbEndings[Want];
            }

            var text = entry.Kana;
            switch (scriptType) {
                case JapaneseScriptType.Kanji:
                    text = entry.Kanji;
                    break;
            }

            switch (entry.GetPartOfSpeech()) {
                case PartOfSpeech.GodanVerb:
                    return ConjugateGroup1Verb(text, form);

                case PartOfSpeech.IchidanVerb:
                    return ConjugateGroup2Verb(text, form);

                case PartOfSpeech.Copula:
                case PartOfSpeech.SuruVerb:
                    return ConjugateIrregularVerb(text, form);
            }
            return null;
        }

        public static PhraseSequenceElement[] GetVerbForms(DictionaryDataEntry entry) {
            var forms = GetVerbForms();
            var arr = new PhraseSequenceElement[forms.Length];
            for (int i = 0; i < forms.Length; i++) {
                arr[i] = new PhraseSequenceElement(entry.ID, forms[i]);
            }
            return arr;
        }

        public static PhraseSequenceElement[] GetAdjectiveForms(DictionaryDataEntry entry) {
            var forms = GetAdjectiveForms();
            var arr = new PhraseSequenceElement[forms.Length];
            for (int i = 0; i < forms.Length; i++) {
                arr[i] = new PhraseSequenceElement(entry.ID, forms[i]);
            }
            return arr;
        }
    }
}