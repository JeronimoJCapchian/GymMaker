using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// This interface is used to universalize the scripts that need to receive pause state updates.
    /// Implement this interface if you wish to receive Pause State change events.
    /// (e.g. stop certain functionality when running while paused)
    /// </summary>
    public interface IPauseEventReceiver
    {
        /// <summary>
        /// Event is received when the pause state changes.
        /// This is a one shot event, which will execute when the pause state changes.
        /// </summary>
        /// <param name="state">state = true when paused, otherwise false</param>
        public void OnPauseChange(bool state);
    }
}