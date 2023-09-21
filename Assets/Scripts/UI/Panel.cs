using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System;

public class Panel : MonoBehaviour
{
    [SerializeField] UIHandler uIHandler;
    [Header("Propiedades")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Button openCloseButton;
    [SerializeField] TextMeshProUGUI textButton;
    [Header("Posiciones")]
    [SerializeField] Vector2 closeTransform;
    [SerializeField] Vector2 openTransform;
    [Header("Variables")]
    [Range(0.1f, 1f)] [SerializeField] float openDuration;
    [Range(0.1f, 1f)] [SerializeField] float closeDuration;
    [SerializeField] string openText;
    [SerializeField] string closeText;
    [SerializeField] Ease openEaseType;
    [SerializeField] Ease closeEaseType;
    [SerializeField] bool isOpen;

    [SerializeField] AnimationCurve pepito;

    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        uIHandler = GetComponentInParent<UIHandler>();
    }

    public void ChecksOtherPanels()
    {
        if(!isOpen)
        {
            foreach (var panel in uIHandler.allPanels)
            {
                if(panel.isOpen)
                    return;
            }
        }
        MovePanel();
    }

    public void MovePanel()
    {
        if(isOpen)
        {
            //rectTransform.anchoredPosition = closeTransform;
            rectTransform.DOAnchorPos(closeTransform, openDuration).SetEase(pepito);
            textButton.text = closeText;
            isOpen = false;
        }
        else
        {
            //rectTransform.anchoredPosition = openTransform;
            rectTransform.DOAnchorPos(openTransform, closeDuration).SetEase(pepito);
            textButton.text = openText;
            isOpen = true;
        }
    }
}
