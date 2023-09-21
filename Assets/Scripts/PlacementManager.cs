using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] UIHandler uIHandler;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField] private GridData gridData;

    //[SerializeField] private List<GameObject> placedGameObject = new();
    [SerializeField] private ObjectPlacer objectPlacer;

    [SerializeField] ObjectDatabase dataBase;


    //[SerializeField] GameObject gridVisualization;

    [SerializeField] PreviewSystem preview;

    Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();

        gridData = new();
    }

    // Activa la colocación de un objeto según la id que se le pase
    public void StartPlacement(int ID)
    {
        StopPlacement();
        //gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           dataBase,
                                           gridData,
                                           objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }


    public void StartRemoving()
    {
        StopPlacement();
        //gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, gridData, objectPlacer);
        uIHandler.UpdateButton(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    // Coloca el objeto en la posición del Mapa
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     // GridData selectedData = dataBase.machines[selectedObjectIndex].ID == 0 ?
    //     //     floorData :
    //     //     furnitureData;

    //     GridData selectedData = gridData;

    //     return selectedData.CanPlaceObjectAt(gridPosition, dataBase.machines[selectedObjectIndex].Size);
    // }

    // Apaga la colocación
    public void StopPlacement()
    {
        if (buildingState == null)
            return;
        //gridVisualization.SetActive(false);
        buildingState.EndeState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        uIHandler.UpdateButton(false);
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    /// <summary>Obtiene la posición del mouse al activar el modo de colocación</summary>
    public void ObtainMouseVariables()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }


        // if (placementValidity == false)
        //     return;

    }

}
