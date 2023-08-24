using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] ObjectDatabase objectDatabase;

    [SerializeField] GameObject containers;
    [SerializeField] List<Container> machinesContainers = new();

    [SerializeField] GameObject buttonPrefab;

    
    //[SerializeField] Transform spawnPosition;


    private void OnEnable()
    {
        for (var i = 0; i < containers.transform.childCount; i++)
        {
            Debug.Log(containers.transform.GetChild(i).GetComponent<Container>());
            machinesContainers.Add(containers.transform.GetChild(i).GetComponent<Container>());
            machinesContainers[i].TurnContainer();
        }
    }

}
