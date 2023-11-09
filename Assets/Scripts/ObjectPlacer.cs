using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObject = new();

    public int PlaceObject(GameObject prefab, Vector3 position, bool isRotated)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        if(isRotated)
            newObject.transform.eulerAngles = new(0f,90f,0f);
        placedGameObject.Add(newObject);
        return placedGameObject.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObject.Count <= gameObjectIndex
            || placedGameObject[gameObjectIndex] == null)
            return;
        Destroy(placedGameObject[gameObjectIndex]);
        placedGameObject[gameObjectIndex] = null;
    }
}
