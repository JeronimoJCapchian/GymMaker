using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Interactiva.Core.Utilities;

namespace Interactiva.Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CrossPlatformLabel : MonoBehaviour
    {
        [SerializeField]
        private CrossPlatformObject<string> text;

        private void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = text.Get();
        }

    }
}