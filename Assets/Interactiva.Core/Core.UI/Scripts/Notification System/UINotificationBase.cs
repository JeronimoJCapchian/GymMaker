using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// UI Notification is the base class that contains the information for a notification.
    /// </summary>
    public abstract class UINotificationBase : ScriptableObject
    {
        /// <summary>
        /// The title of this notification.
        /// </summary>
        public virtual string Title { get; }
        /// <summary>
        /// The description is the smaller text shown under the title.
        /// </summary>
        public virtual string Description { get; }
        /// <summary>
        /// The icon is shown to one side of the title and description.
        /// </summary>
        public virtual Sprite Icon { get; }
        /// <summary>
        /// When true this notification will only fire once even if it is triggered multiple times.
        /// </summary>
        public virtual bool OneShot { get; } = false;
    }
}