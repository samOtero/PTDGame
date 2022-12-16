using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/CreateUnit")]
public class CreateUnitEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public Func<UnitProfile, TDUnitTypes, int, Unit> listener;

    public void Reset()
    {
        listener = null;
    }

    // partyPosition only for towers
    public Unit Raise(UnitProfile unitProfile, TDUnitTypes unitType, int partyPosition=-1)
    {
        if (listener == null)
        {
            return null;
        }

        return listener(unitProfile, unitType, partyPosition);
    }

    public void RegisterListener(Func<UnitProfile, TDUnitTypes, int, Unit> listener)
    {
        this.listener = listener;
    }

    public void UnregisterListener(Func<UnitProfile, TDUnitTypes, int, Unit> listener)
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


