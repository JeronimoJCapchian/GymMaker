using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    public InputManager inputManager;
    public PlacementManager placementManager;
    public FloorSystem floorSystem;
    public CameraManager cameraManager;
    public UIHandler uIHandler;

    public ObjectDatabase objectDatabase;

    bool canMove = true;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }

    private void Start() {
        StateManager.Instance.AddOnAction(ChangeState);
    }

    void ChangeState(bool value)
    {
        canMove = !value;
    }

    // Ejecución de los controles
    void Update()
    {
        if(!canMove) return;

        inputManager.HandleInputs();
        placementManager.ObtainMouseVariables();
    }

    private void FixedUpdate()
    {
        cameraManager.HandleMovements();
    }
}
