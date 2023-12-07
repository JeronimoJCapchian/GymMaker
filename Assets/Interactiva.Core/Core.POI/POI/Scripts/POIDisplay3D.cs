using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Interactiva.Core.POIs
{
    public class POIDisplay3D : POI, IPOIToggle
    {
        [SerializeField] private UnityEvent onToggleTrue;
        [SerializeField] private UnityEvent onToggleFalse;
        private bool toggle = false;
        private Animator animator;
        private Outline outlineEffect;

        [SerializeField] private bool hideNavigatorOnToggle = false;
        [SerializeField] private string _PromptTextVar = "show more";
        [SerializeField] private string _AltPromptTextVar = "hide info";
        [SerializeField] private bool _HidePromptOnInteract = true;
        [SerializeField] private bool _AttenuateAudio = false;

        public override bool CanInteract => true;

        public override string PromptText => _PromptTextVar;

        public override bool HidePromptOnInteract => _HidePromptOnInteract;

        public string AltPromptText => _AltPromptTextVar;

        public bool HideNavigator => hideNavigatorOnToggle;

        public bool FreezeNavigator => false;

        public bool AttenuateAudio => _AttenuateAudio;

        public bool ToggleValue => toggle;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            outlineEffect = GetComponentInChildren<Outline>();
        }

        public override void Interact()
        {
            base.Interact();
            if (toggle)
            {
                OnToggleFalse();
            } else
            {
                OnToggleTrue();
            }
            
        }
        public override void EnableHighlight()
        {
            if (canHighlight)
            {
                outlineEffect.enabled = true;
            }
        }

        public override void DisableHighlight()
        {
            if (canHighlight)
            {
                outlineEffect.enabled = false;
            }
        }

        public void OnToggleFalse()
        {
            toggle = false;
            onToggleFalse.Invoke();
            animator.SetBool("Show", false);
        }

        public void OnToggleTrue()
        {
            toggle = true;
            onToggleTrue.Invoke();
            animator.SetBool("Show", true);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
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
    }
}