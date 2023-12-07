using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactiva.Core.Utilities;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// Class holds the information of notifications.
    /// Supports multiple platforms.
    /// </summary>
    [CreateAssetMenu(fileName = "Notification", menuName = "Interactiva/UI/Notification Cross Platform")]
    public class UINotificationCrossPlatform : UINotificationBase
    {
        [SerializeField] private CrossPlatformObject<string> title;
        [SerializeField] private CrossPlatformObject<string> description;
        [SerializeField] private CrossPlatformObject<Sprite> icon;
        [SerializeField] private bool oneShot = false;

        public override string Title => title.Get();
        public override string Description => description.Get();
        public override Sprite Icon => icon.Get();
        public override bool OneShot => oneShot;
    }
}