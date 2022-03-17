using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/IntEvent")]
public class IntEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Func<int, int>> listeners = new List<Func<int, int>>();

    public void Reset()
    {
        listeners.Clear();
    }
    public void Raise(int core)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i](core);
    }

    public void RegisterListener(Func<int, int> listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(Func<int, int> listener)
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

