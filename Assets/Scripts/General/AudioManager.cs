using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public static AudioManager instance;


    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }        
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound i in Sounds)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.Clip;

            i.source.volume = i.vol;
            i.source.pitch = i.pit;
            i.source.loop = i.loop;
        }
    }

    public void Play(string name)
    {
        Sound i = Array.Find(Sounds, Sound => Sound.name == name);
        if (i == null)
        {
            Debug.LogWarning("Sound / SFX" + name + " Missing!");
            return;
        }

        i.source.pitch = i.pit + Random.Range(-0.1f, 0.1f);
        i.source.Play();
    }

    public void Stop(string name)
    {
        Sound i = Array.Find(Sounds, Sound => Sound.name == name);
        Debug.Log("Tracking lost, stopping audio");
        if (i.source.isPlaying)
        {
            i.source.Stop();
        }
    }

}

