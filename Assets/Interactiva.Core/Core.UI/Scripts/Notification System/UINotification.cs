using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// Class that holds the information of a notification.
    /// Use for universal platform support. If content differs by platform, use UINotificationCrossPlatform instead.
    /// </summary>
    [CreateAssetMenu(fileName = "Tutorial", menuName = "Interactiva/UI/Notification")]
    public class UINotification : UINotificationBase
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private bool oneShot;

        public override string Title => title;

        public override string Description => description;

        public override Sprite Icon => icon;

        public override bool OneShot => oneShot;
    }
}