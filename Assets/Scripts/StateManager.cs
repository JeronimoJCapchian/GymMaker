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

    List<BoxCollider> colliders =new List<BoxCollider>();

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

    public void AddBoxCollider(BoxCollider x)
    {
        colliders.Add(x);
    }

    public void RemoveBoxCollider(BoxCollider x)
    {
        colliders.Remove(x);
    }

    public void EnableDisable(bool value)
    {
        foreach (var item in colliders)
        {
            item.enabled = value;
        }
    }
}
