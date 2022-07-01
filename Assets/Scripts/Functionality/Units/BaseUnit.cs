using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class has basic functionality for dealing with units
[CreateAssetMenu(menuName = "Functionality/Base Unit")]
public class BaseUnit : ScriptableObject
{
    // Base enemy that will follow path and get candy
    public GameObject EnemyFollowPath;


    // Create enemy unit from a profile
    public Unit CreateEnemy(UnitProfile profile, GameObject unitTemplate) {
        // Create new copy of profile for this unit
        var newProfile = new UnitProfile(profile);
        var newUnit = Instantiate(unitTemplate);
        var unitGfxName = UnitProfile.GetWholeUnitGfxName(newProfile.unitID);
        newUnit.name = "Enemy_"+unitGfxName;

        // Get graphic resource
        var graphicResourceName = "unitGfx/"+unitGfxName;
        var unitGfx = Object.Instantiate(Resources.Load(graphicResourceName), newUnit.transform) as GameObject;
        unitGfx.name = "unitGfx";

        // Add default values to profile
        // this will probably move higher up the chain once more is built out
        UnitProfile.getBaseValues(newProfile);

        //Set unit script
        var unitScript = newUnit.GetComponent<Unit>();
        unitScript.doInit(newProfile);

        return unitScript;
    }

    // Calculate HP based on profile
    public int calculateHP(UnitProfile profile) {
        int newHP = 2 * profile.baseHP * profile.lvl / 100;
		newHP +=  10 + profile.lvl;
        newHP *= profile.modHP; // Multiply by modifier
        return newHP;
    }
}
