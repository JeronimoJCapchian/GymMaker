using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactiva.Core.Utilities.EventFilters
{
    public class RandomEventFilter : BaseEventFilter
    {
        [Range(0f, 1f)] public float probability = 0.5f;



        public override void OnTrigger()
        {
            if (enabled)
            {
                float random = Random.value;
                if (random <= probability)
                {
                    onTrigger.Invoke();
                }
            }
        }
    }
}