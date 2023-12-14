using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Interactiva.Core.Navigation.ThirdPerson
{
    /// <summary>
    /// Class provides input to Cinemachine virtual cameras. This is primarily used for Third Person character camera.
    /// Simply attach to a Cinemachine virtual camera and delete the Axis strings.
    /// Note that you need a NavigationManager in your scene.
    /// 
    /// More INFO:
    /// https://docs.unity3d.com/Packages/com.unity.cinemachine@2.6/api/Cinemachine.AxisState.IInputAxisProvider.html
    /// </summary>
    public class TPLookInput : MonoBehaviour, AxisState.IInputAxisProvider
    {
        public float GetAxisValue(int value)
        {
            if (NavigationManager.singleton != null)
            {
                if (NavigationManager.IsFrozen()) return 0;
            } else
            {
                return 0;
            }
            switch(value)
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