using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BooleanToMethod : MonoBehaviour
{
    [SerializeField] UnityEvent onTrue;
    [SerializeField] UnityEvent onFalse;
    public void BooleanTrigger(bool value)
    {
        if (value)
        {
            onTrue.Invoke();
        } else
        {
            onFalse.Invoke();
        }
    }
}
