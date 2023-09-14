using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected Image image;
    [SerializeField] protected Button button;

    public int itemIndex { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void LoadAttributes(int ID, Sprite sp, string name)
    {
        itemIndex = ID;
        image.sprite = sp;
        nameText.text = name;
    }

    public virtual void LoadButton()
    {
        
    }
}
