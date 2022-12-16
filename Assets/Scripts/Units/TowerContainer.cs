using System;
using UnityEngine;
// Holds a reference to a particular Tower
[Serializable]
public class TowerContainer: ISerializationCallbackReceiver
{
   public Unit unit;
   public UnitProfile profile;
    public UnitProfileObj profileTemp; // Temporary for testing purposes of adding a profile straight into the party
   public bool hasBeenCreated;

    public void AddUnit(Unit unit)
    {
        profile = unit.profile;
        this.unit = unit;
        hasBeenCreated= true;
    }
    public UnitProfile getUnitProfile()
    {
        if (profile == null && profileTemp != null)
        {
            profile = new UnitProfile(profileTemp);
        }

        return profile;

    }

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
        profile = null;
    }
}
