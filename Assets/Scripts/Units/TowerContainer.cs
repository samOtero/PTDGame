using System;
using UnityEngine;
// Holds a reference to a particular Tower
[Serializable]
public class TowerContainer: ISerializationCallbackReceiver
{
   public Unit unit;
   public UnitProfile profile;
   public bool hasBeenCreated;

   public void OnAfterDeserialize()
    {
        Reset();
    }

     public void OnBeforeSerialize()
    {
        //nothing
    }

    private void Reset() {
        unit = null;
        hasBeenCreated = false;
    }
}
