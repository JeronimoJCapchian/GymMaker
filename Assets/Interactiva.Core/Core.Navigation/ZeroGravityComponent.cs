using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactiva.Core.Navigation.Utilities
{
    [RequireComponent(typeof(Navigator))]
    public class ZeroGravityComponent : MonoBehaviour
    {

        private float m_Input;
        private CharacterController characterController;
        private FirstPerson.FirstPersonController fpsc;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            fpsc = GetComponent<FirstPerson.FirstPersonController>();
            fpsc.StickToGround = false;
        }

        private void FixedUpdate()
        {
            if (!NavigationManager.IsFrozen())
            {
                characterController.Move(new Vector3(0, m_Input, 0) * Time.fixedDeltaTime);
            }
        }

        public void GetUpDownInput(InputAction.CallbackContext ctx)
        {
            m_Input = ctx.ReadValue<float>();
        }
    }
}