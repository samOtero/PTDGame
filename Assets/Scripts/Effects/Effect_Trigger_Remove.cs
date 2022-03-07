// This effect lets you manually trigger a remove at an arbitrary time
// Ex: When a projectile hits we might want to trigger a bunch of end effects from this effect
public class Effect_Trigger_Remove : Effect
{
    public Effect_Trigger_Remove(float delay=0, string tag=null, string tag2=null): base(delay, tag, tag2) {}
    public void doTrigger() {
        needRemove = true;
    }
}
