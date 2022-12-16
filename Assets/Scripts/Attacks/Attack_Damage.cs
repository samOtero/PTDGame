public class Attack_Damage : Attack
{
    protected override int doAttackActual() {
        Unit target = getTargetInRange();
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
