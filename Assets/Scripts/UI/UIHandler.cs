using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] ObjectDatabase objectDatabase;

    [Header("Paneles Proprieties")]
    [SerializeField] GameObject panels;
    public List<Panel> allPanels;

    [Header("Containers Proprieties")]
    [SerializeField] GameObject containers;
    [SerializeField] List<Container> machinesContainers = new();
    [SerializeField] Container currentContainer;

    [SerializeField] GameObject buttonPrefab;

    bool isMachinePanel;
    bool isFloorPanel;
    //[SerializeField] Transform spawnPosition;


    private void OnEnable()
    {
        for (var i = 0; i < containers.transform.childCount; i++)
        {
            machinesContainers.Add(containers.transform.GetChild(i).GetComponent<Container>());
            machinesContainers[i].gameObject.SetActive(false);
            currentContainer = machinesContainers[i];
        }

        for (var i = 0; i < panels.transform.childCount; i++)
        {
            allPanels.Add(panels.transform.GetChild(i).GetComponent<Panel>());
        }

        currentContainer.gameObject.SetActive(true);
    }

    #region Panel Region

    void MovePanel()
    {
        
    }

    #endregion

    #region  Container Region

    public void TurnContainer(Container container)
    {
        if(container != currentContainer)
        {
            currentContainer.gameObject.SetActive(false);
            currentContainer = container;
        }

        currentContainer.gameObject.SetActive(true);
    }

    #endregion  Containers

}
