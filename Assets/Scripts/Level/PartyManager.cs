using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public UnitParty currentParty;
    public IntEvent AddedToParty;
    public UnitEvent UnitCapturedEvent;
    public BaseUnit unitFunc;
    // Start is called before the first frame update
    void Start()
    {
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
        init();
    }

    private void init() {
       Debug.Log("initializing party...");
       
       // Loop through all the party spots and create a unit for each one
       for(var i=0; i < currentParty.party.Count; i++) {
            var container = currentParty.party[i];
            if (container.unit) Destroy(container.unit);
            if (container.profile == null) continue; // skip if we have no profile on this spot
            container.unit = unitFunc.CreateTower(container.profile, unitFunc.TowerTemplate, i);
            container.hasBeenCreated = true;
       }

       Debug.Log("party initialized");
    }

    // Called when a unit is captured, will try to add to party
    public int onUnitCaptured(Unit unit) {

        // Hardcoding this for now, later will extract from actual unit
        var newProfile = new UnitProfile();
        newProfile.unitID = 19;
        newProfile.nickname = "Ratty";
        newProfile.attack1ID = AttackID.BASIC_PROJECTILE;
        newProfile.lvl = 1;
        newProfile.baseHP = 10;
        newProfile.baseSpeed = 7;
        // Try to add to our party
        addCapturedUnitToParty(newProfile);
        return 0;
    }


    private void addCapturedUnitToParty(UnitProfile profile) {
       // Check to see if we have space in our party
       var emptySpot = getEmptySpotInParty();
       if (emptySpot == -1) {
           Debug.Log("No space in party for new unit");
           return;
       }
       // Create a new unit and add it to our party
       var container = currentParty.party[emptySpot];
       container.profile = profile;
       container.unit = unitFunc.CreateTower(profile, unitFunc.TowerTemplate, emptySpot);
       container.hasBeenCreated = true;

       // Send event that we have added to party
       AddedToParty.Raise(emptySpot);
    }

    private int getEmptySpotInParty() {
        for(var i=0; i < currentParty.party.Count; i++) {
            var container = currentParty.party[i];
            if (container.hasBeenCreated == false) return i;
        }
        return -1;
    }
}
