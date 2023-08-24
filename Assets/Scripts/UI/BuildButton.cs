using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected Image image;
    protected Button button;

    public int machineIndex { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void LoadAttributes(int ID, Sprite sp, string name)
    {
        machineIndex = ID;
        image.sprite = sp;
        nameText.text = name;
    }

    public virtual void LoadButton()
    {
        
    }
}
