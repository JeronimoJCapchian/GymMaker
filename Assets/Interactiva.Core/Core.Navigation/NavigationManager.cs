using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Interactiva.Core.Navigation
{
    /// <summary>
    /// NavigationManager is the main class that manages the usage of Navigators and their input.
    /// Supports Switching, freezing, hiding, navigators and locking the cursor.
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        /// <summary>
        /// Enum containing input modes.
        /// Generally, the two input modes are only available on desktop platforms and
        /// correspond to regular game controls for the main, and simplified controls for the secondary.
        /// </summary>
        public enum InputMode { Main, Alternate };
        [Tooltip("Default Input mode. Must be changed via the ChangeInputMode() method at runtime.")]
        [SerializeField] private InputMode inputMode = InputMode.Main;

        public static NavigationManager singleton;
        /// <summary>
        /// An array containing all the navigators in the scene. Please assign all navigators manually.
        /// </summary>
        [SerializeField] private Navigator[] navigators;
        /// <summary>
        /// Variable overrides locking behavior on start. Change only through inspector.
        /// </summary>
        [SerializeField] private bool lockCursor = true; //Inspector overrides locking behavior

        [SerializeField] private UnityEvent onFreeze;
        [SerializeField] private UnityEvent onUnfreeze;
        /// <summary>
        /// The index of the current navigator.
        /// </summary>
        private int curNavIndex = 0;
        /// <summary>
        /// The frozen layer level. If isFrozen > 0 then the navigator is frozen.
        /// </summary>
        private int isFrozen = 0;
        /// <summary>
        /// Is the cursor currently locked?
        /// </summary>
        private bool m_cursorIsLocked = false;
        /// <summary>
        /// The lock layer level. 0 or smaller permits locking, 1 or bigger does not.
        /// </summary>
        private int canLockCursor = 0;

        [SerializeField] private float androidSensitivity = 1f;

        [SerializeField] private float IOSSensitivity = 1f;

        [SerializeField] private float desktopSensitivity = 1f;

        public float PlatformSensitivity
        {
            get
            {
#if UNITY_WEBGL || UNITY_STANDALONE
                return desktopSensitivity;
#elif UNITY_ANDROID
                return androidSensitivity;
#elif UNITY_IOS 
            return IOSSensitivity;
#else
            return 1;
#endif
            }
        }

        private void Awake()
        {
            singleton = this; //Assigning our singleton
        }

        private void Start()
        {
            ChangeInputMode(inputMode);
            //Enabling our current navigator, disabling all others
            for (int i = 0; i < navigators.Length; i++)
            {
                
                if (i == curNavIndex)
                {
                    navigators[i].Enable();
                }
                else
                {
                    
                    navigators[i].Disable();
                }
                
            }
        }

        private void Update()
        {
#if UNITY_WEBGL || UNITY_EDITOR || UNITY_STANDALONE
            //Only update cursor lock if we are in a desktop platform.
            InternalLockUpdate();
#endif
        }


        /// <summary>
        /// Get our current navigator.
        /// </summary>
        /// <returns>The navigator being used by the user.</returns>
        public Navigator GetCurrentNavigator()
        {
            return navigators[curNavIndex];
        }

        /// <summary>
        /// Set our current navigator by index.
        /// </summary>
        /// <param name="index">The index of the navigator.</param>
        public void SetCurrentNavigator(int index)
        {
            if (index < 0 || index >= navigators.Length || index == curNavIndex) //Array out of bounds
            {
                return;
            }
            GetCurrentNavigator().Disable();
            curNavIndex = index;
            GetCurrentNavigator().Enable();
        }
        /// <summary>
        /// Returns whether given navigator is active/enabled.
        /// </summary>
        /// <param name="nav">Navigator to check.</param>
        /// <returns>Whether navigator checked is the currently active navigator.</returns>
        public bool IsNavigatorEnabled(Navigator nav)
        {
            return navigators[curNavIndex] == nav;
        }

        /// <summary>
        /// Method sets whether the cursor can be locked.
        /// It works by layering several locks (e.g. if it is unlocked twice in a row, it needs to be relocked twice).
        /// Make sure to not execute more than once if not layering intentionally.
        /// </summary>
        /// <param name="value">Lock state. True = locked, False = unlocked.</param>
        public void SetCursorCanLock(bool value)
        {
            canLockCursor += value ? -1 : 1;
            if (canLockCursor > 0)
            {
                SetCursorLock(false);
            } else if (canLockCursor <= 0)
            {
                SetCursorLock(true);
            }
        }
        /// <summary>
        /// Method unlocks or relocks cursor.
        /// </summary>
        /// <param name="value"></param>
        private void SetCursorLock(bool value)
        {

            if (!lockCursor || canLockCursor > 0)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                m_cursorIsLocked = false;
                return;
            }
            m_cursorIsLocked = value;
        }

        /// <summary>
        /// Method updates cursor lock in editor and standalone builds.
        /// </summary>
        private void InternalLockUpdate()
        {
            if (canLockCursor <= 0 && lockCursor)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    m_cursorIsLocked = false;
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    m_cursorIsLocked = true;
                }

                if (m_cursorIsLocked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else if (!m_cursorIsLocked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            } else
            {
                if (m_cursorIsLocked)
                {
                    SetCursorLock(false);
                }
            }
        }
        /// <summary>
        /// Method freezes or unfreezes all navigators.
        /// A frozen navigator cannot move or receive input.
        /// Good for pause menu behavior or switching input to another script.
        /// </summary>
        /// <param name="state">True = frozen, false = unfrozen.</param>
        public void SetFreezeStateAll(bool state)
        {
            isFrozen += state ? 1 : -1;
            if (IsFrozen())
            {
                onFreeze.Invoke();
            } else
            {
                onUnfreeze.Invoke();
            }
            foreach (Navigator nav in navigators)
            {
                nav.SendMessage("OnFreezeStateChange", IsFrozen(), SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Returns whether the main Navigator is frozen.
        /// </summary>
        /// <returns></returns>
        public static bool IsFrozen()
        {
            return singleton.isFrozen > 0;
        }
        /// <summary>
        /// Method sends a hide message to a navigator.
        /// Not all navigators support hiding. If they do, hiding will hide the renderer of the navigator.
        /// </summary>
        /// <param name="state"></param>
        public void SetHideStateAll(bool state)
        {
            foreach (Navigator nav in navigators)
            {
                nav.SendMessage("OnHideStateChange", state, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// The FocusComponent is 
        /// </summary>
        /// <returns>a reference to the FocusComponent of the main Navigator</returns>
        public FocusComponent GetFocusComponent()
        {
            return navigators[curNavIndex].GetComponentInChildren<FocusComponent>();
        }
        /// <summary>
        /// Set sensitivity sets the sensitivity to all characters.
        /// </summary>
        /// <param name="value">The sensitivity to be set. Will be clamped between 0 and 1 (min and max)</param>
        public void SetSensitivity(float value)
        {
            foreach (Navigator nav in navigators)
            {
                nav.SetCameraSensitivity(value);
            }
        }
        /// <summary>
        /// Method is used by the UI Sensitivity slider.
        /// Assign this to the OnValueChange on the Slider component.
        /// </summary>
        /// <param name="slider"></param>
        public void SetSensitivity(Slider slider) 
        {
            SetSensitivity(slider.value);    
        }

        #region Look Input
        /// <summary>
        /// Variable saves the current look input values.
        /// </summary>
        private Vector2 lookInput;
        /// <summary>
        /// Method returns the look input value.
        /// </summary>
        /// <returns>the look input value.</returns>
        public Vector2 GetLookInput()
        {
            return lookInput;
        }
        /// <summary>
        /// Method sets the look input. Used by the Player Input component.
        /// </summary>
        /// <param name="ctx"></param>
        public void SetLookInput(InputAction.CallbackContext ctx)
        {
            if (inputMode == InputMode.Main)
            {
                lookInput = ctx.ReadValue<Vector2>();
            }
        }
        /// <summary>
        /// Method sets the look input for the alternate navigation mode.
        /// Used by the Player Input component.
        /// </summary>
        /// <param name="ctx"></param>
        public void SetAltLookInput(InputAction.CallbackContext ctx)
        {
            if (inputMode == InputMode.Alternate)
            {
                lookInput = ctx.ReadValue<Vector2>();
            }
        }

        public void ChangeInputMode(InputMode im)
        {
            inputMode = im;
            foreach (Navigator nav in navigators)
            {
                //Broadcast our OnInputModeChange event to Navigators
                nav.SendMessage("OnInputModeChange", im, SendMessageOptions.DontRequireReceiver);
            }
        }
        /// <summary>
        /// Sets the input mode according to passed boolean.
        /// </summary>
        /// <param name="toggle"></param>
        public void SetInputModeToggle(bool toggle)
        {
            ChangeInputMode(toggle ? InputMode.Main : InputMode.Alternate);
        }

        public static InputMode GetInputMode()
        {
            return singleton.inputMode;
        }
        #endregion
    }


}
