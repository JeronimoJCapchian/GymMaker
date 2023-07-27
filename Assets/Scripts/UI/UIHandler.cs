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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in objectDatabase.machines)
        {
            var prefab = Instantiate(buttonMachinePrefab, buttonContainer.transform).GetComponent<MachineButton>();

            prefab.LoadAttributes(item.ID, item.Icon, item.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
