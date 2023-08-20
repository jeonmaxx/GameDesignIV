using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioClip[] clickSounds;
    public AudioClip[] openSounds;
    public AudioSource audioSource;

    public void ClickButton()
    {
        if (audioSource != null)
        {
            audioSource.clip = clickSounds[Random.Range(0, clickSounds.Length)];
            audioSource.Play();
        }
    }

    public void PlayOpenSound()
    {
        if(audioSource != null)
        {
            audioSource.clip = openSounds[Random.Range(0, openSounds.Length)];
            audioSource.Play();
        }
    }
}
