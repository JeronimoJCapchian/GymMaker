using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public Action<bool> changeState;

    private bool isViewportMode;

    [SerializeField] UnityEvent onEditModeOn;
    [SerializeField] UnityEvent onEditModeOff;

    private void Awake() {
        if(Instance == null) Instance = this;
    }

    public void ChangeState()
    {
        isViewportMode = !isViewportMode;

        changeState?.Invoke(isViewportMode);
        
        if(isViewportMode) onEditModeOff?.Invoke();
        else onEditModeOn?.Invoke();
    }

    public void AddOnAction(Action<bool> action)
    {
        changeState += action;
    }
}
