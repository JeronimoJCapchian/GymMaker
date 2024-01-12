using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Doozy.Engine.UI;

namespace Interactiva.Core.UI
{
    /// <summary>
    /// UI Component class that displays a custom 2-choice panel popup.
    /// </summary>
    public class UIBooleanOptionPanel : UIComponent
    {
        [SerializeField] private TextMeshProUGUI mainTitle;
        [SerializeField] private Image icon1;
        [SerializeField] private Image icon2;

        [SerializeField] private TextMeshProUGUI title1;
        [SerializeField] private TextMeshProUGUI title2;

        [SerializeField] private TextMeshProUGUI description1;
        [SerializeField] private TextMeshProUGUI description2;

        [SerializeField] private Button button1;
        [SerializeField] private Button button2;

        private UIView view;

        private void Awake()
        {
            view = GetComponent<UIView>();
        }

        public void SetBooleanOptionPanel(string title, UIInfo.Info first, UIInfo.Info second, Button.ButtonClickedEvent onClick1, Button.ButtonClickedEvent onClick2)
        {
            mainTitle.text = title;
            icon1.sprite = first.image;
            icon2.sprite = second.image;

            title1.text = first.main;
            title2.text = second.main;

            description1.text = first.secondary;
            description2.text = second.secondary;

            button1.onClick = onClick1 ?? new Button.ButtonClickedEvent();
            button2.onClick = onClick2 ?? new Button.ButtonClickedEvent();
        }

        public override bool Activate()
        {
            view.Show();
            return base.Activate();
        }

        public override void Deactivate()
        {
            view.Hide();
            base.Deactivate();
        }
    }
}