using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactiva.Core.Utilities
{
    /// <summary>
    /// Class forces the correct control scheme on different inputs. Attach to the same GameObject as your PlayerInput.
    /// The PlayerInput component can have erroneous behavior when dealing with more than one scheme in different platforms.
    /// This script solves those problems.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputSchemeSwitcher : MonoBehaviour
    {
        private PlayerInput inputManager;

        private void Start()
        {
            inputManager = GetComponent<PlayerInput>();

        }

        private void Update()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            if (inputManager.currentControlScheme != "Gamepad")
            {
                inputManager.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
            }
#elif (UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE)
            if (inputManager.currentControlScheme != "Keyboard&Mouse")
            {
                inputManager.SwitchCurrentControlScheme("Keyboard&Mouse");
            }
#endif
        }
    }
}