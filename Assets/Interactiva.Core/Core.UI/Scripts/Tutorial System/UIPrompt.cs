using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactiva.Core.Utilities;

namespace Interactiva.Core.UI
{
    public abstract class UIPrompt : ScriptableObject
    {
        public virtual string Title { get;  }
        public virtual string Description { get; }
        public virtual Sprite Icon { get; }
        /// <summary>
        /// Determines whether the background semi-transparent overlay is shown with this tutorial.
        /// </summary>
        public CrossPlatformObject<bool> showOverlay = new CrossPlatformObject<bool>(true, false);
        
        /// <summary>
        /// Method is executed when this prompt is triggered.
        /// </summary>
        public abstract void Setup();
        /// <summary>
        /// Method is exeucuted continuously while this prompt is active. It will continue as long as it returns false.
        /// Returnign true is a signal that this prompt has been completed / acknowledged.
        /// </summary>
        /// <returns>Whether this prompt has completed.</returns>
        public abstract bool Poll();
        /// <summary>
        /// Method is executed once after polling has completed succesfully. Used for invoking methods.
        /// </summary>
        public abstract void Finish();
    }
}
