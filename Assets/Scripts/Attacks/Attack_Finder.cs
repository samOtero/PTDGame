using UnityEngine;

public class Attack_Finder : MonoBehaviour
{
    public CreateAttackEvent createAttackEvent;
    public BaseAttack baseFunc;
    public EffectEvent addEffectEvent;
    public AttackProfile basicProfile;
    public AttackProfile basicProjectileProfile;

    void Start() {
        createAttackEvent.RegisterListener(createAttack);
    }
    
    public Attack createAttack(Unit unit, AttackID attackID) {
        Attack newAttack = null;
        switch(attackID) {
            case AttackID.BASIC_INSTANT_DAMAGE:
                newAttack = new Attack_Damage(unit, basicProfile, baseFunc, addEffectEvent);
                break;
            case AttackID.BASIC_PROJECTILE:
                newAttack = new Attack_Projectile(unit, basicProjectileProfile, baseFunc, addEffectEvent);
                break;
        }
        return newAttack;
    }
}
