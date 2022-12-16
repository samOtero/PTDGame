using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Profile")]
public class UnitProfileObj : ScriptableObject
{
    public UnitID unitID;
    public string nickname;
    public AttackID attack1ID;
    public AttackID attack2ID = AttackID.NONE;
    public AttackID attack3ID = AttackID.NONE;
    public AttackID attack4ID = AttackID.NONE;
    public int attackSelected;
    public int lvl;
    public float speed = 4;
    public int HP = 10;
    // Multiply inital HP by this to increase total HP, mainly used for enemy units
    public int modHP = 1;
    //Will unit leave the level after reaching the end of the path
    public bool freeRoam;
    // Can the unit capture candy
    public bool canCaptureCandy = true;
    public bool canCaptureMe = true;
    public bool nonDamagingAttackOnly = true;
}
