using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Interactiva.Core.Utilities
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraImpulse : MonoBehaviour
    {
        private CinemachineImpulseSource impulseSource;
        
        void Awake()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void ShakeCamera()
        {
            impulseSource.GenerateImpulse();
        }
    }
}