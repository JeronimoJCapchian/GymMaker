using UnityEngine;

namespace Interactiva.Core.Utilities
{
    /// <summary>
    /// Destroys an object after 'timeToDestroy' seconds.
    /// </summary>
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy = 5f;

        private void Start()
        {
            Destroy(gameObject, timeToDestroy);
        }
    }
}

