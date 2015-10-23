using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static AudioManager main { get; set; }

	public AudioClip wordDropSuccess;
	public AudioClip wordDropFailure;
	public AudioClip phraseSuccess;
	public AudioClip dialogueSuccess;
	public AudioClip dialogueFailure;
	public AudioClip message;
	public AudioClip rankUp;
	public AudioClip levelUp;

	public AudioClip backgroundMusic;

	GameObject musicPlayer;

	// Use this for initialization
	void Awake () {
		main = this;
	}

	void Start(){
		if (!GetComponent<AudioSource>()) {
			gameObject.AddComponent<AudioSource>();
		}

        //if (LevelSettings.main) {
        //    if(LevelSettings.main.disableMusic){
        //        return;
        //    }
        //}

		if (backgroundMusic) {
			musicPlayer = new GameObject("Music");
			musicPlayer.AddComponent<AudioSource>();
			musicPlayer.GetComponent<AudioSource>().clip = backgroundMusic;
			musicPlayer.GetComponent<AudioSource>().loop = true;
			musicPlayer.GetComponent<AudioSource>().Play();
		}
	}
	
	public void PlayWordSuccess(){
		PlaySound (wordDropSuccess);
	}

	public void PlayWordFailure(){
		PlaySound (wordDropFailure);
	}

	public void PlayPhraseSuccess(){
		PlaySound (phraseSuccess);
	}

	public void PlayDialogueSuccess(){
		PlaySound (dialogueSuccess);
	}

	public void PlayDialogueFailure(){
		PlaySound (dialogueFailure);
	}

	public void PlayMessage(){
		PlaySound (message);
	}

	public void PlayRankUp(){
		PlaySound (rankUp);
	}

	public void PlayLevelUp(){
		PlaySound (levelUp);
	}

	void PlaySound(AudioClip clip){
		var audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.clip = clip;
		audioSource.Play ();
		Destroy (audioSource, clip.length);
		/*audio.Stop ();
		audio.clip = clip;
		audio.Play ();*/
	}

}
