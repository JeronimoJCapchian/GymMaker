using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    [SerializeField] string openText;
    [SerializeField] string closeText;
    public bool isOpen;


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
            rectTransform.anchoredPosition = closeTransform;
            textButton.text = closeText;
            isOpen = false;
        }
        else
        {
            rectTransform.anchoredPosition = openTransform;
            textButton.text = openText;
            isOpen = true;
        }
    }
}
