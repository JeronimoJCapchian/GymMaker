using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Menu : MonoBehaviour
{
    public CanvasGroup cg;

    private void Awake() {
        cg = GetComponent<CanvasGroup>();

        cg.DOFade(1, 1.5f).SetDelay(2f);
    }

    public void Play(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
