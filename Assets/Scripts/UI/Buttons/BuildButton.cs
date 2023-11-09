using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : ItemButton
{
    public override void LoadButton()
    {
        base.LoadButton();

        button.onClick.AddListener(() => GameManager.instance.placementManager.StartPlacement(itemIndex));
    }
}
