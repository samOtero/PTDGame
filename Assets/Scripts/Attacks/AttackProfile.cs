using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Profile")]
public class AttackProfile : ScriptableObject
{
    public float range;
    public int power;
    public bool attackerNeedsAlive;
    public float initialCooldown;

    public bool targetFriendly;
}
