using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// Base class for all UI Components.
    /// 
    /// A UI Component is any UI panel or element that can be activated, like a window, a map, a pause menu, etc.
    /// </summary>
    public class UIComponent : MonoBehaviour, IComparable<UIComponent>
    {
        [Tooltip("Priority indicates how important this component is relative to others. If the component has the highest priority of all active components, it will be displayed." +
            "If it has a lower priority, it will be blocked.")]
        [SerializeField] private int priority = 0;
        
        [Tooltip("When checked, this will queue this component until its priority is the highest, and then it will be displayed.")]
        [SerializeField] private bool queueIfConflict = false;
        [Tooltip("When checked, this will force deactivate all active components with lower priority when this component is activated.")]
        [SerializeField] private bool deactivateAll = false;
        [Tooltip("When checked, this will prevent the player from interacting with the POI system.")]
        [SerializeField] private bool freezeInteraction = true;
        [SerializeField] private UnityEvent onActivate;
        [SerializeField] private UnityEvent onDeactivate;

        [SerializeField] private UnityEvent onCanActivateTrue;
        [SerializeField] private UnityEvent onCanActivateFalse;

        /// <summary>
        /// Variable saves whether we can activate this UIComponent.
        /// </summary>
        protected bool canActivate = true;

        /// <summary>
        /// Property returns whether this UIComponent can be activated.
        /// You can set this to false to stop this element from being toggled or displayed.
        /// </summary>
        public virtual bool CanActivate
        {
            get { return canActivate; }
            set
            {
                if (value)
                {
                    onCanActivateTrue.Invoke();
                }
                else
                {
                    onCanActivateFalse.Invoke();
                }
                canActivate = value;
            }
        }

        public virtual bool FreezeInteraction
        {
            get { return freezeInteraction; }
        }
        public int GetPriority()
        {
            return priority;
        }

        public bool CanQueue()
        {
            return queueIfConflict;
        }

        public bool DeactivateAll()
        {
            return deactivateAll;
        }
        /// <summary>
        /// Activate this UIComponent. In most cases you will want to use the UIManager to do this.
        /// Don't execute this methods directly unless you know exactly what you are doing.
        /// </summary>
        public virtual bool Activate()
        {
            if (CanActivate)
            {
                onActivate.Invoke();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deactivate this UIComponent. In most cases you will want to use the UIManager to do this.
        /// </summary>
        public virtual void Deactivate()
        {
            onDeactivate.Invoke();
        }
        /// <summary>
        /// Method toggles this UI Component. It does it by checking the UIManager, so you can execute this directly.
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InputToggleComponent(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                UIManager.singleton.ToggleComponent(this);
            }
        }
        /// <summary>
        /// Method implemented from IComparable interface.
        /// R
        /// </summary>
        /// <param name="other">UIComponent to compare.</param>
        /// <returns>A negative value if this is smaller than other. 0 if they are equal. and positive otherwise.</returns>
        public int CompareTo(UIComponent other)
        {
            return GetPriority() - other.GetPriority();
        }
    }
}
