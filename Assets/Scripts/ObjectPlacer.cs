using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObject = new();
  

    public int PlaceObject(GameObject prefab, Vector3 position, Vector3 pos, Vector3 rot)
    {
        GameObject newObject = Instantiate(prefab);
        Destroy(newObject.GetComponent<TriggeringValidate>());
        StateManager.Instance.AddBoxCollider(newObject.GetComponentInChildren<BoxCollider>());
        newObject.layer = 7;
        newObject.transform.position = position;
        newObject.transform.GetChild(0).position = pos;
        newObject.transform.GetChild(0).eulerAngles = rot;
            
        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex
            || placedGameObject[gameObjectIndex] == null)
            return;

        StateManager.Instance.RemoveBoxCollider(placedGameObject[gameObjectIndex].GetComponentInChildren<BoxCollider>());        
        Destroy(placedGameObject[gameObjectIndex]);
        placedGameObject[gameObjectIndex] = null;
    }
}
