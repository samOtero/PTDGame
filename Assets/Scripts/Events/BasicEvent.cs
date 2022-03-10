using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/BasicEvent")]
public class BasicEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Func<int>> listeners = new List<Func<int>>();

    public void Reset()
    {
        listeners.Clear();
    }
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i]();
    }

    public void RegisterListener(Func<int> listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(Func<int> listener)
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

