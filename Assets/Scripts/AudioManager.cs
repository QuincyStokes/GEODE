using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    private Dictionary<string, AudioSource> soundDictionary;
    private Dictionary<string, AudioClip> audioClipCache;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        soundDictionary = new Dictionary<string, AudioSource>();
        audioClipCache = new Dictionary<string, AudioClip>();

        Play("defaultbackgroundmusic1",.3f, true);
    }

    private AudioClip LoadAudioClip(string name) {
        //if the clip doesn't exist in the cache
        if (!audioClipCache.TryGetValue(name, out AudioClip clip)) {
            //load the clip from the resources folder
            clip = Resources.Load<AudioClip>($"Sounds/{name}");
            //if the clip is a valid file, add it to the cache 
            if (clip != null) {
                audioClipCache.Add(name, clip);
            }
        }
        //return the clip, if it isn't loaded it will be null
        return clip;
    }


    public void Play(string name, float volume=1f, bool loop = false) {
        //if the sound doesnt exist in the dictionary
        if (!soundDictionary.TryGetValue(name, out AudioSource source)) {
            //call our loadaudioclip function to load the clip
            AudioClip clip = LoadAudioClip(name);
            //if the clip is null, print not found and return
            if (clip == null) {
                print(name + "not found");
                return;
            } 
            //if the clip isn't null, create an AudioSource and set it's clip to the loaded clip
            source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            //add the newly created Sound (<name, source>)
            soundDictionary.Add(name, source);
        }
        //no matter what, set it's attributes to the passed attributes and play it
        source.volume = volume;
        source.loop = loop;
        source.Play();
    }

    public void PlayAtPosition(string name, Vector3 position, float volume=1f, bool loop = false) {
        //if the sound doesnt exist in the dictionary
        if (!soundDictionary.TryGetValue(name, out AudioSource source)) {
            //call our loadaudioclip function to load the clip
            AudioClip clip = LoadAudioClip(name);
            //if the clip is null, print not found and return
            if (clip == null) {
                print(name + "not found");
                return;
            } 
            //if the clip isn't null, create an AudioSource and set it's clip to the loaded clip
            source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            //add the newly created Sound (<name, source>)
            soundDictionary.Add(name, source);
        }
        //no matter what, set it's attributes to the passed attributes and play it
        source.volume = volume;
        source.loop = loop;
        AudioSource.PlayClipAtPoint(source.clip, position);
    }

    public void Stop(string name) {
        if (soundDictionary.TryGetValue(name, out AudioSource source)) {
            source.Stop();
        } else {
            print(name + "not found. Cannot stop.");
        }
    }

    
}