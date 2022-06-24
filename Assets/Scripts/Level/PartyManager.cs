using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject TowerTemplate;
    public UnitParty currentParty;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    private void init() {
       Debug.Log("initializing party...");
       
       // Loop through all the party spots and create a unit for each one
       for(var i=0; i < currentParty.party.Count; i++) {
            var container = currentParty.party[i];
            if (container.unit) Destroy(container.unit);
            container.unit = CreateTower(container.profile, i);
            container.hasBeenCreated = true;
       }

       Debug.Log("party initialized");
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
