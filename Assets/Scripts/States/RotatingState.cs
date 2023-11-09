using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingState : IBuildingState
{

    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabase database;
    GridData gridData;
    ObjectPlacer objectPlacer;

    public RotatingState(int iD,
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
    }

    public void OnAction(Vector3Int gridPosition)
    {
        
    }

    public void EndeState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        throw new System.NotImplementedException();
    }
}
