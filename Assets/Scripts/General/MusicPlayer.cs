using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource aud;
    private bool isPlaying;

    void Awake()
    {
        if (!isPlaying && GlobalData.musicStarted)
        {
            Destroy(gameObject);
        }

        if (!GlobalData.musicStarted)
        {
            DontDestroyOnLoad(transform.gameObject);
            aud = GetComponent<AudioSource>();
            PlayMusic();
            GlobalData.musicStarted = true;
            isPlaying = true;
        }
    }

    public void PlayMusic()
    {
        if (aud.isPlaying) return;
        aud.Play();
    }

    public void StopMusic()
    {
        aud.Stop();
    }

}
