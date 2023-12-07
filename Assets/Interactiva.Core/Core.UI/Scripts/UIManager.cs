using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// The UI Manager class provides a framework to handle multiple UI 'components' at the same time. This is useful to make sure UI components don't overlap each other or conflict.
    /// Each component is defined by a UIComponent class which can be extended to provide further functionality.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager singleton;

        private bool frozen = false;

        private Dictionary<string, UIComponent> allComponents = new Dictionary<string, UIComponent>();
        /// <summary>
        /// All the UI Components that are displayed.
        /// </summary>
        private List<UIComponent> activeComponents = new List<UIComponent>();
        /// <summary>
        /// Component Queue is used to display UI elements that were previously blocked due to priority.
        /// </summary>
        private Queue<UIComponent> componentQueue = new Queue<UIComponent>();

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            } else
            {
                Debug.LogWarning("Singleton already exists. Did you place more than one UIManager in the scene?");
            }
            UIComponent[] tempComps = FindObjectsOfType<UIComponent>();
            foreach (UIComponent c in tempComps)
            {
                allComponents.Add(c.gameObject.name, c);
            }
        }

        private void Update()
        {
            if (componentQueue.Count > 0)
            {
                if (componentQueue.Peek().GetPriority() > GetHighestPriority())
                {
                    SetComponentActive(componentQueue.Dequeue());
                }
            }
        }

        #region Public Methods
        /// <summary>
        /// Method sets the UI freeze state. If the UI is frozen, no UI Components will be able to change state.
        /// </summary>
        /// <param name="state">True = frozen; false = unfrozen.</param>
        public void SetFreezeState(bool state)
        {
            frozen = state;
        }
        /// <summary>
        /// Returns whether the UI is frozen.
        /// </summary>
        /// <returns></returns>
        public static bool IsFrozen()
        {
            return singleton.frozen;
        }
        /// <summary>
        /// Method returns whether there are any active components
        /// </summary>
        /// <returns>True if there are active components. Otherwise false.</returns>
        public bool HasActiveComponents()
        {
            return activeComponents.Count > 0;
        }
        /// <summary>
        /// Method returns whether there are any UIComponents that can freeze interaction.
        /// Similar to HasActiveComponents(), but disregards UIComponents that don't freeze the interaction.
        /// </summary>
        /// <returns>Whether any of the activeComponents can Freeze the interaction of the POI system.</returns>
        public bool CanFreezeInteraction()
        {
            for (int i = 0; i < activeComponents.Count; i++)
            {
                if (activeComponents[i].FreezeInteraction)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Method returns whether the given component is active.
        /// </summary>
        /// <param name="component">The component to check for.</param>
        /// <returns>True if the component is active, false otherwise.</returns>
        public bool IsComponentActive(UIComponent component)
        {
            return activeComponents.Contains(component);
        }
        /// <summary>
        /// Void used to Unity Events. Activates a given UIComponent.
        /// </summary>
        /// <param name="component"></param>
        public void ActivateComponentEvent(UIComponent component)
        {
            ActivateComponent(component);
        }

        /// <summary>
        /// Method activates a UI component by the game object name it is attached to.
        /// </summary>
        /// <param name="name">THe name of the component to activate.</param>
        public bool ActivateComponentByName(string name)
        {
            bool f = ActivateComponent(allComponents[name]);
            if (!f)
            {
                Debug.Log("Could not find component. Did you input the name correctly?");
            }
            return f;
            
        }
        /// <summary>
        /// Activate UIComponent by given component;
        /// </summary>
        /// <param name="component">The reference to the UI Component.</param>
        /// <returns>True if activated, false if not.</returns>
        public bool ActivateComponent(UIComponent component)
        {
            if (component == null || frozen)
            {
                return false;
            }
            int highestPriority = GetHighestPriority();
            if (highestPriority == -1)
            {
                //If there are not active components
                return SetComponentActive(component);
            }
            if (component.GetPriority() > highestPriority)
            {
                return SetComponentActive(component);
            } else
            {
                if (component.CanQueue())
                {
                    componentQueue.Enqueue(component);
                    return true;
                } else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Method deactivates a component by the given GameObject name it is attached to.
        /// </summary>
        /// <param name="name">The name of the GameObject this UIComponent is attached to.</param>
        public void DeactivateComponentByName(string name)
        {
            DeactivateComponent(allComponents[name]);
        }
        /// <summary>
        /// Method deactivates a given UIComponent.
        /// </summary>
        /// <param name="component"></param>
        public void DeactivateComponent(UIComponent component)
        {
            if (component == null || frozen)
            {
                return;
            }
            if (activeComponents.Contains(component))
            {
                if (component.GetPriority() == GetHighestPriority())
                {
                    component.Deactivate();
                    activeComponents.Remove(component);
                }
            }
            else
            {
                Debug.Log("Tried to disable an inactive UI component.");
            }
        }
        /// <summary>
        /// Method force deactivates a given component ignoring checks.
        /// </summary>
        /// <param name="component"></param>
        public void ForceDeactivateComponent(UIComponent component)
        {
            component.Deactivate();
            activeComponents.Remove(component);
        }

        /// <summary>
        /// Method toggled a given UIComponent. If it is active, it will be deactivated, and vice versa.
        /// </summary>
        /// <param name="component">The component to toggle.</param>
        public void ToggleComponent(UIComponent component)
        {
            if (activeComponents.Contains(component))
            {
                DeactivateComponent(component);
            }
            else
            {
                ActivateComponent(component);
            }
        }

        #endregion






        #region Private Methods
        /// <summary>
        /// Method returns the highest priority of all active components.
        /// </summary>
        /// <returns>Highest priority. If no object exists, it returns -1.</returns>
        private int GetHighestPriority()
        {
            return HasActiveComponents() ? activeComponents[activeComponents.Count - 1].GetPriority() : -1;
        }
        /// <summary>
        /// Method sets a component as active. Used internally only for factoring code.
        /// </summary>
        /// <param name="component"></param>
        private bool SetComponentActive(UIComponent component)
        {
            if (!component.Activate())
            {
                return false;
            }
            if (component.DeactivateAll())
            {
                for (int i = 0; i < activeComponents.Count; i++)
                {
                    DeactivateComponent(activeComponents[i]);
                }
            }
            activeComponents.Add(component);
            return true;
        }
        #endregion
    }
}
