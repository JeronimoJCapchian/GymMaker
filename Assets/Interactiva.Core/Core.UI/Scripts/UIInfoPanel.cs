using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Interactiva.Core.UI
{
    public class UIInfoPanel : UIComponent
    {
        public static UIInfoPanel singleton;
        [SerializeField] private Image imageUI;
        [SerializeField] private TextMeshProUGUI titleUI;
        [SerializeField] private TextMeshProUGUI descriptionUI;
        [SerializeField] UnityEvent onCanTriggerTrue;
        [SerializeField] UnityEvent onCanTriggerFalse;

        private UIView view;
        

        private void Awake()
        {
            singleton = this;
            view = GetComponentInChildren<UIView>();
            CanActivate = false;
        }

        

        public override bool Activate()
        {
            if (CanActivate)
            {
                view.Show();
            }
            return base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            view.Hide();
        }

        public void SetInfoPanel(UIInfo.Info info)
        {
            imageUI.sprite = info.image;
            titleUI.text = info.main;
            descriptionUI.text = info.secondary;
        }
    }
}