using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Cinemachine;

namespace Interactiva.Core.POIs
{
    /// <summary>
    /// This class implements basic focus functionality for an exhibit object.
    /// Check the Dragon 3D in the prefabs folder for the setup.
    /// </summary>
    public class POIFocus : POI, IPOIFocusable
    {
        public enum POIType { Rotation3D, FreeLook2D}
        [Tooltip("Whether this POI's Virtual Camera starts disabled.")]
        [SerializeField] private bool disableVirtualCamOnStart = true;
        [SerializeField] private bool hideNavigatorOnToggle = false;
        [SerializeField] private CinemachineVirtualCameraBase virtualCamera;
        
        [Tooltip("The maximum level this camera can zoom into")]
        [SerializeField] private byte maxZoomLevel = 4;
        [SerializeField] private bool canZoom = true;
        [SerializeField] private UnityEvent onZoomIn;
        [SerializeField] private UnityEvent onZoomOut;

        private float zoomLevel = 0;
        private bool isFocused = false;

        [SerializeField] private string _PromptTextVar = "focus";
        [SerializeField] private string _AltPromptTextVar = "unfocus";
        [SerializeField] private bool _HidePromptOnInteract = false;
        [SerializeField] private bool _AttenuateAudio = false;

        public override string PromptText => _PromptTextVar;
        public override bool HidePromptOnInteract => _HidePromptOnInteract;
        public override bool CanInteract => true;

        /// <summary>
        /// IPOIToggle Properties
        /// </summary>
        public bool HideNavigator => hideNavigatorOnToggle;
       
        public string AltPromptText => _AltPromptTextVar;
        public bool FreezeNavigator => true;
        public bool AttenuateAudio => _AttenuateAudio;
        public bool ToggleValue => isFocused;

        /// <summary>
        /// IPOIFocusable Properties
        /// </summary>
        public CinemachineVirtualCameraBase VirtualCamera => virtualCamera;

        private void Start()
        {
            if (disableVirtualCamOnStart)
            {
                virtualCamera.gameObject.SetActive(false);
            }
        }

        public override void Interact()
        {
            if (isFocused)
            {
                OnToggleFalse();
            } else
            {
                base.Interact();
                OnToggleTrue();
            }
        }

        public void OnControlZoom(InputAction.CallbackContext context)
        {
            //Revise method
            if (canZoom && isFocused)
            {
                float input = context.ReadValue<float>();
                zoomLevel += input;
                zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoomLevel);
                //camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defFOV - (12.5f * zoomLevel), 0.1f); //Might have trouble interpolating scroll wheel
                if (Mathf.Abs(input) > 0)
                {
                    if (zoomLevel == 0)
                    {
                        onZoomOut.Invoke();
                    }
                    if (zoomLevel > 0)
                    {
                        onZoomIn.Invoke();
                    }
                }
            }
            
        }

        public void OnToggleTrue()
        {
            virtualCamera.gameObject.SetActive(true);
            isFocused = true;
        }

        public void OnToggleFalse()
        {
            virtualCamera.gameObject.SetActive(false);
            isFocused = false;
        }
    }
}
