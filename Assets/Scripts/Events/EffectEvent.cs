using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Effect Event")]
public class EffectEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public List<Func<Effect, int>> listeners = new List<Func<Effect, int>>();

    public void Reset()
    {
        listeners.Clear();
    }
    public void Raise(Effect whichEffect)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i](whichEffect);
    }

    public void RegisterListener(Func<Effect, int> listener)
    {
        if (listeners.Contains(listener) == false)
            listeners.Add(listener);
    }

    public void UnregisterListener(Func<Effect, int> listener)
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

