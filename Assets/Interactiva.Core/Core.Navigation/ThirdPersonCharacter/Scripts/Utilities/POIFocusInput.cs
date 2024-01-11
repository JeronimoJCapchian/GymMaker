using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Interactiva.Core.UI;

namespace Interactiva.Core.Navigation.Utilities
{
    public class POIFocusInput : MonoBehaviour, AxisState.IInputAxisProvider
    {
        private CinemachineBrain cmb;
        private CinemachineVirtualCameraBase virtualCam;
        private void Awake()
        {
            cmb = FindObjectOfType<CinemachineBrain>();
            virtualCam = GetComponent<CinemachineVirtualCameraBase>();
        }
        public float GetAxisValue(int value)
        {
            if (!cmb.IsLive(virtualCam) || UIManager.singleton.HasActiveComponents()) return 0;
            switch (value)
            {
                case 0:
                    return NavigationManager.singleton.GetLookInput().x;
                case 1:
                    return NavigationManager.singleton.GetLookInput().y;
                case 2:
                    return 0;
            }
            return 0;
        }
    }
}
