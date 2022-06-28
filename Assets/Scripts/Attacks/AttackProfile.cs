using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Profile")]
public class AttackProfile : ScriptableObject
{
    public float range;
    public int basePower;
    public ELMTTYPE moveType;
    public bool isPhysical;
    public bool attackerNeedsAlive;
    public float initialCooldown;

    public bool targetFriendly;
}
