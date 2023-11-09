using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void Subscribe(IObserver observer);
    void UnSubscribe(IObserver observer);
    void OnNotify(params object[] parameters);
}
