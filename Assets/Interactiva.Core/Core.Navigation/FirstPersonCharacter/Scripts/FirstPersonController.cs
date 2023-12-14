using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

using UnityEngine.InputSystem.OnScreen;

using Interactiva.Core.Navigation.Utilities;
using Cinemachine;

namespace Interactiva.Core.Navigation.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : Navigator
    {
        
        [Header("First Person Character")]
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
         public MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        public static FirstPersonController singleton;
        private bool m_Jump;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private Transform m_CameraFollowTarget;
        private AudioSource m_AudioSource;
        private bool m_IsWalking = false;
        //Cinemachine
        private CinemachineVirtualCamera m_vCam;

        private float altInput = 0;
        [NonSerialized] public Vector2 runAxis;
        [NonSerialized] public bool jumpAxis;

        public bool StickToGround { get; set; } = true;

        protected override void Awake()
        {
            base.Awake();
            singleton = this;
        }
        // Use this for initialization
        private void Start()
        {
            m_CameraFollowTarget = transform.Find("CameraFollow");
            m_vCam = GetComponentInChildren<CinemachineVirtualCamera>();
            m_MouseLook.XSensitivity = m_MouseLook.YSensitivity = this.sensitivity;
            m_CharacterController = GetComponent<CharacterController>();
            m_OriginalCameraPosition = m_CameraFollowTarget.transform.localPosition;
            m_FovKick.Setup(Camera.main);
            m_HeadBob.Setup(m_CameraFollowTarget, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_CameraFollowTarget);
            m_IsWalking = true;


            touchField = FindObjectOfType<FixedTouchField>();
        }

        FixedTouchField touchField; //AGREGADO

        // Update is called once per frame
        private void Update()
        {
            if (NavigationManager.IsFrozen() || !hasCameraControl)
            {
                return;
            }
            RotateView();
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Alternate)
            {
                m_MouseLook.GetMouseInput(new Vector2(altInput * Time.deltaTime * 60f, 0));
            } else
            {
                Vector2 input;

#if (UNITY_STANDALONE || UNITY_WEBGL)
                input = NavigationManager.singleton.GetLookInput();
#else
                input = (NavigationManager.singleton.GetLookInput() + touchField.TouchDist) / 4;
#endif

                m_MouseLook.GetMouseInput(input);
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }

        public override void Enable()
        {
            base.Enable();
        }

        public override void Disable()
        {
            base.Disable();
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            if (NavigationManager.IsFrozen() || !hasCameraControl)
            {
                return;
            }
            // set the desired speed to be walking or running
            float speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed; ;
            
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                if (StickToGround)
                {
                    m_MoveDir.y = -m_StickToGroundForce;
                }

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded || m_FootstepSounds.Length <= 0)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_CameraFollowTarget.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_CameraFollowTarget.transform.localPosition;
                newCameraPosition.y = m_CameraFollowTarget.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_CameraFollowTarget.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_CameraFollowTarget.transform.localPosition = newCameraPosition;
        }

        /// <summary>
        /// Main Mouse and Keyboard (WASD) navigation mode input event.
        /// </summary>
        /// <param name="inputContext">The callback context of input system.</param>
        public void GetInput(InputAction.CallbackContext ctx)
        {
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Main)
            {
                //This is due to a lack of a dedicated sprint button on mobile.
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                Vector2 input = ctx.ReadValue<Vector2>();
                if (input.magnitude > 0.975f)
                {
                    m_IsWalking = false;
                } else
                {
                    m_IsWalking = true;
                }
#endif
                ProcessInput(ctx.ReadValue<Vector2>());
            }
        }

        /// <summary>
        /// Secondary input for Arrow-key only navigation mode.
        /// Only works on desktop.
        /// </summary>
        /// <param name="input"></param>
        public void GetInputAlt(InputAction.CallbackContext input)
        {
            if (NavigationManager.GetInputMode() == NavigationManager.InputMode.Alternate)
            {
                Vector2 rv = input.ReadValue<Vector2>();
                ProcessInput(new Vector2(0, rv.y));
                if (rv.x > 0)
                {
                    altInput = 1f;
                } else if (rv.x < 0)
                {
                    altInput = -1f;
                } else
                {
                    altInput = 0;
                }
            }
        }

        private void ProcessInput(Vector2 inputValue)
        {
            if (!hasCameraControl)
            {
                return;
            }
            bool waswalking = m_IsWalking;

            // read input and set the desired speed to be walking or running
            m_Input = inputValue;

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        public void GetRunInput (InputAction.CallbackContext context)
        {
            if (!hasCameraControl)
            {
                return;
            }
            // keep track of whether or not the character is walking or running
            m_IsWalking = !context.ReadValueAsButton();
        }

        public void GetJumpInput (InputAction.CallbackContext context)
        {
            if (!hasCameraControl)
            {
                return;
            }
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = context.started;
            }
        }

        public void GetMouseLookInput (InputAction.CallbackContext context)
        {
            if (!hasCameraControl || NavigationManager.GetInputMode() != NavigationManager.InputMode.Main || NavigationManager.IsFrozen())
            {
                return;
            }
            
        }

        


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_CameraFollowTarget.transform, touchField);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        public override void SetCameraSensitivity(float sensitivity)
        {
            base.SetCameraSensitivity(sensitivity);
            m_MouseLook.XSensitivity = m_MouseLook.YSensitivity = this.sensitivity;
        }

        public override CinemachineVirtualCameraBase GetVirtualCamera()
        {
            return m_vCam;
        }

    }
}
