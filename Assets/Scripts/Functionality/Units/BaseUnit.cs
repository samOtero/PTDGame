using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class has basic functionality for dealing with units
[CreateAssetMenu(menuName = "Functionality/Base Unit")]
public class BaseUnit : ScriptableObject
{
    // Base enemy that will follow path and get candy
    public GameObject EnemyFollowPath;

    // Base Tower gameobject ref
    public GameObject TowerTemplate;


    // Create enemy unit from a profile
    public Unit CreateEnemy(UnitProfile profile, GameObject unitTemplate) {
         // Create new copy of profile for this unit
        var newProfile = new UnitProfile(profile);
        return CreateUnit(newProfile, unitTemplate, "Enemy_");
    }

    public Unit CreateTower(UnitProfile profile, GameObject unitTemplate, int partyPosition) {
        // Want to keep the party profile the same for the unit
        var unit = CreateUnit(profile, unitTemplate, "Tower_");
        unit.partyPos = partyPosition; // Add the party position
        return unit;
    }

    // Create enemy unit from a profile
    private Unit CreateUnit(UnitProfile newProfile, GameObject unitTemplate, string prefix) {
        var newUnit = Instantiate(unitTemplate);
        var unitGfxName = UnitProfile.GetWholeUnitGfxName(newProfile.unitID);
        newUnit.name = prefix+unitGfxName;

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

    // Give experience to a unit, returns true if they leveled up
    public bool receiveExperience(UnitProfile profile, int exp) {
        var leveledUp = false;
        var expToNextLevel = getExpNeededToLevel(profile.lvl);
        profile.currentExperience += exp;
        if (profile.currentExperience >= expToNextLevel) {
            profile.currentExperience -= expToNextLevel;
            profile.lvl++; // This could be refactored actually
            leveledUp = true;
            expToNextLevel = getExpNeededToLevel(profile.lvl);
        }

        //Update experience percent
        profile.experiencePercent = (float)profile.currentExperience / (float)expToNextLevel;
        return leveledUp;
    }

    public int getExpNeededToLevel(int level) {
        int expNeeded = (int)Mathf.Pow(level, 3); // Med Fast for everyone!
        return expNeeded;
    }

    // Calculate experience based on profile
    public int calculateExperience(UnitProfile profile, int totalHitMe, int highestLevelHitMe) {
        int totalExp = 0;

        if (profile.baseExperience == 0) return 0;

        // Bonus if the unit cannot be captured
        float isWildBonus = profile.canCaptureMe ? 1.0f : 1.5f;
        int level = profile.lvl;
        int baseExp = profile.baseExperience;
        // See https://bulbapedia.bulbagarden.net/wiki/Experience for calculation
        int calc1 = (int)(isWildBonus * baseExp * level);
        int calc2 = calc1 / (7 * totalHitMe);
        int calc3 = calc2 + 1;

        totalExp = calc3;
        return totalExp;
    }
}
