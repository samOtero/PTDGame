using UnityEngine;

public class Attack: MonoBehaviour
{
    public Unit myUnit;
    public UnitRuntimeCollection targetList;
    //Range of this attack
    public float range;
    public float currentCooldown;
     //Does this attack only target friendly units
    public bool targetFriendly;
    // The cooldown this attack will have
    public float initialCooldown;
    public bool attackerNeedsAlive;
    public int power;
    public ELMTTYPE moveType;
    public bool isPhysical;
    public float stab;
    public EffectEvent addEffectEvent;

    /*
    public Attack(Unit myUnit, AttackProfile profile, EffectEvent addEffectEvent) {
        this.addEffectEvent = addEffectEvent;
        init(myUnit, profile);
    }
    */

    public void init(Unit whichUnit) {
        myUnit = whichUnit;
        /*
        range = profile.range;
        targetFriendly = profile.targetFriendly;
        initialCooldown = profile.initialCooldown;
        attackerNeedsAlive = profile.attackerNeedsAlive;
        power = profile.basePower;
        moveType = profile.moveType;
        isPhysical = profile.isPhysical;
        */
        // we can calculate this one time and be done!
        // in PTD we calculated this every time we attacked
        stab = BaseAttack.getStabMultiplier(myUnit, moveType);
        targetList = !targetFriendly ? myUnit.getEnemyList() : myUnit.getFriendlyList();
        reset();
    }

    protected void reset() {
        currentCooldown = initialCooldown;
    }

    public void runCooldown(float timePassed) {
        currentCooldown = BaseAttack.runCooldown(timePassed, currentCooldown);;
    }

    public int doAttack() {
        if (currentCooldown > 0) return 0;
        if (myUnit.profile.nonDamagingAttackOnly && !isNonDamagingAttack()) return 0;
        return doAttackActual();
    }


    //Does this attack deal damage, used to see if the unit can use it or not
    public virtual bool isNonDamagingAttack() {
        return false;
    }

    protected virtual int doAttackActual() {
        //implement in subclass
        reset();
        return 0;
    }

    protected virtual Unit getTargetInRange()
    {
        return BaseAttack.getTargetInRange(myUnit, targetList, range);
    }
}
