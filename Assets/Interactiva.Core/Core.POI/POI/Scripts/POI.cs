using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Outline;
using UnityEngine.Events;


namespace Interactiva.Core.POIs
{
    /// <summary>
    /// Base class for all POI objects. POI stands for Point Of Interest.
    /// A point of Interest is basically anything that the player can interact with.
    /// Please note that you will have to implement your own functionality in derived classes. This script should not be modified.
    /// 
    /// To set up a POI, simply attach a trigger to the game object this script is attached to.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class POI : MonoBehaviour
    {
        /// <summary>
        /// Whether this object can show the highlight effect when focusable.
        /// </summary>
        [SerializeField]
        protected bool canHighlight = true;
        /// <summary>
        /// If checked, lookAtPivot = true
        /// </summary>
        [SerializeField]
        protected bool lookAtPivot = true;
        /// <summary>
        /// UnityEvent is triggered when this POI is interacted with.
        /// </summary>
        [SerializeField]
        protected UnityEvent onInteract;

        /// <summary>
        /// Returns whether this POI can be interacted with.
        /// </summary>
        public abstract bool CanInteract { get; }
        /// <summary>
        /// Returns the prompt string to display to the player when focusable.
        /// </summary>
        public abstract string PromptText { get; }
        /// <summary>
        /// Returns whether the prompt to display should be hidden as soon as we interact.
        /// NOTE: You might want to implement this with a CrossPlatformObject if this object is a IPOIToggle
        /// This is because in mobile the prompt is used as the input handler as well.
        /// </summary>
        public abstract bool HidePromptOnInteract { get; }

        public bool LookAtPivot { get => lookAtPivot; set => lookAtPivot = value; }

        /// <summary>
        /// Reference to the OutlineLayerCollection.
        /// This is used to create the outline effect on objects when they are focusable.
        /// </summary>
        protected static OutlineLayerCollection olc;

        protected virtual void Awake()
        {
            if (olc == null)
            {
                olc = Resources.LoadAll<OutlineLayerCollection>("")[0];
            }
        }
        /// <summary>
        /// Interact is executed whenever the player "Interacts" with this POI, via the FocusComponent.
        /// </summary>
        public virtual void Interact()
        {
            onInteract.Invoke();
        }


        /// <summary>
        /// Method is triggered to showcase the highlight effect whenever the player is focused.
        /// </summary>
        public virtual void EnableHighlight()
        {
            if (canHighlight)
            {
                if (!olc.GetOrAddLayer(0).Contains(gameObject))
                {
                    olc.GetOrAddLayer(0).Add(gameObject);
                }
            }
        }

        /// <summary>
        /// Method is triggered when the player can no longer focus on this object.
        /// </summary>
        public virtual void DisableHighlight()
        {
            if (canHighlight)
            {
                olc.GetOrAddLayer(0).Remove(gameObject);
            }
        }

        /// <summary>
        /// Method returns the position of this POI.
        /// The position is usually the pivot of the transform.
        /// It can be overriden to provide custom behavior.
        /// </summary>
        /// <returns>The position of this POI</returns>
        public virtual Vector3 GetPosition()
        {
            return transform.position;
        }

        /// <summary>
        /// OnTriggerEnter is executed when the user enters the trigger of this POI.
        /// Triggers are used to detect when a user in nearby.
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                POIManager.AddFocusablePOI(this);
                
            }
        }
        /// <summary>
        /// OnTriggerExit is executed when the user enters the trigger of this POI.
        /// Triggers are used to detect when a user in nearby.
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                POIManager.RemoveFocusablePOI(this);
            }
        }
    }
}