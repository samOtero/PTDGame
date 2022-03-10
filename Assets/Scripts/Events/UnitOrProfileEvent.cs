using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/UnitOrProfileEvent")]
public class UnitOrProfileEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Func<Unit, UnitProfile, int>> listeners = new List<Func<Unit, UnitProfile, int>>();

    public void Reset()
    {
        listeners.Clear();
    }
    public void Raise(Unit unit, UnitProfile profile)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i](unit, profile);
    }

    public void RegisterListener(Func<Unit, UnitProfile, int> listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(Func<Unit, UnitProfile, int> listener)
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

