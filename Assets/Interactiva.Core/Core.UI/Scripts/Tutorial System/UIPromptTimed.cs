using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactiva.Core.UI
{
    [CreateAssetMenu(fileName = "Prompt", menuName = "Interactiva/UI/Timed Prompt")]
    public class UIPromptTimed : UIPrompt
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private string description;

        public override Sprite Icon => icon;
        public override string Title => title;
        public override string Description => description;
        /// <summary>
        /// Time in seconds to display this prompt.
        /// </summary>
        public float time = 3f; 
        private float timer = 0;
        
        public override void Setup()
        {
            timer = 0;
        }

        public override bool Poll()
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                return true;
            }
            return false;
        }

        public override void Finish()
        {
            
        }


    }
}