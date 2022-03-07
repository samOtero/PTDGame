using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ITargetable myTarget;
    public int targetID;
    public Vector3 targetLocation;
    public float speed;
    public float acceleration;
    public float maxSpeed;
    public bool faceTarget;
    public Effect_Trigger_Remove OnHitEffect;
    public ProjectileBasicFunc func;
    public EffectEvent addEffectEvent;
    public Animator animator;
    public bool paused;
    public bool isInit;

    public GameObject gfx;

    public void init(ITargetable target, Effect_Trigger_Remove OnHitEffect=null, GameObject gfx=null, float speed = 2f, bool faceTarget=true)
    {
        this.gfx = gfx;
        this.speed = speed;
        this.OnHitEffect = OnHitEffect;
        this.myTarget = target;
        targetID = target.getID();
        targetLocation = target.getLocation();
        animator = gameObject.GetComponent<Animator>();
        this.faceTarget = faceTarget;

        //Add OnHitEffect to container
        if (OnHitEffect != null) {
            addEffectEvent.Raise(OnHitEffect);
        }
        isInit = true;
    }

    void Update()
    {
        if (isInit)
            func.doRun(this);       
    } 
}
