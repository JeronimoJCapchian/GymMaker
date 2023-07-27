using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image image;
    [SerializeField] Button button;

    public Sprite machineSprite;
    public string machineName;
    public int machineIndex;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

        LoadAttributes();
    }

    void LoadAttributes()
    {
        image.sprite = machineSprite;
        nameText.text = "" + machineName;
    }

    private void OnEnable()
    {
        // image.sprite = machineSprite;
        // nameText.text = machineName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
