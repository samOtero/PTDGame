using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Projectile : Attack
{
    public Attack_Projectile(Unit myUnit, AttackProfile profile, BaseAttack baseFunc, EffectEvent addEffectEvent): base(myUnit, profile, baseFunc, addEffectEvent) {}

    protected override int doAttackActual() {
        Unit target = baseFunc.getTargetInRange(myUnit, targetList, range);
        if (target) {
            //Create projectile effect and trigger effects
            baseFunc.faceTarget(myUnit, target);
            var damageFx = new Effect_Damage(target, myUnit, power, attackerNeedsAlive);
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
