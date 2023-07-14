using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Managers")]
    [SerializeField] InputManager inputManager;
    [SerializeField] PlacementManager placementManager;

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
        inputManager.HandleAction();
        placementManager.ObtainMouseVariables();
    }
}
