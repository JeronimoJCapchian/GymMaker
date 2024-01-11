using UnityEngine;
using UnityEngine.InputSystem;
using Interactiva.Core.Utilities;
using Interactiva.Core.POIs;
using Interactiva.Core.UI;
using TMPro;
using UnityEngine.Events;
using Interactiva.Core.Audio;

namespace Interactiva.Core.Navigation
{
    public class FocusComponent : MonoBehaviour
    {
        public static FocusComponent singleton;

        [SerializeField] private float distanceToFocus = 2f;
        [SerializeField] private float angleToFocus = 60f;
        [SerializeField] private CrossPlatformObject<string> promptAction = new CrossPlatformObject<string>("Press 'Space' to ", "Tap to ");

        /// <summary>
        /// Event executes when user is prompted to interact with any POI.
        /// </summary>
        [SerializeField] private UnityEvent onPromptInteract;
        [SerializeField] private UnityEvent onStopPromptInteract;


        /// <summary>
        /// Current navigator this Wrapper is monitoring.
        /// </summary>
        [SerializeField] private Navigator navigator;
        [SerializeField] private TextMeshProUGUI promptUI;
        

        private const int focusPriority = 20; //Priority to set virtual camera when focusing.
        private const int unfocusPriority = 0; //Priority to set virtual camera when unfocusing.

        private bool canInteract = true; //Whether we can interact with POIs in the scene
        /// <summary>
        /// Whether we are currently interacting in a POI (and Camera is controlled by POI)
        /// </summary>
        private bool interacting = false;

        private POI curPoi; //Current POI we can interact with
        private POI prevPoi; //Previous POI cache, used to detect when we've shifted between POIs.

        private void Start()
        {
            singleton = this;
        }
        private void Update()
        {
            if (!interacting)
            {
                if ((curPoi = POIManager.singleton.GetBestFocusablePOI(Camera.main.transform, distanceToFocus, angleToFocus)) != null)
                {
                    if (!curPoi.CanInteract || !NavigationManager.singleton.IsNavigatorEnabled(navigator) || UIManager.singleton.HasActiveComponents())
                    {
                        onStopPromptInteract.Invoke();
                        return;
                    }
                    //Highlight object that can be focused on
                    if (prevPoi != null && prevPoi != curPoi) //If we have changed are highlighted POI without unhighlighting
                    {
                        prevPoi.DisableHighlight(); //Disable the previous POI highlight
                    }
                    if (promptUI != null)
                    {
                        promptUI.text = promptAction.Get() + curPoi.PromptText;
                    }
                    curPoi.EnableHighlight(); //Enable new POI highlight
                    onPromptInteract.Invoke();
                } else //If we don't have a POI in sight
                {
                    if (prevPoi != null) //If we had a previos POI
                    {
                        prevPoi.DisableHighlight(); //Stop highlighting previous POI
                        onStopPromptInteract.Invoke();
                    }
                }
                prevPoi = curPoi; //Update previous POI
                
            }
        }
        /// <summary>
        /// Method for events.
        /// </summary>
        public void OnPressInteract()
        {
            if (!canInteract || !NavigationManager.singleton.IsNavigatorEnabled(navigator) || UIManager.singleton.CanFreezeInteraction())
            {
                return;
            }
            PressInteract();
        }
        /// <summary>
        /// Method for key input.
        /// </summary>
        /// <param name="context"></param>
        public void OnPressInteract(InputAction.CallbackContext context)
        {
            
            if (context.started)
            {
                OnPressInteract();
            }
        }
        /// <summary>
        /// Method returns if the player is currently interacting (e.g. a IPOIToggle is true)
        /// </summary>
        /// <returns>Whether the navigator is interacting.</returns>
        public bool IsInteracting()
        {
            return interacting;
        }

        private void PressInteract()
        {
            if (curPoi == null)
            {
                return;
            }
            if (!curPoi.CanInteract)
            {
                return;
            }
            
            if (curPoi is IPOIToggle)
            {
                IPOIToggle poiToggle = (IPOIToggle)curPoi;
                bool toggle = poiToggle.ToggleValue;
                curPoi.Interact();
                if (poiToggle.ToggleValue != toggle)
                {
                    ChangeToggleState(poiToggle);
                }
            } else
            {
                curPoi.Interact();
            }
            if (curPoi.HidePromptOnInteract)
            {
                curPoi.DisableHighlight();
                onStopPromptInteract.Invoke();
            }
            
            
        }
        /// <summary>
        /// Method used to force change toggle state. It is invoked by interacting with an IPOIToggle.
        /// It can also be triggered externally via script if needed.
        /// </summary>
        /// <param name="poiToggle"></param>
        public void ChangeToggleState(IPOIToggle poiToggle)
        {
            NavigationManager.singleton.SetHideStateAll(poiToggle.ToggleValue ? poiToggle.HideNavigator : false);
            if (poiToggle.AttenuateAudio)
            {
                SoundManager.singleton.SetAudioAttenuationLevel(poiToggle.ToggleValue);
            }
            if (poiToggle.FreezeNavigator)
            {
                NavigationManager.singleton.SetFreezeStateAll(poiToggle.ToggleValue);
            }
            interacting = poiToggle.ToggleValue;
            if (poiToggle.ToggleValue)
            {
                if (promptUI != null)
                {
                    promptUI.text = promptAction.Get() + poiToggle.AltPromptText;
                }
            }
            if (poiToggle is IPOIFocusable)
            {
                IPOIFocusable poiFocusable = (IPOIFocusable)poiToggle;
                if (poiFocusable.VirtualCamera != null)
                {
                    poiFocusable.VirtualCamera.Priority = poiFocusable.ToggleValue ? focusPriority : unfocusPriority;
                    navigator.GetVirtualCamera().Priority = poiToggle.ToggleValue ? unfocusPriority : focusPriority;
                }
            }
        }
        /// <summary>
        /// Set whether controller can interact with POIs
        /// </summary>
        /// <param name="value">True = can Interact, false = cannot Interact</param>
        public void SetCanInteract(bool value)
        {
            canInteract = value;
        }

    }
}
