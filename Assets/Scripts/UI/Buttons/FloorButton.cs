using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : ItemButton
{
    private AudioSource audiosource;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    public override void LoadButton()
    {
        base.LoadButton();
        button.onClick.AddListener(() => GameManager.instance.floorSystem.ChangeFloor(itemIndex, Panel.meshToPaint));
        button.onClick.AddListener(() => audiosource.Play());
    }


}
