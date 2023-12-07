using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactiva.Core.Utilities.Save
{
    [RequireComponent(typeof(Toggle))]
    public class Save_UIToggle : BaseSavable
    {
        [SerializeField] private string key;

        private Toggle toggle;

        public override void Setup()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((bool val) =>
            {
                Save();
            });
            Load();
        }

        public override void Load()
        {
            bool toggleValue = SaveManager.GetBool(key, toggle.isOn);
            toggle.isOn = toggleValue;
            toggle.onValueChanged.Invoke(toggleValue);
        }

        public override void Save()
        {
            SaveManager.SetBool(key, toggle.isOn);
        }
    }
}