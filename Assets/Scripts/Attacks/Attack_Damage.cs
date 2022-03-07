public class Attack_Damage : Attack
{
    public Attack_Damage(Unit myUnit, AttackProfile profile, BaseAttack baseFunc, EffectEvent addEffectEvent): base(myUnit, profile, baseFunc, addEffectEvent) {
    }
    protected override int doAttackActual() {
        Unit target = baseFunc.getTargetInRange(myUnit, targetList, range);
        if (target) {
            //Create damage effect for target
            var damageFx = new Effect_Damage(target, myUnit, power, attackerNeedsAlive);
            addEffectEvent.Raise(damageFx);
            reset();
            return 1;
        }
        return 0;
    }
}
