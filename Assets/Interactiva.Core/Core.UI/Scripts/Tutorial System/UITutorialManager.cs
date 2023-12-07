using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Doozy.Engine.UI;
using Interactiva.Core.UI;
using Interactiva.Core.Utilities;

namespace Interactiva.Core.UI {
    public class UITutorialManager : MonoBehaviour
    {
        [Header("On Start")]

        [Tooltip("OnStartAny is executed when any tutorial is started. Good for feedback.")]
        [SerializeField] private UnityEvent onStartAny;

        [SerializeField] private TutorialWrapper.Tutorial[] initialTutorial;
        [SerializeField] private TutorialWrapper[] tutorials;

        [Header("On Finish")]
        [Tooltip("OnFinishAny is triggered when any tutorial is finished. Good for feedback.")]
        [SerializeField] private UnityEvent onFinishAny; 

        /// <summary>
        /// The popup name is used by the Doozy Control Panel to figure out what popup to show.
        /// To access the Doozy Control Panel Popup menu go to Tools > Doozy > Control Panel. Then click on Popups on the left.
        /// It is a cross platform object since it is convention to use two different popups (and locations) on different platforms.
        /// 
        /// Currently being initialized with the default Desktop and Mobile popup presets from Doozy.
        /// </summary>
        [SerializeField] private CrossPlatformObject<string> popupName = new CrossPlatformObject<string>("Tutorial - Popup", "MobileTutorial - Popup");

        private Queue<TutorialWrapper.Tutorial> tutorialQueue = new Queue<TutorialWrapper.Tutorial>();
        private bool running = false;

        private void Start()
        {
            for (int i = 0; i < initialTutorial.Length; i++)
            {
                tutorialQueue.Enqueue(initialTutorial[i]);
            }
            ///Setting up listeners for the onStartAny and onFinishAny
            for (int i = 0; i < tutorials.Length; i++)
            {
                for (int j = 0; j < tutorials[i].tutorials.Length; j++)
                {
                    if (j == 0)
                    {
                        tutorials[i].tutorials[j].onStart.AddListener(() =>
                        {
                            onStartAny.Invoke();
                        });
                    }
                    if (j == tutorials[i].tutorials.Length - 1)
                    {
                        tutorials[i].tutorials[j].onFinish.AddListener(() =>
                        {
                            onFinishAny.Invoke();
                        });
                    }
                }
            }
        }

        private void Update()
        {
            if (tutorialQueue.Count > 0)
            {
                if (!running) {
                    StartCoroutine(StartTutorial(tutorialQueue.Dequeue()));
                }
            }
        }

        /// <summary>
        /// Internal IEnumerator that is executed for each individual tutorial.
        /// </summary>
        /// <param name="t">The tutorial to run</param>
        /// <returns></returns>
        private IEnumerator StartTutorial(TutorialWrapper.Tutorial t)
        {
            running = true;
            onStartAny.Invoke();
            yield return new WaitForSeconds(t.delay);
            UIPopup tPopup = UIPopupManager.GetPopup(popupName.Get());
            
            tPopup.Data.SetLabelsTexts(t.tutorial.Title, t.tutorial.Description);
            tPopup.Data.SetImagesSprites(t.tutorial.Icon);
            UIPopupManager.ShowPopup(tPopup, false, false);
            if (!t.tutorial.showOverlay.Get())
            {
                yield return null;
                tPopup.Overlay.RectTransform.gameObject.SetActive(false);
            }
            t.tutorial.Setup();
            while (!t.tutorial.Poll())
            {
                yield return null;
            }
            tPopup.HideBehavior.OnFinished.Event.AddListener(() =>
            {
                t.onFinish.Invoke();
                t.tutorial.Finish();
                running = false;
            });
            tPopup.Hide();
        }


        public void RunTutorial(int index)
        {
            TutorialWrapper t = tutorials[index];
            if (t.save && t.tutorials.Length > 0)
            {
                if (PlayerPrefs.HasKey(t.tutorials[0].tutorial.Title))
                {
                    t.onStartTutorialAlreadyRun.Invoke();
                    return;
                }
                PlayerPrefs.SetInt(t.tutorials[0].tutorial.Title, 1);
            }
            if (!t.save)
            {

            }
            for (int i = 0; i < t.tutorials.Length; i++)
            {
                tutorialQueue.Enqueue(t.tutorials[i]);
            }
        }
    } 

    [System.Serializable]
    public class TutorialWrapper
    {
        [System.Serializable]
        public class Tutorial
        {
            [Tooltip("Reference to the Tutorial Prompt asset. To create a Tutorial, right click Project > Interactiva > UI > Create Tutorial")]
            public UIPrompt tutorial;

            [SerializeField]
            ///Event is triggered when tutorial starts.
            [Tooltip("Event is executed when this tutorial is started.")]
            public UnityEvent onStart;
            /// <summary>
            /// Event executed when this tutorial is finished.
            /// </summary>
            [Tooltip("Event executed when this tutorial prompt is finished.")]
            public UnityEvent onFinish;
            
            /// <summary>
            /// Delay applied before the tutorial is displayed, in seconds.
            /// </summary>
            [Tooltip("Time in seconds before tutorial is displayed.")]
            public float delay = 1f;
            
        }

        /// <summary>
        /// Determines whether we save the status of this tutorial being executed in player prefs.
        /// If it is saved, this tutorial is only executed once, and never again.
        /// </summary>
        public bool save = false;
        public Tutorial[] tutorials;

        /// <summary>
        /// Event is executed when save = true, and the tutorial has already been run before.
        /// Used if you need to execute something when tutorial is running, and we disable further runs.
        /// </summary>
        public UnityEvent onStartTutorialAlreadyRun;
        

    }
}

