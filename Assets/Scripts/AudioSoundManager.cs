using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoundManager : MonoBehaviour
{
    public static AudioSoundManager Instance { get; private set; }

    private AudioSource audioS;
    public AudioClip[] clips;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        audioS = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        audioS.clip = clips[index];
        audioS.Play();
    }
}
