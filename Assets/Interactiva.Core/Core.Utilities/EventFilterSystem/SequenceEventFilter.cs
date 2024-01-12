using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interactiva.Core.Utilities.EventFilters
{
    public class SequenceEventFilter : BaseEventFilter
    {
        public UnityEvent[] onTriggerEvents;
        private int counter = 0;

       

        public override void OnTrigger()
        {
            if (enabled)
            {
                if (counter == 0)
                {
                    onTrigger.Invoke();
                }
                if (counter < onTriggerEvents.Length)
                {
                    onTriggerEvents[counter].Invoke();
                }
                counter++;
            }
        }


    }
}