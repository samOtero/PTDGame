public class EffectForUnit : Effect
{
    protected Unit target;
    protected Unit fromWho;
    protected int targetID;
    public EffectForUnit(Unit target, Unit fromWho, float delay=0, string tag=null, string tag2=null): base(delay, tag, tag2) {
        this.target = target;
        this.targetID = target.getID();
        this.fromWho = fromWho;
    }
}
