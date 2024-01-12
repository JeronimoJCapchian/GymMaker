using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Utilities.Save
{
    /// <summary>
    /// Class loads all Savables on start.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private bool clearOnStart = false;
        void Start()
        {
            
            if (clearOnStart)
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("All Player Prefs deleted!");
            }
            BaseSavable[] savables = FindObjectsOfType<BaseSavable>(true);
            for (int i = 0; i < savables.Length; i++)
            {
                savables[i].Setup();
            }
        }
        /// <summary>
        /// Method saves a bool on disk.
        /// </summary>
        /// <param name="key">Key to store bool</param>
        /// <param name="value">Value to store under key</param>
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) >= 1 ? true : false;
        }
    }
}
