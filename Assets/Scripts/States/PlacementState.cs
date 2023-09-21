using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase database;
    GridData gridData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectDatabase database,
                          GridData gridData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.machines.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            //gridVisualization.SetActive(true);
            previewSystem.StartShowingPlacementPreview(
                database.machines[selectedObjectIndex].Prefab,
                database.machines[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }

    public void EndeState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        int index = objectPlacer.PlaceObject(database.machines[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = gridData;

        // GridData selectedData = dataBase.machines[selectedObjectIndex].ID == 0 ?
        //     floorData :
        //     furnitureData;

        selectedData.AddObjectAt(gridPosition,
            database.machines[selectedObjectIndex].Size,
            database.machines[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // GridData selectedData = database.machines[selectedObjectIndex].ID == 0 ?
        //     floorData :
        //     furnitureData;

        GridData selectedData = gridData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.machines[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
