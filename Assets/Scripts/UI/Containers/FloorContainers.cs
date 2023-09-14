using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorContainers : Container
{
    public override void LoadButtons(GameObject buttonPrefab)
    {
        foreach (var item in GameManager.instance.objectDatabase.floors)
        {
            var prefab = Instantiate(buttonPrefab, buttonContainer.transform).GetComponent<FloorButton>();

            prefab.LoadAttributes(item.ID, item.Icon, item.Name);
            prefab.LoadButton();
        }
    }
    public override void TurnContainer()
    {
        //base.TurnContainer();
    }
}