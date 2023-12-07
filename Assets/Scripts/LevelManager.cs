using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        UIBackgroundFader.Instance.FadeOut(0.5f);
    }

    public void OnChangeScene(string sceneName)
    {
        UIBackgroundFader.Instance.FadeIn(0.5f, ()=> SceneManager.LoadScene(sceneName));
    }
}
