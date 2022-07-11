public class Effect_Damage : EffectForUnit
{
    protected int amount;
    protected bool attackerNeedsAlive;
    public Effect_Damage(Unit target, Unit fromWho, int amount, bool attackerNeedsAlive = true, float delay=0, string tag=null, string tag2=null): base(target, fromWho, delay, tag, tag2) {
       this.amount = amount;
       this.attackerNeedsAlive = attackerNeedsAlive;
    }

     protected override void doRunCustom()
    {
        if (needRemove)
            return;

        //If our attacker needs to be alive and it is not then don't do this damage
        if (attackerNeedsAlive == true && !fromWho.isAlive)
        {
            needRemove = true;
            return;
        }

        //Damage target, if they aren't dead
        if (target.getID() == targetID) //ensure this is still the same target and not a pooled one
            target.takeDamage(amount, fromWho);

        //Remove this effect
        needRemove = true;
    }
}
