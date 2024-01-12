using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Utilities.EventFilters
{
    public class SpecificPlatformEventFilter : BaseEventFilter
    {
        private enum Platform { Desktop, Mobile};
        [SerializeField] private Platform platform;
        public override void OnTrigger()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (platform == Platform.Desktop)
            {
                onTrigger.Invoke();
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (platform == Platform.Mobile)
            {
                onTrigger.Invoke();
            }
#else
            //Default behaviour
#endif
        }
    }
}
