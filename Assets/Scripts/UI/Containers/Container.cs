//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected ObjectDatabase objectDatabase;
    [SerializeField] protected UIHandler uIHandler;
    [SerializeField] protected Transform buttonContainer;
    [SerializeField] protected GameObject buttonPrefab;

    private void Start()
    {
        gameManager = GameManager.instance;
        uIHandler = gameManager.uIHandler;
        objectDatabase = gameManager.objectDatabase;

        LoadButtons(buttonPrefab.gameObject);
    }

    public virtual void LoadButtons(GameObject buttonPrefab)
    { }

    public virtual void TurnContainer()
    {
        if(this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }
}
