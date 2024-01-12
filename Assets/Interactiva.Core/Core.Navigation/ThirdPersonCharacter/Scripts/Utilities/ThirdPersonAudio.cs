using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Navigation.ThirdPerson
{
    [RequireComponent(typeof(AudioSource))]
    public class ThirdPersonAudio : MonoBehaviour
    {
        [SerializeField] AudioClip runningSound;
        [SerializeField] AudioClip walkingSound;
        [SerializeField] AudioClip idleSound;
        private ThirdPersonController tpc;
        private AudioSource audioSource;

        private void Start()
        {
            tpc = GetComponentInParent<ThirdPersonController>();
            audioSource = GetComponent<AudioSource>();
        }
        void Update()
        {
            
            if (tpc.Speed > 0)
            {
                if (tpc.Sprinting)
                {
                    audioSource.clip = runningSound;
                }
                else
                {
                    audioSource.clip = walkingSound;
                }
            } else
            {
                audioSource.clip = idleSound;
            }
            if (audioSource.clip != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }
}