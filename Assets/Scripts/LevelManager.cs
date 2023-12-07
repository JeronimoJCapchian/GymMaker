using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        UIBackgroundFader.Instance.FadeOut(0.75f);
    }

    public void OnChangeScene(string sceneName)
    {
        UIBackgroundFader.Instance.FadeIn(0.75f, ()=> SceneManager.LoadScene(sceneName));
    }
}
