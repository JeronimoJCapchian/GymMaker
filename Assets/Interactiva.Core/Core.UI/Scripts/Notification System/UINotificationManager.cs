using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

namespace Interactiva.Core.UI
{
    public class UINotificationManager : MonoBehaviour
    {
        public static UINotificationManager singleton;

        /// <summary>
        /// Name of popup in Doozy Manager.
        /// </summary>
        [SerializeField] private string popupName = "Notification - Popup";
        /// <summary>
        /// Icon to use when UINotification icon is null.
        /// </summary>
        [SerializeField] private Sprite defaultIcon;
        /// <summary>
        /// Array containing all the notifications.
        /// </summary>
        [SerializeField] private UINotificationBase[] notifications;
        private UIPopup popup;
        private List<int> indicesUsed = new List<int>();

        private void Awake()
        {
            if (singleton == null) singleton = this;   
        }

        public void DisplayNotification(int index)
        {
            popup = UIPopupManager.GetPopup(popupName);
            CanvasScaler cs = popup.GetTargetCanvas().Canvas.GetComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(960, 640);
            if (popup == null)
            {
                return;
            }
            UINotificationBase data = notifications[index];
            if (indicesUsed.Contains(index))
            {
                return;
            }
            if (data.OneShot)
            {
                indicesUsed.Add(index);
            }
            popup.Data.SetLabelsTexts(data.Title, data.Description);
            popup.Data.SetImagesSprites(data.Icon != null ? data.Icon : defaultIcon);
            UIPopupManager.ShowPopup(popup, popup.AddToPopupQueue, false);
        }

        public void ClearPopupQueue()
        {
            UIPopupManager.ClearQueue();
        }
    }

}
