using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] ObjectDatabase objectDatabase;

    [SerializeField] GameObject buttonMachinePrefab;
    [SerializeField] GameObject buttonContainer;
    //[SerializeField] Transform spawnPosition;

    private void OnEnable()
    {
        foreach (var item in objectDatabase.machines)
        {
            var prefab = Instantiate(buttonMachinePrefab, buttonContainer.transform);
            Debug.Log(item.Name);
            prefab.GetComponent<MachineButton>().machineName = item.Name;
            Debug.Log(item.ID);
            prefab.GetComponent<MachineButton>().machineIndex = item.ID;
            Debug.Log(item.Icon);
            prefab.GetComponent<MachineButton>().machineSprite = item.Icon;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
