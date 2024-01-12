using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class loads a given scene from an event or method call.
/// </summary>
public class LoadScene : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = 0;

    public void LoadSceneImmediate()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
