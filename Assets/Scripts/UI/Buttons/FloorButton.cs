using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : ItemButton
{
    public override void LoadButton()
    {
        base.LoadButton();
        button.onClick.AddListener(() => GameManager.instance.floorSystem.ChangeFloor(itemIndex));
    }


}
