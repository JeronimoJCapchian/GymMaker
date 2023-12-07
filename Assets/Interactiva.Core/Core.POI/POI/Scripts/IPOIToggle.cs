using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.POIs
{
    /// <summary>
    /// IPOIToggle is the interface used for a POI that can be toggled.
    /// For example, a video that can be played or paused, or an exhibit UI can be toggled.
    /// This is used so that the FocusComponent can correctly treat the input when toggling a POI.
    /// 
    /// To use simply create a class that inherits from POI and implements this IPOIToggle interface.
    /// </summary>
    public interface IPOIToggle
    {
        /// <summary>
        /// This is the alternate text to display in the user prompt whent they have toggled a component.
        /// For example, if the prompt text is "play video" this could return "pause video."
        /// 
        /// Please note that you should only add verbs here, as the format of prompts is handled by the FocusComponent.
        /// The focus component will create the first part of the string: "Press 'Space' to"; while this property should return the second part "pause video."
        /// </summary>
        public string AltPromptText { get; }
        /// <summary>
        /// Returns whether this POIToggle can hide player when toggled.
        /// </summary>
        public bool HideNavigator { get; }
        /// <summary>
        /// Returns whether this POIToggle can freeze the player when toggled..
        /// </summary>
        public bool FreezeNavigator { get; }
        /// <summary>
        /// Returns whether this POI will attenuate audio when focused.
        /// Audio attenuation is used so the user isn't distracted by ambient noises or music.
        /// </summary>
        public bool AttenuateAudio { get; }


        /// <summary>
        /// Returns the current status of this IPOIToggle.
        /// True = active; false = inactive.
        /// </summary>
        public bool ToggleValue { get; }
        /// <summary>
        /// Method executed when toggle is changed to true.
        /// Please note you will have to execute this manually, it is only included for organization.
        /// </summary>
        public void OnToggleTrue();
        /// <summary>
        /// Method executed when toggle is changed to false.
        /// Please note you will have to execute this manually, it is only included for organization.
        /// </summary>
        public void OnToggleFalse();

        
    }
}