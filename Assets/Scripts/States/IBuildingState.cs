using UnityEngine;

public interface IBuildingState
{
    void EndeState();
    void OnAction(Vector3Int gridPosition, Quaternion gridRot);
    void UpdateState(Vector3Int gridPosition);
}