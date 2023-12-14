using UnityEngine;
using Cinemachine;

namespace Interactiva.Core.POIs
{
    /// <summary>
    /// IPOI Focusable is an interface that implements IPOIToggle.
    /// There is only one difference: IPOIFocusable includes the property VirtualCamera.
    /// This is the VirtualCamera that will be put in focus when the player interacts with this POI.
    /// 
    /// Use this if you want to take control of the camera when the player interacts with this POI. (e.g. an interactive exhibit)
    /// </summary>
    public interface IPOIFocusable : IPOIToggle
    {
        /// <summary>
        /// Method returns the virtual camera associated with this POIFocusable.
        /// </summary>
        /// <returns>The virtual camera associated with this POI.</returns>
        public CinemachineVirtualCameraBase VirtualCamera { get; }


    }
}