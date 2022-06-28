using System;
[Serializable]
public class Attack
{
    protected Unit myUnit;
    protected UnitRuntimeCollection targetList;
    //Range of this attack
    protected float range;
    protected float currentCooldown;
     //Does this attack only target friendly units
    protected bool targetFriendly;
    // The cooldown this attack will have
    protected float initialCooldown;
    protected bool attackerNeedsAlive;
    protected int power;
    protected ELMTTYPE moveType;
    protected bool isPhysical;
    protected float stab;
    protected BaseAttack baseFunc;
    protected EffectEvent addEffectEvent;

    public Attack(Unit myUnit, AttackProfile profile,  BaseAttack baseFunc, EffectEvent addEffectEvent) {
        this.addEffectEvent = addEffectEvent;
        init(myUnit, profile, baseFunc);
    }

    private void init(Unit whichUnit, AttackProfile profile, BaseAttack baseFunc) {
        this.baseFunc = baseFunc;
        myUnit = whichUnit;
        range = profile.range;
        targetFriendly = profile.targetFriendly;
        initialCooldown = profile.initialCooldown;
        attackerNeedsAlive = profile.attackerNeedsAlive;
        power = profile.basePower;
        moveType = profile.moveType;
        isPhysical = profile.isPhysical;
        // we can calculate this one time and be done!
        // in PTD we calculated this every time we attacked
        stab = baseFunc.getStabMultiplier(myUnit, moveType);
        targetList = !targetFriendly ? myUnit.getEnemyList() : myUnit.getFriendlyList();
        reset();
    }

    protected void reset() {
        currentCooldown = initialCooldown;
    }

    public void runCooldown(float timePassed) {
        currentCooldown = baseFunc.runCooldown(timePassed, currentCooldown);;
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
}
