using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    GameManager gameManager;
    PlacementManager placementManager;
    [SerializeField] ObjectDatabase objectDatabase;

    [Header("Paneles Proprieties")]
    [SerializeField] GameObject panels;
    public List<Panel> allPanels;

    [Header("Containers Proprieties")]
    [SerializeField] GameObject containers;
    [SerializeField] List<Container> machinesContainers = new();
    [SerializeField] Container currentContainer;

    [Header("Buttons Proprieties")]
    [SerializeField] Sprite removeIconOpen;
    [SerializeField] Sprite removeIconClose;
    [SerializeField] Image removingIcon;

    [SerializeField] GameObject buttonPrefab;

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

    private void Start()
    {
        gameManager = GameManager.instance;
        placementManager = gameManager.placementManager;

        StateManager.Instance.AddOnAction(ChangeState);
    }

    #region  Container Region

    public void TurnContainer(Container container)
    {
        if (container != currentContainer)
        {
            currentContainer.gameObject.SetActive(false);
            currentContainer = container;
        }

        currentContainer.gameObject.SetActive(true);
    }

    #endregion  Containers

    public void ChangeState(bool value)
    {
        foreach (Panel panel in allPanels)
        {
            panel.ForceClosePanel(value);
        }
    }

    #region Buttons Region

    public void UpdateButton(bool state)
    {
        // if (placementManager.isRemoving)
        //     removingIcon.sprite = removeIconOpen;
        // else
        //     removingIcon.sprite = removeIconClose;
        if (state)
            removingIcon.sprite = removeIconOpen;
        else
            removingIcon.sprite = removeIconClose;
    }

    #endregion

}
