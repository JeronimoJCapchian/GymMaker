using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactiva.Core.Utilities.Save
{
    [RequireComponent(typeof(Slider))]
    public class Save_UISlider : BaseSavable
    {
        [SerializeField] private string key;

        private Slider slider;

        public override void Setup()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener((float val) =>
            {
                Save();
            });
            Load();
        }

        public override void Load()
        {
            float sliderValue = PlayerPrefs.GetFloat(key, slider.value);
            slider.value = sliderValue;
            slider.onValueChanged.Invoke(sliderValue);
        }

        public override void Save()
        {
            PlayerPrefs.SetFloat(key, slider.value);
        }
    }
}