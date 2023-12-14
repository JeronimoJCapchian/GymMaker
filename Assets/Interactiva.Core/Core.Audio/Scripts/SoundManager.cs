using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Interactiva.Core.Audio {
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager singleton;
        [SerializeField] private AudioMixer audioMixer;
        private int attenuateLevel = 0;
        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            } else
            {
                Debug.LogWarning("SoundManager singleton already exists. Did you place more than one component in your scene?");
            }
        }
        public void SetAudioAttenuationLevel(bool attenuate)
        {
            if (attenuate)
            {
                attenuateLevel++;
            }
            else
            {
                attenuateLevel--;
            }
            SetAudioAttenuationLevel(attenuateLevel > 0 ? -30f : 0);
        }

        private void SetAudioAttenuationLevel(float level)
        {
            if (audioMixer == null)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(SetAttenuationLevel(level));
        }

        private IEnumerator SetAttenuationLevel(float level)
        {

            float value;
            
            while (audioMixer.GetFloat("AttenuateVolume", out value) && !Mathf.Approximately(value, level))
            {
                value = Mathf.MoveTowards(value, level, Time.deltaTime * 75f);
                audioMixer.SetFloat("AttenuateVolume", value);
                yield return null;
            }

        }
    }
}
