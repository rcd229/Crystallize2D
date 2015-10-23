using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HobbyPhraseResources : DefaultPhrasePipelineBuilder, IPhraseResources {

    public static HobbyPhraseResources Instance { get { return new HobbyPhraseResources(); } }

    public const string Singing = "singing";
    public const string Cooking = "cooking";
    public const string Reading = "reading";
    public const string Running = "running";
    public const string WatchingMovies = "watching movies";
    public const string PlayingGames = "playing games";
    public const string Traveling = "traveling";
    public const string MeetingFriends = "meeting friends";

    public static PhraseSequence LikeSinging { get { return GetPhrase("like singing"); } }
    public static PhraseSequence LikeCooking { get { return GetPhrase("like cooking"); } }
    public static PhraseSequence LikeReading { get { return GetPhrase("like reading"); } }
    public static PhraseSequence LikeRunning { get { return GetPhrase("like running"); } }
    public static PhraseSequence LikeWatchingMovies { get { return GetPhrase("like watching movies"); } }
    public static PhraseSequence LikePlayingGames { get { return GetPhrase("like playing games"); } }
    public static PhraseSequence LikeTraveling { get { return GetPhrase("like traveling"); } }
    public static PhraseSequence LikeMeetingFriends { get { return GetPhrase("like meeting friends"); } }

    public static PhraseSequence ILikeHobby { get { return GetPhrase("I like [doinghobby]"); } }

    static Dictionary<string, PhraseSequence> hobbyPhrases = new Dictionary<string, PhraseSequence>();
    static HobbyPhraseResources() {
        hobbyPhrases[Singing] = LikeSinging;
        hobbyPhrases[Cooking] = LikeCooking;
        hobbyPhrases[Reading] = LikeReading;
        hobbyPhrases[Running] = LikeRunning;
        hobbyPhrases[WatchingMovies] = LikeWatchingMovies;
        hobbyPhrases[PlayingGames] = LikePlayingGames;
        hobbyPhrases[Traveling] = LikeTraveling;
        hobbyPhrases[MeetingFriends] = LikeMeetingFriends;
    }

    public static PhraseSequence GetPhraseForHobby(string hobbyKey) {
        if (hobbyPhrases.ContainsKey(hobbyKey)) {
            return hobbyPhrases[hobbyKey];
        } else {
            Debug.LogError(hobbyKey + " is not a hobby");
            return null;
        }
    }

    public static PhraseSequence GetLikePhraseForHobby(string hobbyKey) {
        if (hobbyPhrases.ContainsKey(hobbyKey)) {
            var c = new ContextData();
            c.Set("doinghobby", hobbyPhrases[hobbyKey]);
            return ILikeHobby.InsertContext(c);
        } else {
            Debug.LogError(hobbyKey + " is not a hobby");
            return null;
        }
    }

    public static IEnumerable<PhraseSequence> GetHobbies() {
        return GetHobbyKeys().Select(k => GetPhrase(k));
    }

    public static IEnumerable<string> GetHobbyKeys() {
        return new string[]{
            Singing, Cooking, Reading, Running, 
            WatchingMovies, PlayingGames, Traveling, MeetingFriends
        };
    }

    public string SetKey { get { return GetType().ToString(); } }
    public IEnumerable<string> GetPhraseKeys() { return GetHobbyKeys(); }

}
