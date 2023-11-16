using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Contiene la información de la Grilla</summary>
public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedMachines = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (var position in positionsToOccupy)
        {
            if (placedMachines.ContainsKey(position))
                throw new Exception($"Dictionary already contains this cell position {position}");

            placedMachines[position] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (var x = 0; x < objectSize.x; x++)
        {
            for (var y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedMachines.ContainsKey(gridPosition))
            return -1;

        return placedMachines[gridPosition].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var position in placedMachines[gridPosition].occupiedPosition)
        {
            placedMachines.Remove(position);
        }
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedMachines.ContainsKey(pos))
                return false;
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    // Constructor
    public PlacementData(List<Vector3Int> occupiedPosition, int iD, int placedObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
