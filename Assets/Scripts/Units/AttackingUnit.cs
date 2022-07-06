using UnityEngine;

public class AttackingUnit : MonoBehaviour
{
    public Unit myUnit;
    public Attack attack1;
    public CreateAttackEvent createAttackEvent;
    public bool isInit;
    public IntVariable PauseStatus;
    public BasicEvent DoRun;

    private void Start() {
        init();
    }

    // Called by Level Runner
    public int onDoRun()
    {
        if (PauseStatus.Value > 0) return 0;
        
        if (isInit == false) {
            initAttack();
            return 1;
        }
        if (attack1 != null) {
            attack1.runCooldown(Time.deltaTime);
            // Only try to attack if we are battling
            if (myUnit.isBattling) attack1.doAttack();
        }

        return 1;
    }

    public void init() {
        DoRun.RegisterListener(onDoRun);
    }

    public void initAttack() {

        if (myUnit == null)
            myUnit = gameObject ? gameObject.GetComponent<Unit>(): null;

        if (myUnit != null) {
            var attackId = myUnit.profile.attack1ID;
            attack1 = GetAttack(myUnit, attackId);
            if (attack1 != null) {
                isInit = true;
            } 
        }
        
        
    }

    private Attack GetAttack(Unit myUnit, AttackID id) {
        return createAttackEvent.Raise(myUnit, id);
    }
}
