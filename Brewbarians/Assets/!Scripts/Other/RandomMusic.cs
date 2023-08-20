using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomMusic : MonoBehaviour
{
    public AudioClip[] soundtracks;

    public void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = soundtracks[Random.Range(0, soundtracks.Length)];
        source.Play();
    }
}
