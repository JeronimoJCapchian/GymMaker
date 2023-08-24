using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinesContainer : Container
{
    public override void LoadButtons(GameObject buttonPrefab)
    {
        foreach (var item in GameManager.instance.objectDatabase.machines)
        {
            var prefab = Instantiate(buttonPrefab, buttonContainer.transform).GetComponent<BuildButton>();

            prefab.LoadAttributes(item.ID, item.Icon, item.Name);
            prefab.LoadButton();
        }
    }
}
