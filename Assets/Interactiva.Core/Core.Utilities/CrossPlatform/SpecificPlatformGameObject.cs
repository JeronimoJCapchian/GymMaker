using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Utilities
{
    
    public class SpecificPlatformGameObject : MonoBehaviour
    {
        public enum Platform {Desktop, Mobile};
        [SerializeField] private Platform platformToDisable = Platform.Desktop;

        private void Awake()
        {

#if (UNITY_EDITOR && (UNITY_STANDALONE || UNITY_WEBGL)) || UNITY_STANDALONE || UNITY_WEBGL
            if (platformToDisable == Platform.Desktop)
            {
                gameObject.SetActive(false);
            } else
            {
                gameObject.SetActive(true);
            }
#else
            if (platformToDisable == Platform.Mobile) {
                gameObject.SetActive(false);
            } else {
                gameObject.SetActive(true);
            }
#endif
            Destroy(this);
        }
    }
}