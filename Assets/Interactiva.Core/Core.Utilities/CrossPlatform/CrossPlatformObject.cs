using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Utilities
{
    /// <summary>
    /// Cross Platform Object is a generic class which takes in a type argument for any object.
    /// It is meant to replace variables of type T that vary between desktop and mobile.
    /// <example>
    /// The following:
    /// 
    /// bool move = true;
    /// 
    /// Would become...
    /// 
    /// CrossPlatformObject<bool> move = new CrossPlatformObject<bool>(true, false);
    /// 
    /// Where the first value is the value to be returned on desktop, and the second value the one to be returned in mobile.
    /// 
    /// Then, to get the value, you would use:
    /// 
    /// move.Get();
    /// 
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of object to store and return.</typeparam>
    [System.Serializable]
    public class CrossPlatformObject<T>
    {
        /// <summary>
        /// If this is checked, the desktop platform value will be used for all platforms. Usually keep this as false unless you universal functionality across platforms.
        /// </summary>
        [SerializeField] bool desktopAsUniversal = false;
        /// <summary>
        /// The value to be returned on the desktop platform.
        /// </summary>
        [SerializeField] T desktop;
        /// <summary>
        /// The value to be returned on the mobile platform.
        /// </summary>
        [SerializeField] T mobile;
        /// <summary>
        /// A default object in case another platform is used.
        /// </summary>
        private T defaultObj;

        /// <summary>
        /// Creates a new CrossPlatformObject.
        /// </summary>
        /// <param name="d">The value to be returned on desktop.</param>
        /// <param name="m">The value to be returned on mobile.</param>
        public CrossPlatformObject(T d, T m)
        {
            desktop = d;
            mobile = m;
            defaultObj = desktop;
        }
        /// <summary>
        /// Returns the object, use this when you want to read the value of the variable.
        /// It will automatically return the value depending on the platform.
        /// </summary>
        /// <returns>The value according to the current platform.</returns>
        public T Get()
        {
            if (desktopAsUniversal)
            {
                return desktop;
            }
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return desktop;
#elif UNITY_IOS || UNITY_ANDROID
            return mobile;
#else
            return defaultObj;
#endif

        }
    }
}