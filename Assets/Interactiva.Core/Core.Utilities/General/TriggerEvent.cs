using UnityEngine;
using UnityEngine.Events;

namespace Interactiva.Core.Utilities
{
    /// <summary>
    /// Method invokes given Unity events when object enters trigger.
    /// </summary>
    public class TriggerEvent : MonoBehaviour
    {
        public string colliderTag = "Player";
        public UnityEvent<Collider> onTriggerEnter = new UnityEvent<Collider>();
        public UnityEvent<Collider> onTriggerExit = new UnityEvent<Collider>();
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(colliderTag))
            {
                onTriggerEnter.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(colliderTag))
            {
                onTriggerExit.Invoke(other);
            }
        }
    }
}
