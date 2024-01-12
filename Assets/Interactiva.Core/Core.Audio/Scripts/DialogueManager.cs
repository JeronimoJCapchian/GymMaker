using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Interactiva.Core.UI;
using Interactiva.Core.Utilities;
using TMPro;

namespace Interactiva.Core.Audio
{

    public class DialogueManager : MonoBehaviour, IPauseEventReceiver
    {
        /// <summary>
        /// Override replaces current dialogue, Queue waits until current dialogue is finished than starts, cancel cancels the dialogue
        /// </summary>
        public enum QueueType { Override, Queue, Cancel }
        /// <summary>
        /// Base dialogue provides basic methods for all voice play types.
        /// </summary>
        [System.Serializable]
        public abstract class BaseDialogue
        {
            /// <summary>
            /// The dialogue key of this Dialogue. A Dialogue Key is a unique string identifier which you can use
            /// to play dialogues by executing the PlayDialogue method.
            /// </summary>
            [field: SerializeField]
            public virtual string DialogueKey { get; set; } = "";
            /// <summary>
            /// Returns whether this dialogue is paused
            /// </summary>
            public virtual bool Paused { get; private set; }
            /// <summary>
            /// Pause status *****Not implemented
            /// </summary>
            public virtual void PauseToggle(bool toggle)
            {

            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="startDelay"></param>
            /// <param name="src"></param>
            public abstract void PlayDialogue();


            public abstract bool IsDialogueValid();
        }
        /// <summary>
        /// A Dialogue is composed of multiple Voice Lines, which are phrases spoken.
        /// </summary>
        [System.Serializable]
        public class VoiceLine
        {
            /// <summary>
            /// Time in seconds to wait until starting line.
            /// </summary>
            public float delay = 0.5f;
            /// <summary>
            /// String identifying the unique subtitle for this line.
            /// </summary>
            [TextArea]
            public string subtitleText;
            
            /// <summary>
            /// The clip to play (voice)
            /// </summary>
            [Tooltip("The audio clip representing this voice line.")]
            public AudioClip voice;
            /// <summary>
            /// The audio source of this line.
            /// </summary>
            [Tooltip("The audio source to use for this voice line. You should use a single audio source for each character.")]
            public AudioSource source;
            /// <summary>
            /// Time in seconds until line is finished.
            /// </summary>
            [Tooltip("Time in seconds before after the voice clip has finished as padding.")]
            public float padding = 0.5f;
        }

        /// <summary>
        /// Dialogue is the class for general dialogue exchanges.
        /// (e.g. multiple voice lines in sucession)
        /// </summary>
        [System.Serializable]
        public class Dialogue : BaseDialogue
        {
            public QueueType queueType = QueueType.Queue;
            public VoiceLine[] voiceLines;
            public UnityEvent onFinish;

            /// <summary>
            /// Are we currently paused?
            /// </summary>
            private bool paused = false;
            /// <summary>
            /// If we are paused, what voice line clip index are we in?
            /// </summary>
            private int pauseClipIndex = 0;
            /// <summary>
            /// If we are paused, what time are we currently at?
            /// </summary>
            private float pauseTime = 0;
            /// <summary>
            /// If we are paused, at what level are we (0 = delay, 1 = playing voice, 2 = padding)
            /// </summary>
            private int pauseLevel = 0;

            public override bool Paused => paused;


            /// <summary>
            /// Returns true if any of the AudioSources are already playing.
            /// (e.g. if one of the characters in this dialogue is already reciting another dialogue)
            /// Used to handle conflicting dialogues.
            /// </summary>
            /// <returns>True if any of the audio sources for the character are playing, otherwise false</returns>
            public bool IsVoiceSourceRunning
            {
                get
                {
                    for (int i = 0; i < voiceLines.Length; i++)
                    {
                        if (voiceLines[i].source.isPlaying)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            /// <summary>
            /// Returns if the current Dialogue is running.
            /// </summary>
            public bool IsRunning { get; private set; } = false;

            /// <summary>
            /// Returns all the audio sources (voice sources) associated with this Dialogue.
            /// </summary>
            public AudioSource[] GetAudioSources 
            { 
                get
                {
                    List<AudioSource> sources = new List<AudioSource>();
                    for (int i = 0; i < voiceLines.Length; i++)
                    {
                        if (!sources.Contains(voiceLines[i].source))
                        {
                            sources.Add(voiceLines[i].source);
                        }
                    }
                    return sources.ToArray();
                } 
            }

            public override void PlayDialogue()
            {
                if (IsDialogueValid())
                {
                    if (queueType == QueueType.Override)
                    {
                        singleton.StartCoroutine(StartDialogueRoutine());
                    }
                    else if (queueType == QueueType.Queue)
                    {
                        if (IsVoiceSourceRunning)
                        {
                            singleton.dialogueQueue.Add(this);
                        }
                        else
                        {
                            singleton.StartCoroutine(StartDialogueRoutine());
                        }
                    }
                    else
                    {
                        if (IsVoiceSourceRunning)
                        {
                            return;
                        }
                        else
                        {
                            singleton.StartCoroutine(StartDialogueRoutine());
                        }
                    }
                }
            }

            public override void PauseToggle(bool toggle)
            {
                base.PauseToggle(toggle);
                paused = toggle;
                if (toggle)
                {
                    StopDialogue(false);
                } else
                {
                    singleton.StartCoroutine(StartDialogueRoutine());
                }
            }

            /// <summary>
            /// Stops the dialogue if it is playing. Mostly used for debugging purposes.
            /// </summary>
            /// <param name="executeOnFinish">Whether to execute the onFinish event.</param>
            public void StopDialogue(bool executeOnFinish)
            {
                singleton.StopAllCoroutines();
                if (executeOnFinish)
                {
                    onFinish.Invoke();
                }
                if (singleton.subtitleLabel != null)
                {
                    singleton.subtitleLabel.Get().text = "";
                }
                IsRunning = false;
                AudioSource[] sources = GetAudioSources;
                for (int i = 0; i < sources.Length; i++)
                {
                    sources[i].clip = null;
                    sources[i].Stop();
                    
                }

            }

            private IEnumerator StartDialogueRoutine()
            {
                IsRunning = true;
                for (int i = pauseClipIndex; i < voiceLines.Length; i++)
                {
                    pauseClipIndex = i;
                    VoiceLine m = voiceLines[i];
                    if (pauseLevel == 0)
                    {
                        while (pauseTime < m.delay)
                        {
                            pauseTime += Time.deltaTime;
                            yield return null;
                        }
                        pauseLevel = 1;
                        pauseTime = 0;
                    }
                    if (pauseLevel == 1)
                    {
                        if (singleton.subtitleLabel != null)
                        {
                            singleton.subtitleLabel.Get().text = m.source.name + ": " + m.subtitleText;
                        }
                        m.source.clip = m.voice;
                        m.source.time = pauseTime;
                        m.source.Play();
                        while (pauseTime < m.voice.length)
                        {
                            pauseTime += Time.deltaTime;
                            yield return null;
                        }
                        pauseLevel = 2;
                        pauseTime = 0;
                    }
                    if (pauseLevel == 2)
                    {
                        if (singleton.subtitleLabel != null)
                        {
                            singleton.subtitleLabel.Get().text = "";
                        }
                        while (pauseTime < m.padding)
                        {
                            pauseTime += Time.deltaTime;
                            yield return null;
                        }
                        pauseTime = 0;
                        pauseLevel = 0;
                    }
                }
                pauseClipIndex = 0;
                onFinish.Invoke();
                IsRunning = false;
            }
            /// <summary>
            /// Making sure that all AudioSources are assigned.
            /// </summary>
            /// <returns>True if all audio sources for a dialogue are assigned.</returns>
            public override bool IsDialogueValid()
            {
                for (int i = 0; i < voiceLines.Length; i++)
                {
                    if (voiceLines[i].source == null)
                    {
                        return false;
                    }
                }
                return true;
            }
            

            
        }
        /// <summary>
        /// DialoguePool is a class used to trigger voicelines that can be repeated or replaced with other voice lines.
        /// E.g. if a character 
        /// </summary>
        [System.Serializable]
        public class DialoguePool : BaseDialogue
        {
            [System.Serializable]
            public class VoiceLinePool
            {
                public AudioClip voice;
                public int probability = 1;
                /// <summary>
                /// In seconds, how much time until this voice line can be said again
                /// </summary>
                public float repeatInterval = 20f;
            }

            public AudioSource src;
            public List<VoiceLinePool> activeVoiceLines;

            public override void PlayDialogue()
            {
                if (!src.isPlaying && activeVoiceLines.Count > 0)
                {
                    int index = CalculateVoice();
                    src.clip = activeVoiceLines[index].voice;
                    src.Play();
                    if (activeVoiceLines[index].repeatInterval > 0)
                    {
                        singleton.StartCoroutine(RepeatVoiceLineRoutine(index, activeVoiceLines[index].repeatInterval));
                    }
                }
            }

            public IEnumerator RepeatVoiceLineRoutine(int index, float repeatInterval)
            {
                VoiceLinePool disabledVoiceLine = activeVoiceLines[index];
                activeVoiceLines.RemoveAt(index);
                yield return new WaitForSeconds(repeatInterval);
                activeVoiceLines.Add(disabledVoiceLine);
            }

            public int CalculateVoice()
            {
                int count = 0;
                for (int i = 0; i < activeVoiceLines.Count; i++)
                {
                    count += activeVoiceLines[i].probability;
                }
                int calculatedRandom = Random.Range(0, count);
                count = 0;
                for (int i = 0; i < activeVoiceLines.Count; i++)
                {
                    count += activeVoiceLines[i].probability;
                    if (count >= calculatedRandom)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public override bool IsDialogueValid()
            {
                return true;
            }
        }
        public static DialogueManager singleton;
        [SerializeField] private CrossPlatformObject<TextMeshProUGUI> subtitleLabel;
        [SerializeField] private Dialogue[] dialoguesUI;
        [SerializeField] private DialoguePool[] dialoguePoolsUI;
        [SerializeField] private Dictionary<string, BaseDialogue> intDialogues = new Dictionary<string, BaseDialogue>();
        private List<Dialogue> dialogueQueue = new List<Dialogue>();

        public bool IsRunningAny
        {
            get
            {
                foreach (Dialogue d in dialoguesUI)
                {
                    if (d.IsRunning)
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        private void Awake()
        {
            singleton = this;
            foreach (Dialogue d in dialoguesUI)
            {
                intDialogues.Add(d.DialogueKey, d);
            }
            foreach (DialoguePool d in dialoguePoolsUI)
            {
                intDialogues.Add(d.DialogueKey, d);
            }
            
        }

        private float queueTimer = 0;
        private void Update()
        {
            if (dialogueQueue.Count > 0)
            {
                queueTimer += Time.deltaTime;
                if (queueTimer >= 1f)
                {
                    queueTimer = 0;
                    if (dialogueQueue[0].IsDialogueValid())
                    {
                        if (!dialogueQueue[0].IsVoiceSourceRunning)
                        {
                            dialogueQueue[0].PlayDialogue();
                            dialogueQueue.RemoveAt(0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method stops all dialogues currently running, and executes their onFinish events.
        /// 
        /// Useful for debugging purposes.
        /// </summary>
        public void SkipAllDialogues()
        {
            foreach (Dialogue d in dialoguesUI)
            {
                if (d.IsRunning)
                {
                    d.StopDialogue(true);
                }
            }
        }

        public void StartDialogue(string key)
        {
            try
            {
                intDialogues[key].PlayDialogue();
                Debug.Log("Playing dialogue: " + key);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Couldn't execute dialogue: " + e);
            }
        }

        public static void StartDialogue(string key, AudioSource src = null)
        {
            try
            {
                singleton.intDialogues[key].PlayDialogue();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Couldn't execute dialogue: " + e);
            }
        }

        public void OnPauseChange(bool state)
        {
            foreach (Dialogue d in dialoguesUI)
            {
                if (state)
                {
                    if (d.IsRunning)
                    {
                        d.PauseToggle(true);
                    }
                } else
                {
                    if (d.Paused)
                    {
                        d.PauseToggle(false);
                    }
                }
            }
        }
    }
}