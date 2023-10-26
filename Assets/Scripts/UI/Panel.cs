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
    [Header("Posiciones")]
    [SerializeField] Vector2 closeTransform;
    [SerializeField] Vector2 openTransform;
    [Header("Variables")]
    [Range(0.1f, 1f)] [SerializeField] float openDuration;
    [Range(0.1f, 1f)] [SerializeField] float closeDuration;
    [SerializeField] GameObject openIcon;
    [SerializeField] GameObject closeIcon;
    [SerializeField] Ease openEaseType;
    [SerializeField] Ease closeEaseType;
    [SerializeField] bool isOpen;

    [SerializeField] AnimationCurve curveOfMovement;

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
            rectTransform.DOAnchorPos(closeTransform, openDuration).SetEase(curveOfMovement);
            closeIcon.SetActive(false);
            openIcon.SetActive(true);
            isOpen = false;
        }
        else
        {
            //rectTransform.anchoredPosition = openTransform;
            rectTransform.DOAnchorPos(openTransform, closeDuration).SetEase(curveOfMovement);
            closeIcon.SetActive(true);
            openIcon.SetActive(false);
            isOpen = true;
        }
    }
}
