using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine.Events;


namespace Interactiva.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioCrossFade : MonoBehaviour
    {
        private AudioSource audioSource;
        private float defaultVolume;

        Tweener curTween = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            defaultVolume = audioSource.volume;
        }

        public void CrossFadeIn(float duration)
        {
            if (curTween != null)
            {
                audioSource.Pause();
                curTween.Kill();
            }
            audioSource.Play();
            curTween = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, defaultVolume, duration).OnComplete(() =>
            {
                curTween = null;
            });
            
        }

        public void CrossFadeOut(float duration)
        {
            if (curTween != null)
            {
                curTween.Kill();
            }
            curTween = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, duration).OnComplete(() =>
            {
                audioSource.Pause();
                curTween = null;
            });
            
        }

    }
}