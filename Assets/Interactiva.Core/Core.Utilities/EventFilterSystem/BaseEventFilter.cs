using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactiva.Core.Utilities.EventFilters
{
    public abstract class BaseEventFilter : MonoBehaviour
    {
        public UnityEvent onTrigger;

        private void Start()
        {
            
        }
        public abstract void OnTrigger();

    }
}
