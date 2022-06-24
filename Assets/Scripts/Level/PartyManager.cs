using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject TowerTemplate;
    public UnitParty currentParty;
    public IntEvent AddedToParty;
    public UnitEvent UnitCapturedEvent;
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
            container.unit = CreateTower(container.profile, i);
            container.hasBeenCreated = true;
       }

       Debug.Log("party initialized");
    }

    // Called when a unit is captured, will try to add to party
    public int onUnitCaptured(Unit unit) {

        // Hardcoding this for now, later will extract from actual unit
        var newProfile = new UnitProfile();
        newProfile.unitID = 20;
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
       container.unit = CreateTower(profile, emptySpot);
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

    // Create unit from a profile
    private Unit CreateTower(UnitProfile profile, int partyPosition) {
        var unitTemplate = TowerTemplate; //Would get which template we need from the profile, for now we just have only one
        var newUnit = Instantiate(unitTemplate, transform);
        var unitGfxName = UnitProfile.GetUnitGfxName(profile.unitID);
        newUnit.name = "Tower_"+unitGfxName;

        // Get graphic resource
        var graphicResourceName = "unitGfx/"+unitGfxName;
        var unitGfx = Object.Instantiate(Resources.Load(graphicResourceName), newUnit.transform) as GameObject;
        unitGfx.name = "unitGfx";

        //Set unit script
        var unitScript = newUnit.GetComponent<Unit>();
        unitScript.doInit(profile);
        unitScript.partyPos = partyPosition;

        return unitScript;
    }
}
