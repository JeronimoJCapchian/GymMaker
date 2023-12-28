using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : ItemButton
{
    private AudioSource audiosource;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    public override void LoadButton()
    {
        base.LoadButton();

        button.onClick.AddListener(() => GameManager.instance.placementManager.StartPlacement(itemIndex));
        button.onClick.AddListener(() => audiosource.Play());
    }
}
