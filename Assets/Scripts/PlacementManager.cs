using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    [SerializeField] GameObject cellIndicator;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;

    [SerializeField] ObjectDatabase dataBase;
    // Si es -1 no se selecciono ningún objeto
    int selectedObjectIndex = -1;

    //[SerializeField] GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
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
        cellIndicator.SetActive(true);
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
        GameObject newObject = Instantiate(dataBase.machines[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }

    // Apaga la colocación
    void StopPlacement()
    {
        selectedObjectIndex = -1;
        //gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    /// <summary>Obtiene la posición del mouse al activar el modo de colocación</summary>
    public void ObtainMouseVariables()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

}
