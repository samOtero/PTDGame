public class Attack_Projectile : Attack
{
    protected override int doAttackActual() {
        Unit target = getTargetInRange();
        if (target) {
            //Create projectile effect and trigger effects
            BaseAttack.faceTarget(myUnit, target);
            var damageAmount = BaseAttack.getDamage(myUnit, target, moveType, stab, power, isPhysical);
            var damageFx = new Effect_Damage(target, myUnit, damageAmount, attackerNeedsAlive);
            var fadeFx = new EffectFadeUnit(myUnit);
            var trigger = new Effect_Trigger_Remove();
            trigger.addToEndStack(damageFx);
            trigger.addToStack(fadeFx);
            var unitGfxName = UnitProfile.GetUnitGfxName(myUnit.profile.unitID);
            var projectile = new Effect_Projectile("unitGfx/gfx/"+unitGfxName, trigger, target, myUnit);
            addEffectEvent.Raise(projectile);
            reset();
            return 1;
        }
        return 0;
    }
}
