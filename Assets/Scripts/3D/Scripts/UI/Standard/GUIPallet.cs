using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPallet : MonoBehaviour {

    const string ResourcePath = "UI/GUIPallet";
    static GUIPallet _instance;
    public static GUIPallet Instance {
        get {
            if (!_instance) {
                _instance = GameObjectUtil.GetResourceInstance<GUIPallet>(ResourcePath);
            }
            return _instance;
        }
    }

	public Texture2D WhiteBackground { get; set; }

	public Font defaultFont;
	public Sprite lockedWordShape;
	public Sprite leftSpeechBubble;
	public Sprite rightSpeechBubble;
    public Sprite leftPhoneSpeechBubble;
    public Sprite rightPhoneSpeechBubble;
    public Sprite offscreenSpeechBubble;

    public Color importantMessageColor = Color.white;
    public Color lightGray = new Color(0.8f, 0.8f, 0.8f);
    public Color darkGray = new Color(0.2f, 0.2f, 0.2f);
    public Color inactiveColor = Color.white;
    public Color successColor = Color.white;
    public Color failureColor = Color.white;

    public Color defaultBackgroundColor = Color.gray;
    public Color defaultTextColor = Color.white;
    public Color lightTextColor = Color.white;

    public Color selfColor = Color.white;
    public Color otherColor = Color.white;

	public Color verbColor = Color.white;
	public Color nounColor = Color.white;
	public Color pronounColor = Color.white;
	public Color adverbColor = Color.white;
	public Color adjectiveColor = Color.white;
	public Color particleColor = Color.white;
	public Color questionWordColor = Color.white;
	public Color greetingColor = Color.white;

	public Color rumorPhraseColor = Color.white;
	public Color normalPhraseColor = Color.white;
	public Color contructedPhraseColor = Color.white;
	public Color inventoryBackgroundColor = Color.white;
	public Color constructorBackgroundColor = Color.white;
	public Color objectiveWordColor = Color.white;
	public Color confirmButtonColor = Color.white;
	public Color disabledButtonColor = Color.white;

	public Color[] levelColors;
	public Color[] stageColors;
	
	public Color[] rumorColors;
	public Texture2D textBubblePointer;

	int colorIndex = 0;
	Dictionary<string, Color> rumorColorDictionary = new Dictionary<string, Color>();

	Dictionary<int, GUIStyle> labelStyles = new Dictionary<int, GUIStyle>();

	// Use this for initialization
	void Awake () {
		WhiteBackground = new Texture2D (1, 1);
		WhiteBackground.SetPixel (0, 0, Color.white);
		WhiteBackground.Apply ();
	}

	public Color GetColorForRumor(string rumor){
		if(!rumorColorDictionary.ContainsKey(rumor)){
			rumorColorDictionary[rumor] = rumorColors[colorIndex];
			colorIndex++;
		}
		return rumorColorDictionary [rumor];
	}

	public Color GetColorForWordCategory(PhraseCategory category){
		switch (category) {
		case PhraseCategory.Verb:
			return verbColor;
		case PhraseCategory.Noun:
			return nounColor;
		case PhraseCategory.Pronoun:
			return pronounColor;
		case PhraseCategory.Adjective:
			return adjectiveColor;
		case PhraseCategory.Adverb:
			return adverbColor;
		case PhraseCategory.Particle:
			return particleColor;
		case PhraseCategory.Greeting:
			return greetingColor;
		case PhraseCategory.Question:
			return questionWordColor;
		default:
			return Color.white;
		}
	}

	public PhraseCategory[] GetColoredCategories(){
		return new PhraseCategory[] {
			PhraseCategory.Greeting,
			PhraseCategory.Verb,
			PhraseCategory.Noun,
			PhraseCategory.Pronoun,
			PhraseCategory.Particle,
			PhraseCategory.Question,
			PhraseCategory.Adjective,
			PhraseCategory.Adverb
		};
	}

}
