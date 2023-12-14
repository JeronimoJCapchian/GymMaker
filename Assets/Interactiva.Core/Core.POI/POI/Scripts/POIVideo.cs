using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using Interactiva.Core.UI.Utilities;

namespace Interactiva.Core.POIs
{
    public class POIVideo : POI, IPOIToggle
    {
        public string URL;
        [SerializeField] private Slider progressBar;
        [SerializeField] private UIFader loadingIcon;
        /// <summary>
        /// If assigned, the material will be dark when paused, and otherwise white to show video.
        /// </summary>
        [SerializeField] private Material screenMat;
        [SerializeField] private Renderer screenRenderer;
        [SerializeField] private UnityEvent onPlay;
        [SerializeField] private UnityEvent onPause;

        private VideoPlayer videoPlayer;

        [SerializeField] private bool hideNavigatorOnToggle = false;
        [SerializeField] private string _PromptTextVar = "play video";
        [SerializeField] private string _AltPromptTextVar = "pause video";
        [SerializeField] private bool _HidePromptOnInteract = false;
        [SerializeField] private bool _AttenuateAudio = false;

        /// <summary>
        /// Time in seconds to destroy the video player component when its not being used.
        /// </summary>
        private const float timeToDestroyVideo = 90f;
        private float timerToDestroy = 0;

        private bool isPlaying = false;
        /// <summary>
        /// Last video player time. Used to resume when destroying component.
        /// </summary>
        private double lastTime = 0;

        /// <summary>
        /// Reference to player
        /// </summary>
        private static GameObject player;

        public override bool HidePromptOnInteract => _HidePromptOnInteract;
        public override string PromptText => _PromptTextVar;
        public override bool CanInteract => true;

        /// <summary>
        /// IPOIToggle properties.
        /// </summary>
        public bool HideNavigator => hideNavigatorOnToggle;
        public bool ToggleValue => isPlaying;
        public bool FreezeNavigator => false;
        public bool AttenuateAudio => _AttenuateAudio;
        public string AltPromptText => _AltPromptTextVar;


        

        protected override void Awake()
        {
            base.Awake();
            if (loadingIcon != null)
            {
                loadingIcon.FadeOut(0.5f);
            }
            screenRenderer.material = Instantiate(screenMat);
            if (progressBar == null)
            {
                progressBar = GetComponentInChildren<Slider>();
            }
        }
        /// <summary>
        /// Method creates a new video player component.
        /// We execute this when playback is needed.
        /// We then try to destroy the video player while the user isn't watching, since we don't want to waste memory.
        /// </summary>
        private void CreateVideoPlayer()
        {
            if (videoPlayer != null)
            {
                return;
            }
            (videoPlayer = gameObject.AddComponent<VideoPlayer>()).playOnAwake = false;
            videoPlayer.skipOnDrop = true;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = URL;
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = screenRenderer;
            videoPlayer.targetMaterialProperty = "_BaseMap";
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += new VideoPlayer.EventHandler((videoPlayer) =>
            {

                videoPlayer.time = lastTime;
                UpdateProgress();
                if (progressBar != null)
                {
                    progressBar.maxValue = (float)videoPlayer.length;
                }
            });
            videoPlayer.loopPointReached += new VideoPlayer.EventHandler((videoPlayer) =>
            {
                if (ToggleValue)
                {
                    OnToggleFalse();
                    player.SendMessage("ChangeToggleState", this, SendMessageOptions.DontRequireReceiver);
                    screenRenderer.material.color = Color.black;
                }
            });
        }

        private void DestroyVideoPlayer()
        {
            lastTime = videoPlayer.time;
            Destroy(videoPlayer);
            screenRenderer.material.color = Color.black;
        }

        private void Update()
        {
            //Update video player progress bar
            if (ToggleValue)
            {
                if (progressBar != null)
                {
                    UpdateProgress();
                }
            } else
            {
                if (videoPlayer != null)
                {
                    timerToDestroy += Time.deltaTime;
                    if (timerToDestroy >= timeToDestroyVideo)
                    {
                        DestroyVideoPlayer();
                        timerToDestroy = 0;
                    }
                }
            }
        }
        

        public override void Interact()
        {
            base.Interact();
            if (ToggleValue)
            {
                OnToggleFalse();
            } else
            {
                OnToggleTrue();
            }
        }

        

        public void OnToggleTrue()
        {
            CreateVideoPlayer();
            isPlaying = true;
            timerToDestroy = 0;
            StartCoroutine(LoadVideo());
        }

        private IEnumerator LoadVideo()
        {
            while (!videoPlayer.isPrepared)
            {
                if (!loadingIcon.Visible)
                {
                    loadingIcon.FadeIn(0.5f);
                }
                loadingIcon.transform.Rotate(Vector3.forward, -50f * Time.deltaTime);
                yield return null;
            }
            if (loadingIcon.Visible)
            {
                loadingIcon.FadeOut(0.5f);
            }
            if (screenRenderer != null)
            {
                screenRenderer.material.color = Color.white;
            }
            onPlay.Invoke();
            videoPlayer.Play();
        }

        public void OnToggleFalse()
        {
            isPlaying = false;
            videoPlayer.Pause();
            onPause.Invoke();
            StopAllCoroutines();
            if (screenRenderer != null)
            {
                //screenRenderer.material.color = Color.black;
            }
        }
        public override void EnableHighlight()
        {

            if (canHighlight)
            {
                
                if (!olc.GetOrAddLayer(0).Contains(gameObject))
                {
                    olc.GetOrAddLayer(0).Add(gameObject);
                }
            }
        }

        public override void DisableHighlight()
        {
            if (canHighlight)
            {
                olc.GetOrAddLayer(0).Remove(gameObject);   
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (player == null)
                {
                    player = other.gameObject;
                }
                POIManager.AddFocusablePOI(this);
                CreateVideoPlayer();
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                POIManager.RemoveFocusablePOI(this);
                if (ToggleValue)
                {
                    OnToggleFalse();
                    other.SendMessage("ChangeToggleState", this, SendMessageOptions.DontRequireReceiver);
                }
                
            }
        }

        private void UpdateProgress()
        {
            progressBar.value = (float)videoPlayer.time; 
        }
    }
}
