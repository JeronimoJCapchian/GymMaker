using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIBackgroundFader : MonoBehaviour
{
    public static UIBackgroundFader Instance { get; private set;}
    [SerializeField] private Image background;

    [SerializeField] private Ease easeType;

    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start() {
        FadeOut(0.5f);
    }

    public void FadeIn(float timer)
    {
        background.DOFade(1, timer).SetEase(easeType);
    }

    public void FadeOut(float timer)
    {
        background.DOFade(0, timer).SetEase(easeType);
    }
}
