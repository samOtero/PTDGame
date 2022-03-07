using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/CreateAttack")]
public class CreateAttackEvent : ScriptableObject, ISerializationCallbackReceiver
{

    public Func<Unit, AttackID, Attack> listener;

    public void Reset()
    {
        listener = null;
    }
    public Attack Raise(Unit unit, AttackID whichAttack)
    {
        if (listener == null) {
            return null;
        }

        return listener(unit, whichAttack);
    }

    public void RegisterListener(Func<Unit, AttackID, Attack> listener)
    {
        this.listener = listener;
    }

    public void UnregisterListener(Func<Unit, AttackID, Attack> listener)
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

