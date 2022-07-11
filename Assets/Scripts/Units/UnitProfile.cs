using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Unit/Profile")]
public class UnitProfile : ScriptableObject
{
    public int unitID;
    public string nickname;
    public AttackID attack1ID;
    public AttackID attack2ID;
    public AttackID attack3ID;
    public AttackID attack4ID;
    public int attackSelected;
    public int lvl;
    public int special;
    // Multiply inital HP by this to increase total HP, mainly used for enemy units
    public int modHP;
    public int baseHP;
    public int baseAttack;
    public int baseSpAttack;
    public int baseDefense;
    public int baseSpDefense;
    public float baseSpeed;
    public int baseExperience;
    public int currentExperience;
    public float experiencePercent;
    //Will unit leave the level after reaching the end of the path
    public bool freeRoam;
    // Can the unit capture candy
    public bool canCaptureCandy;
    public bool canCaptureMe;
    public List<ELMTTYPE> elements;

    public bool nonDamagingAttackOnly;

    public UnitProfile(UnitProfile copyProfile=null) {
        //If we are copying from another profile, copy all values
        if (copyProfile != null) {
            unitID = copyProfile.unitID;
            nickname = copyProfile.nickname;
            attack1ID = copyProfile.attack1ID;
            attack2ID = copyProfile.attack2ID;
            attack3ID = copyProfile.attack3ID;
            attack4ID = copyProfile.attack4ID;
            attackSelected = copyProfile.attackSelected;
            lvl = copyProfile.lvl;
            special = copyProfile.special;
            modHP = copyProfile.modHP;
            baseHP = copyProfile.baseHP;
            baseAttack = copyProfile.baseAttack;
            baseSpAttack = copyProfile.baseSpAttack;
            baseDefense = copyProfile.baseDefense;
            baseSpDefense = copyProfile.baseSpDefense;
            baseSpeed = copyProfile.baseSpeed;
            baseExperience = copyProfile.baseExperience;
            freeRoam = copyProfile.freeRoam;
            canCaptureCandy = copyProfile.canCaptureCandy;
            elements = copyProfile.elements == null ? new List<ELMTTYPE>() : new List<ELMTTYPE>(copyProfile.elements);
            nonDamagingAttackOnly = copyProfile.nonDamagingAttackOnly;
            canCaptureMe = copyProfile.canCaptureMe;
        }

        currentExperience = 0;
        experiencePercent = 0.0f;
    }

    public static void getBaseValues(UnitProfile profile) {
        var baseInfo = GetBaseInfo(profile.unitID);
        profile.baseAttack = baseInfo.baseAttack;
        profile.baseDefense = baseInfo.baseDefense;
        profile.baseSpAttack = baseInfo.baseSpAttack;
        profile.baseSpDefense = baseInfo.baseSpDefense;
        profile.baseExperience = baseInfo.baseExperience;
        profile.elements = baseInfo.elements;
        // TODO: Add HP and Speed once those are implemented better
    }

    public static UnitBaseInfo GetBaseInfo(int unitID) {
        UnitBaseInfo baseInfo = new UnitBaseInfo();
        switch(unitID) {
            case 19:
                baseInfo.gfxResourceName = "u0020_rat";
                baseInfo.UnitResourceName = "unt_0020_rat";
                baseInfo.elements = new List<ELMTTYPE>() { ELMTTYPE.NORMAL };
                baseInfo.baseHP = 30;
                baseInfo.baseAttack = 56;
                baseInfo.baseDefense = 35;
                baseInfo.baseSpAttack = 25;
                baseInfo.baseSpDefense = 35;
                baseInfo.baseSpeed = 72;
                baseInfo.baseExperience = 51;
                baseInfo.UIScale = 37f;
                break;
            case 1:
            default:
                baseInfo.gfxResourceName = "u0001_frog";
                baseInfo.UnitResourceName = "unt_0001_frog";
                baseInfo.elements = new List<ELMTTYPE>() { ELMTTYPE.GRASS, ELMTTYPE.POISON };
                baseInfo.baseHP = 45;
                baseInfo.baseAttack = 49;
                baseInfo.baseDefense = 49;
                baseInfo.baseSpAttack = 65;
                baseInfo.baseSpDefense = 65;
                baseInfo.baseSpeed = 45;
                baseInfo.baseExperience = 64;
                baseInfo.UIScale = 37f;
                break;
        }
        return baseInfo;
    }

    public static string GetUnitGfxName(int unitID) {
        var baseInfo = GetBaseInfo(unitID);
        var name = baseInfo != null ? baseInfo.gfxResourceName : "";
        return name;
    }

    public static string GetWholeUnitGfxName(int unitID) {
         var baseInfo = GetBaseInfo(unitID);
        var name = baseInfo != null ? baseInfo.UnitResourceName : "";
        return name;
    }
}
