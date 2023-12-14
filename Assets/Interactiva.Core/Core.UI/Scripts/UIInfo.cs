using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// This class is a general container for the standard UI format of:
    /// -Image
    /// -Title
    /// -Description
    /// That is commonly used in all sorts of panels and popups.
    /// </summary>
    [CreateAssetMenu(fileName = "Info", menuName = "Interactiva/UI/UIInfo")]
    public class UIInfo : ScriptableObject
    {
        [System.Serializable]
        public class Info
        {
            public Sprite image;
            public string main;
            [TextArea]
            public string secondary;
        }
        [SerializeField] Info info;
        public virtual Info Information => info;

        /// <summary>
        /// Method creates a new UIInfo object with the given parameters.
        /// </summary>
        /// <param name="img">The main image.</param>
        /// <param name="txt1">The main text. Usually the title.</param>
        /// <param name="txt2">The secondary text. Usually the description.</param>
        public UIInfo (Sprite img, string txt1, string txt2)
        {
            info.image = img;
            info.main = txt1;
            info.secondary = txt2;
        }
    }
}