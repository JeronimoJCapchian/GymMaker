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
    [SerializeField] GameObject openGameObject;
    [SerializeField] GameObject closeGameObject;
    [SerializeField] Ease openEaseType;
    [SerializeField] Ease closeEaseType;
    [SerializeField] bool isOpen;

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
                    panel.MovePanel();
            }
        }
        MovePanel();
    }

    public void MovePanel()
    {
        if(isOpen)
        {
            // Close
            rectTransform.DOAnchorPos(closeTransform, openDuration).SetEase(closeEaseType);
            openGameObject.SetActive(true);
            closeGameObject.SetActive(false);
            isOpen = false;
        }
        else
        {
            // Open
            rectTransform.DOAnchorPos(openTransform, closeDuration).SetEase(openEaseType);
            openGameObject.SetActive(false);
            closeGameObject.SetActive(true);
            isOpen = true;
        }
    }
}
