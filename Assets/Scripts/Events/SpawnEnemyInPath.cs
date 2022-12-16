using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Spawn Enemy In Path")]
public class SpawnEnemyInPath : ScriptableObject, ISerializationCallbackReceiver
{

    public Func<UnitProfileObj, int, int> listener;

    public void Reset()
    {
        listener = null;
    }
    public void Raise(UnitProfileObj profile, int pathNum)
    {
        if (listener != null) listener(profile, pathNum);
    }

    public void RegisterListener(Func<UnitProfileObj, int, int> listener)
    {
        this.listener = listener;
    }

    public void UnregisterListener(Func<UnitProfileObj, int, int> listener)
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

