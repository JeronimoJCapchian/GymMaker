using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    UIHandler uIHandler;
    [SerializeField] protected Transform buttonContainer;
    [SerializeField] protected GameObject buttonPrefab;

    private void Start()
    {
        uIHandler = GameManager.instance.uIHandler;
        LoadButtons(buttonPrefab.gameObject);
    }

    public virtual void LoadButtons(GameObject buttonPrefab)
    { }

    public void TurnContainer()
    {
        if(this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }
}
