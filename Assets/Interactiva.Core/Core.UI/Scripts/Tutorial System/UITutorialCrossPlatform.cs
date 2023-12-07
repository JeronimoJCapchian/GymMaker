using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactiva.Core.Utilities;

namespace Interactiva.Core.UI
{
    [CreateAssetMenu(fileName = "Tutorial", menuName = "Interactiva/UI/Tutorial Cross Platform")]
    public class UITutorialCrossPlatform : UITutorialBase
    {
        [SerializeField] private CrossPlatformObject<Sprite> icon;
        [SerializeField] private CrossPlatformObject<string> title;
        [SerializeField] private CrossPlatformObject<string> description;
        public override string Description { get { return description.Get(); } }
        public override string Title { get { return title.Get(); } }

        public override Sprite Icon { get { return icon.Get(); } }
    }
}
