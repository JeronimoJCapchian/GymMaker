using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all Settings Save classes.
/// </summary>
namespace Interactiva.Core.Utilities.Save
{
    public abstract class BaseSavable : MonoBehaviour
    {
        /// <summary>
        /// Method called by SaveManager On Start for loading and setting up the savable.
        /// </summary>
        public abstract void Setup();
        /// <summary>
        /// Method saves the Savable information to disk.
        /// </summary>
        public abstract void Save();
        /// <summary>
        /// Method loads the savable information to the simulation.
        /// </summary>
        public abstract void Load();
    }
}