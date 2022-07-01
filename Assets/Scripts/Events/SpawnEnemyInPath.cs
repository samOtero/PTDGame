using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Spawn Enemy In Path")]
public class SpawnEnemyInPath : ScriptableObject, ISerializationCallbackReceiver
{

    public Func<UnitProfile, int, int> listener;

    public void Reset()
    {
        listener = null;
    }
    public void Raise(UnitProfile profile, int pathNum)
    {
        if (listener != null) listener(profile, pathNum);
    }

    public void RegisterListener(Func<UnitProfile, int, int> listener)
    {
        this.listener = listener;
    }

    public void UnregisterListener(Func<UnitProfile, int, int> listener)
    {
       if (this.listener == listener) this.listener = null;
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

