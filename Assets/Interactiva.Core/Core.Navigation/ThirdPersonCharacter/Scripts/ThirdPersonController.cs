using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Interactiva.Core.Navigation.ThirdPerson
{
    /// <summary>
    /// Class handles input for the ThirdPersonController.
    /// This class requires a ThirdPersonCharacter attached to the same GameObject.
    /// Also make sure to have a NavigationManager in your scene.
    /// </summary>
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonController : Navigator
    {
        public static ThirdPersonController singleton;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        //Saving movement input
        private float h;
        private float v;
        bool crouch = false;
        bool sprint = false;
        //Sensitivity cache
        private Vector2 cameraSpeed;
        private const float altTurnMult = 0.5f; //Variable holding turn multiplier for alternate input mode
        //Cinemachine
        private CinemachineFreeLook m_vCam;
        private CinemachineStateDrivenCamera m_vBlendCam;

        public bool Sprinting => sprint;
        public float Speed => m_Move.magnitude;

        protected override void Awake()
        {
            base.Awake();
            singleton = this;
            m_vCam = GetComponentInChildren<CinemachineFreeLook>();
            m_vBlendCam = GetComponentInChildren<CinemachineStateDrivenCamera>();
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            renderers = GetComponentsInChildren<Renderer>();
        }
        private void Start()
        {
            cameraSpeed.x = m_vCam.m_XAxis.m_MaxSpeed;
            cameraSpeed.y = m_vCam.m_YAxis.m_MaxSpeed;
        }

        /// <summary>
        /// Method event for receiving Jump input.
        /// </summary>
        /// <param name="ctx"></param>
        public void GetJumpInput(InputAction.CallbackContext ctx)
        {
            if (!hasCameraControl || NavigationManager.IsFrozen()) return;

            m_Jump = ctx.started;
        }
        /// <summary>
        /// Obtaining Main Input.
        /// WASD in Desktop, Left Stick on Mobile.
        /// </summary>
        /// <param name="ctx"></param>
        public void GetInput(InputAction.CallbackContext ctx)
        {
            if (!hasCameraControl || NavigationManager.IsFrozen()) return;
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Main)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                //Enable sprinting when input is close to 1 only in mobile.
                //This is due to a lack of a dedicated sprint button on mobile.
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

                if (input.magnitude > 0.975f)
                {
                    sprint = true;
                } else
                {
                    sprint = false;
                }
#endif
                h = input.x;
                v = input.y;
            }
        }
        /// <summary>
        /// Get alternate Input. Arrow keys on desktop.
        /// </summary>
        /// <param name="ctx"></param>
        public void GetAltInput(InputAction.CallbackContext ctx)
        {
            if (!hasCameraControl || NavigationManager.IsFrozen()) return;
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Alternate) { 
                Vector2 input = ctx.ReadValue<Vector2>();
                h = input.x;
                v = input.y > 0 ? input.y : 0;
            }
        }

        public void GetCrouchInput(InputAction.CallbackContext ctx)
        {
            if (!hasCameraControl || NavigationManager.IsFrozen()) return;

            crouch = ctx.ReadValueAsButton();
        }

        public void GetSprintInput(InputAction.CallbackContext ctx)
        {
            if (!hasCameraControl || NavigationManager.IsFrozen()) return;

            sprint = ctx.ReadValueAsButton();
        }
        /// <summary>
        /// Get the Virtual Camera associated with this Third Person Controller.
        /// </summary>
        /// <returns>A reference to the Virtual Camera of this Third Person Controller</returns>
        public override CinemachineVirtualCameraBase GetVirtualCamera()
        {
            return m_vBlendCam;
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {

           // Debug.Log("Input Values: " + h + ", " + v);
            // calculate move direction to pass to character
            if (m_Camera != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h* m_Camera.transform.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
			// walk speed multiplier
	        if (!sprint) m_Move *= 0.5f;

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

        /// <summary>
        /// Method sets the sensitivity for both input modes.
        /// Sensitivity affects the Camera speed movement on the Main input mode
        /// and affects the rotation speed in the secondary input mode.
        /// </summary>
        /// <param name="sensitivity"></param>
        public override void SetCameraSensitivity(float sensitivity)
        {
            base.SetCameraSensitivity(sensitivity);
            m_vCam.m_XAxis.m_MaxSpeed = this.sensitivity * cameraSpeed.x;
            m_vCam.m_YAxis.m_MaxSpeed = this.sensitivity * cameraSpeed.y;
            //Setting sensitivity to animator for alternate input mode
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Alternate) {
                m_Character.SetTurnSensitivity(this.sensitivity * altTurnMult);
            }

        }
        /// <summary>
        /// Message sent by the NavigationManager when the InputMode is changed.
        /// </summary>
        /// <param name="inputMode">The new InputMode.</param>
        public void OnInputModeChange(NavigationManager.InputMode inputMode)
        {
            bool shoulderCam = inputMode == NavigationManager.InputMode.Alternate ? true : false;
            GetComponentInChildren<CinemachineStateDrivenCamera>().GetComponent<Animator>().SetBool("ShoulderCam", shoulderCam);
            m_Character.SetTurnSensitivity(shoulderCam ? sensitivity * altTurnMult : 1);
        }
        /// <summary>
        /// Message is received when hiding state is changed by the NavigationManager.
        /// Currently this behaves by fading the renderers into a semi-transparent mode using
        /// the FadeCoroutine to achieve this gradually.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="immediate"></param>
        public void OnHideStateChange(bool state)
        {
            StopAllCoroutines();
            foreach (Renderer r in renderers)
            {
                foreach (Material m in r.materials)
                {
                    StartCoroutine(FadeCoroutine(m, state));
                }
            }
        }
        //Might want to consider moving fading code outside
        private IEnumerator FadeCoroutine(Material m, bool state)
        {
            const float fadeSpeed = 5f;
            float target = state ? 0.2f : 1f;
            float alpha = m.color.a;
            if (state)
            {
                m.SetOverrideTag("RenderType", "Transparent");
                m.SetFloat("_Surface", 1);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.EnableKeyword("_ALPHABLEND_ON");
                m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            while (!Mathf.Approximately(alpha, target))
            {
                alpha = Mathf.MoveTowards(alpha, target, Time.deltaTime * fadeSpeed);
                m.color = new Color(m.color.r, m.color.g, m.color.b, alpha);
                yield return null;
            }
            if (!state)
            {
                m.SetOverrideTag("RenderType", "Opaque");
                m.SetFloat("_Surface", 0);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                m.SetInt("_ZWrite", 1);
                m.DisableKeyword("_ALPHABLEND_ON");
                m.renderQueue = -1;
            }
        }
        /// <summary>
        /// Message sent by the NavigationManager when the Navigator switches Freeze state.
        /// </summary>
        /// <param name="newState">True = frozen, False = unfrozen</param>
        public void OnFreezeStateChange(bool newState)
        {
            h = 0;
            v = 0;
            m_vCam.enabled = !newState;
        }
    }
}
