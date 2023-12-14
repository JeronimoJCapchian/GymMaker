using UnityEngine;

namespace Interactiva.Core.Utilities.EventFilters
{
    public class ComponentDestroyerFilter : BaseEventFilter
    {
        public Component[] components;

        public override void OnTrigger()
        {
            foreach (Component c in components)
            {
                Destroy(c);
            }
            onTrigger.Invoke();
        }
    }
}
