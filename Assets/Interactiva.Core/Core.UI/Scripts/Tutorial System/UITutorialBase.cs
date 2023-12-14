using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactiva.Core.UI
{
    
    public abstract class UITutorialBase : UIPrompt
    {
        [SerializeField] private InputAction key;
        [SerializeField] private float thresholdToPress = 0.5f;
        

        private float timer = 0;

        private void OnEnable()
        {
            timer = 0;
        }

        public override void Setup()
        {
            key.Enable();
        }

        public override bool Poll()
        {
            if (key.phase == InputActionPhase.Started)
            {
                timer += Time.deltaTime;
            }
            if (timer >= thresholdToPress)
            {
                Finish();
                return true;
            }
            return false;
        }

        public void ForceComplete()
        {
            timer = thresholdToPress + 1f;
        }

        public override void Finish()
        {
            key.Disable();
        }
    }
}
