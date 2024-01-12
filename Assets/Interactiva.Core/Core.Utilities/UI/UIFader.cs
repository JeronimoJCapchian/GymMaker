using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Interactiva.Core.UI.Utilities
{
    /// <summary>
    /// Script fades a UI Canvas Group with given methods. Good for gradually showing/hiding a UI.
    /// </summary>
    public class UIFader : MonoBehaviour
    {
        [SerializeField] private bool disableOnAlpha0 = true;
        private CanvasGroup canvasGroup;

        private bool fading = false;
        private bool visible = false;
        public bool IsFading => fading;
        public bool Visible => visible;

        private Tween curTween = null;

            
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeIn(float time)
        {
            if (curTween != null)
            {
                curTween.Kill();
            }
            if (disableOnAlpha0)
            {
                gameObject.SetActive(true);
            }
            visible = true;
            fading = true;
            curTween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, time).OnComplete(() =>
            {
                fading = false;
            });
        }

        public void FadeOut(float time)
        {
            if (curTween != null)
            {
                curTween.Kill();
            } 
            visible = false;
            fading = true;
            curTween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, time).OnComplete(() =>
            {
                fading = false;
                if (disableOnAlpha0)
                {
                    gameObject.SetActive(false);
                }
            });
        }

        
    }
}