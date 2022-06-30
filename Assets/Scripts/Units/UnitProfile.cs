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
    public int exp;
    public int special;
    public int baseHP;
    public int baseAttack;
    public int baseSpAttack;
    public int baseDefense;
    public int baseSpDefense;
    public float baseSpeed;
    //Will unit leave the level after reaching the end of the path
    public bool freeRoam;
    // Can the unit capture candy
    public bool canCaptureCandy;
    public List<ELMTTYPE> elements;

    public bool nonDamagingAttackOnly;

    public static void getBaseValues(UnitProfile profile) {
        var baseInfo = GetBaseInfo(profile.unitID);
        profile.baseAttack = baseInfo.baseAttack;
        profile.baseDefense = baseInfo.baseDefense;
        profile.baseSpAttack = baseInfo.baseSpAttack;
        profile.baseSpDefense = baseInfo.baseSpDefense;
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
