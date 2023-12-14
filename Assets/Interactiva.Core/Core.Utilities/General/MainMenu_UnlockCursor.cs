using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UnlockCursor : MonoBehaviour
{
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
