using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interactiva.Core.UI.Utilities
{
    public class BooleanOptionTrigger : MonoBehaviour
    {
        [SerializeField] private string mainTitle;
        [SerializeField] private UIInfo.Info button1;
        [SerializeField] private UIInfo.Info button2;

        [SerializeField] private Button.ButtonClickedEvent onClick1;
        [SerializeField] private Button.ButtonClickedEvent onClick2;

        [SerializeField] private UIBooleanOptionPanel panel;

        private void Start()
        {
            if (panel == null)
            {
                panel = FindObjectOfType<UIBooleanOptionPanel>();
            }
        }

        public void OnTrigger()
        {
            if (UIManager.singleton.ActivateComponent(panel))
            {
                panel.SetBooleanOptionPanel(mainTitle, button1, button2, onClick1, onClick2);
                onClick1.AddListener(() =>
                {
                    UIManager.singleton.DeactivateComponent(panel);
                });
                onClick2.AddListener(() =>
                {
                    UIManager.singleton.DeactivateComponent(panel);
                });
            }
            
        }
    }
}
