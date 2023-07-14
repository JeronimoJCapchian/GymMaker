using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    [SerializeField] LayerMask placementLayerMask;
    Vector3 lastPosition;

    
    public event Action OnClicked, OnExit;

    public void HandleAction()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    /// <summary>Devuelve si el cursor esta sobre la interfaz</summary>
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    /// <summary>Obtiene la posición del mapa en según la posición del mouse cuando se ejecute</summary>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
    
}
