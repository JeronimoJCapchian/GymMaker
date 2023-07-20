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

    [Header("Movement Axis")]
    [SerializeField] Vector2 movementAxis;
    public float horizontalAxi;
    public float verticalAxi;

    [Header("Handler Actions")]
    [SerializeField] Vector2 rotationAxis;
    public float turnLeft;
    public float turnRight;

    [SerializeField] PlayerControls playerControls;


    public event Action OnClicked, OnExit;

    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Gameplay.CameraMovement.performed += i => movementAxis = i.ReadValue<Vector2>();
            playerControls.Gameplay.CameraRotation.performed += i => rotationAxis = i.ReadValue<Vector2>();

            // playerControls.Gameplay.CameraRotationLeft.performed += i => turnLeft = true;
            // playerControls.Gameplay.CameraRotationLeft.canceled += i => turnLeft = false;

            // playerControls.Gameplay.CameraRotationRight.performed += i => turnRight = true;
            // playerControls.Gameplay.CameraRotationRight.canceled += i => turnRight = false;
        }

        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleInputs()
    {
        HandleMovementAxis();
        HandleRotationAxis();
        HandleAction();
    }

    void HandleAction()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    void HandleMovementAxis()
    {
        horizontalAxi = movementAxis.x;
        verticalAxi = movementAxis.y;
    }
    
    void HandleRotationAxis()
    {
        turnLeft = rotationAxis.x;
        turnRight = rotationAxis.y;
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
