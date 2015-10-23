using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseAudioResources : ScriptableObject {

    public List<AudioResource> audioResources = new List<AudioResource>();

    public AudioClip GetAudioResource(int id) {
        var r = (from a in audioResources where a.id == id select a).FirstOrDefault();
        if (r == null) {
            return null;
        }
        return r.audioClip;
    }

    public int GetAudioResourceID(AudioClip clip) {
		if (clip == null) {
			return -1;
		}

        var r = (from a in audioResources where a.audioClip == clip select a).FirstOrDefault();
        if (r == null) {
            r = new AudioResource(GetNextID(), clip);
            audioResources.Add(r);
        }
        return r.id;
    }

    public int GetNextID() {
        var l = new HashSet<int>(from a in audioResources select a.id);
        int id = 0;
        while (l.Contains(id)) {
            id++;
        }
        return id;
    }

}
