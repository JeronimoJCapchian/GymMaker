using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    [CreateAssetMenu(fileName = "Tutorial", menuName = "Interactiva/UI/Tutorial")]
    public class UITutorial : UITutorialBase
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private string description;

        public override string Title { get => title; }
        public override string Description { get => description;}

        public override Sprite Icon => icon;
    }
}