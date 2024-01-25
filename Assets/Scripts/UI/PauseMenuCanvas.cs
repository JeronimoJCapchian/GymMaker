using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseMenuCanvas : MonoBehaviour
{

    public static PauseMenuCanvas Instance { get; private set; }
    public bool isOpen;

    Transform content;

    public UnityEvent onOpenMenu;
    public UnityEvent onCloseMenu;

    [SerializeField] AudioMixer audioM;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        isOpen = false;

        content = transform.GetChild(0);
    }

    public void OpenCloseInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            OpenClose();
        }
    }

    public void OpenClose()
    {
        isOpen = !isOpen;

        if (isOpen) onOpenMenu?.Invoke();
        else onCloseMenu?.Invoke();

        content.gameObject.SetActive(isOpen);
    }

    public void MuteSound(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);

        if (obj.activeSelf) audioM.SetFloat("sfx", MathF.Log10(0.0000001f) * 20);
        else audioM.SetFloat("sfx", MathF.Log10(1) * 20);

    }

    public void MuteMusic(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);

        if (obj.activeSelf) audioM.SetFloat("music", MathF.Log10(0.0000001f) * 20);
        else audioM.SetFloat("music", MathF.Log10(1) * 20);
    }

    public void AppQuit()
    {
        Application.Quit();
    }
}
