using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;
    [SerializeField] private GridData gridData;

    [SerializeField] private List<GameObject> placedGameObject = new();

    [SerializeField] ObjectDatabase dataBase;
    // Si es -1 no se selecciono ningún objeto
    int selectedObjectIndex = -1;

    //[SerializeField] GameObject gridVisualization;

    [SerializeField] PreviewSystem preview;

    Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();

        gridData = new();
        // Debug.Log(gridData);
        // floorData = new();
        // Debug.Log(floorData);
        // furnitureData = new();
        // Debug.Log(furnitureData);
    }

    // Activa la colocación de un objeto según la id que se le pase
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = dataBase.machines.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogWarning($"No ID Found{ID}");
            return;
        }
        //gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(
            dataBase.machines[selectedObjectIndex].Prefab,
            dataBase.machines[selectedObjectIndex].Size);

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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(dataBase.machines[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);

        GridData selectedData = gridData;

        // GridData selectedData = dataBase.machines[selectedObjectIndex].ID == 0 ?
        //     floorData :
        //     furnitureData;

        selectedData.AddObjectAt(gridPosition,
            dataBase.machines[selectedObjectIndex].Size,
            dataBase.machines[selectedObjectIndex].ID,
            placedGameObject.Count - 1);

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // GridData selectedData = dataBase.machines[selectedObjectIndex].ID == 0 ?
        //     floorData :
        //     furnitureData;

        GridData selectedData = gridData;

        return selectedData.CanPlaceObjectAt(gridPosition, dataBase.machines[selectedObjectIndex].Size);
    }

    // Apaga la colocación
    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        //gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    /// <summary>Obtiene la posición del mouse al activar el modo de colocación</summary>
    public void ObtainMouseVariables()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }


        // if (placementValidity == false)
        //     return;

    }

}
