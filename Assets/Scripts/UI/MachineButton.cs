using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image image;
    Button button;

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

        button.onClick.AddListener(() => GameManager.instance.placementManager.StartPlacement(machineIndex));
    }

    private void OnEnable()
    {
        // image.sprite = machineSprite;
        // nameText.text = machineName;
    }

}
