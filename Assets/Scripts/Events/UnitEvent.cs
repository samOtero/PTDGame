using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/UnitEvent")]
public class UnitEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Func<Unit, int>> listeners = new List<Func<Unit, int>>();

    public void Reset()
    {
        listeners.Clear();
    }
    public void Raise(Unit core)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i](core);
    }

    public void RegisterListener(Func<Unit, int> listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(Func<Unit, int> listener)
    {
        if (listeners.Contains(listener) == true)
            listeners.Remove(listener);
    }

    public void OnBeforeSerialize()
    {
        //nothing
    }

    public void OnAfterDeserialize()
    {
        Reset(); //clear our list before trying to serialize
    }
}

