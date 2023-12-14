using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.Utilities
{
    public class Enabler : MonoBehaviour
    {
        [SerializeField] GameObject gameobject;
        void Update()
        {
            if (!gameobject.activeSelf)
            {
                gameobject.SetActive(true);
                Destroy(this);
            }
        }
    }
}
