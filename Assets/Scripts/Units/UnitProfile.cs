using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Profile")]
public class UnitProfile : ScriptableObject
{
    public int unitID;
    public AttackID attack1ID;
    public AttackID attack2ID;
    public AttackID attack3ID;
    public AttackID attack4ID;
    public int attackSelected;
    public int lvl;
    public int exp;
    public int special;
    public int baseHP;
    public float baseSpeed;
    //Will unit leave the level after reaching the end of the path
    public bool freeRoam;

    public bool nonDamagingAttackOnly;

    public static string GetUnitGfxName(int unitID) {
        return "u0000_dog";
    }
}
