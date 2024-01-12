using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState, IObserver
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase database;
    GridData gridData;
    ObjectPlacer objectPlacer;

    Vector2Int originSize;

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

        GameManager.instance.inputManager.Subscribe(this);
        previewSystem.isRotated = false;

        originSize = database.machines[selectedObjectIndex].Size;
    }

    public void EndeState()
    {
        previewSystem.StopShowingPreview();
        GameManager.instance.inputManager.UnSubscribe(this);
        database.machines[selectedObjectIndex].Size = originSize;
        previewSystem.isRotated = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.inputManager.UnSubscribe(this);
    }

    public void OnAction(Vector3Int gridPosition, Quaternion gridRot)
    {

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false || previewSystem.PreviewObject.GetComponent<TriggeringValidate>().validity == false)
        {
            AudioSoundManager.Instance.PlaySound(2);
            return;
        }
            

        int index = objectPlacer.PlaceObject(database.machines[selectedObjectIndex].Prefab,
        previewSystem.PreviewObject.transform.position,
        previewSystem.PreviewObject.transform.GetChild(0).position,
        previewSystem.PreviewObject.transform.GetChild(0).eulerAngles);

        GridData selectedData = gridData;

        // GridData selectedData = dataBase.machines[selectedObjectIndex].ID == 0 ?
        //     floorData :
        //     furnitureData;

        selectedData.AddObjectAt(gridPosition,
            database.machines[selectedObjectIndex].Size,
            database.machines[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);

        AudioSoundManager.Instance.PlaySound(0);
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

    public void Notify(IObservable observable)
    {
        previewSystem.RotateObject();
        ChangeSize();
    }

    void ChangeSize()
    {
        var x = database.machines[selectedObjectIndex].Size.x;

        database.machines[selectedObjectIndex].Size.x = database.machines[selectedObjectIndex].Size.y;
        database.machines[selectedObjectIndex].Size.y = x;
    }
}
