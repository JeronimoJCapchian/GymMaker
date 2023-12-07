using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

namespace Interactiva.Core.Navigation
{
    /// <summary>
    /// Base class for all Navigators (e.g. player controllers).
    /// Inherit from it to create new player controllers.
    /// </summary>
    [System.Serializable]
    public abstract class Navigator : MonoBehaviour
    {
        [Header("Navigator")]
        [SerializeField] private float m_minSensitivity = 0.25f;
        [SerializeField] private float m_maxSensitivity = 2f;
        /// <summary>
        /// The sensitivity of this navigator's camera movement.
        /// The default value is the midpoint between min and max sensitivities above.
        /// </summary>
        protected float sensitivity = 1f;

        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onDisable;
        //Does this navigator currently have control of the main camera?
        protected bool hasCameraControl = true;
        //Main camera
        protected Camera m_Camera;
        protected Renderer[] renderers;

        protected virtual void Awake()
        {
            m_Camera = Camera.main;
            renderers = GetComponentsInChildren<Renderer>();
        }

        private void Start()
        {
            SetCameraSensitivity(0.5f); //Setting sensitivity to mid point between min and max
        }

        /// <summary>
        /// Method enables this navigator, making it display the camera and receive player input
        /// </summary>
        public virtual void Enable()
        {
            gameObject.SetActive(true);
            onEnable.Invoke();
        }

        /// <summary>
        /// Method disables the navigator, disabling its camera and player input
        /// </summary>
        public virtual void Disable()
        {
            gameObject.SetActive(false);
            onDisable.Invoke();
        }
        
        
        /// <summary>
        /// Method returns the virtual camera of this navigator.
        /// Must be implemented by Child class.
        /// </summary>
        /// <returns>The Virtual Camera of this navigator.</returns>
        public abstract CinemachineVirtualCameraBase GetVirtualCamera();
        /// <summary>
        /// Method sets the camera sensitivity to the specified value.
        /// </summary>
        /// <param name="sensitivity">Value to set sensitivity to. Accepts ranges from 0 to 1.</param>
        public virtual void SetCameraSensitivity(float sensitivity)
        {
            this.sensitivity = Mathf.Lerp(m_minSensitivity, m_maxSensitivity, sensitivity) * NavigationManager.singleton.PlatformSensitivity;
        }
        /// <summary>
        /// Method is called when navigator loses Camera Control
        /// </summary>
        /// <returns></returns>
        public virtual void OnLoseCameraControl()
        {
            hasCameraControl = false;
        }
        
        /// <summary>
        /// Method is called when navigator gains camera control.
        /// </summary>
        public virtual void OnGainCameraControl()
        {
            hasCameraControl = true;
        }
    } 
}
