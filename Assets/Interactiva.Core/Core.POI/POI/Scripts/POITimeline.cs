using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Interactiva.Core.Navigation;

namespace Interactiva.Core.POIs
{
    public class POITimeline : POI, IPOIFocusable
    {
        [SerializeField] private CinemachineVirtualCameraBase virtualCamera;
        [SerializeField] private string promptText = "Interact";
        [SerializeField] private PlayableDirector timeline;
        [SerializeField] private UnityEvent onCompleteTimeline;

        private bool isPlaying = false;
        public CinemachineVirtualCameraBase VirtualCamera => virtualCamera;

        public string AltPromptText => "";

        public bool HideNavigator => false;

        public bool FreezeNavigator => true;

        public bool AttenuateAudio => false;

        public bool ToggleValue => isPlaying;

        public override bool CanInteract => !isPlaying;

        public override string PromptText => promptText;

        public override bool HidePromptOnInteract => true;

        private bool enteredTrigger = false;

        private float timerToFinish = 0;

        protected override void Awake()
        {
            base.Awake();
            timeline.stopped += ((PlayableDirector dir) =>
            {
                onCompleteTimeline.Invoke();
            });
        }

        public override void Interact()
        {
            if (!isPlaying)
            {
                OnToggleTrue();
                Debug.Log("Interacted: " + gameObject.name);
                base.Interact();
            }
        }
        public void OnToggleFalse()
        {
            isPlaying = false;
        }

        public void OnToggleTrue()
        {
            isPlaying = true;
            timeline.Play();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Entered trigger: " + gameObject.name);
                POIManager.AddFocusablePOI(this);
                if (!enteredTrigger)
                {
                    timeline.stopped += ((PlayableDirector dir) =>
                    {
                        OnToggleFalse();
                        other.GetComponent<FocusComponent>().ChangeToggleState(this);
                        OnTriggerExit(other);
                    });
                    enteredTrigger = true;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}