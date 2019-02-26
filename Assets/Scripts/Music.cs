using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : Singleton<Music> {

	[SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip endGameMusic;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        audioSource.UnPause();
    }

    public void PlayMenuMusic()
    {
        //audioSource.Stop();
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void PlayGameMusic()
    {
        //audioSource.Stop();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    public void PlayEndGameMusic()
    {
        audioSource.clip = endGameMusic;
        audioSource.Play();
    }
}
