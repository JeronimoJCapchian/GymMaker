using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    public InputManager inputManager;
    public PlacementManager placementManager;
    public CameraManager cameraManager;
    public UIHandler uIHandler;

    public ObjectDatabase objectDatabase;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Ejecución de los controles
    void Update()
    {
        inputManager.HandleInputs();
        placementManager.ObtainMouseVariables();
    }

    private void FixedUpdate()
    {
        cameraManager.HandleMovements();
    }
}
