using UnityEngine;

public interface IBuildingState
{
    void EndeState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}