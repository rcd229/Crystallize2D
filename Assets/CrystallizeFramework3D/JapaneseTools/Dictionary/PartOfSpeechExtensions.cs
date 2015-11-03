using UnityEngine;
using System.Collections;

public static class PartOfSpeechExtensions{

	public static PhraseCategory GetCategory (this PartOfSpeech pos){
		switch (pos) {
		case PartOfSpeech.Adjective:
			return PhraseCategory.Adjective;

		case PartOfSpeech.Adverb:
			return PhraseCategory.Adverb;

		case PartOfSpeech.Conjunction:
		
		
		case PartOfSpeech.Copula:
		case PartOfSpeech.GodanVerb:
		case PartOfSpeech.IchidanVerb:
		case PartOfSpeech.SuruVerb:
			return PhraseCategory.Verb;

		case PartOfSpeech.Counter:
			return PhraseCategory.Particle;

		case PartOfSpeech.Expression:
		case PartOfSpeech.Interjection:
			return PhraseCategory.Greeting;

		case PartOfSpeech.Noun:
		case PartOfSpeech.Numeric:
			return PhraseCategory.Noun;
		
		case PartOfSpeech.Particle:
			return PhraseCategory.Particle;

		case PartOfSpeech.Pronoun:
			return PhraseCategory.Pronoun;

		default:
			return PhraseCategory.Unknown;
		}
	}

}
