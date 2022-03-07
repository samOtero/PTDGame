using UnityEngine;

public class AttackingUnit : MonoBehaviour
{
    public Unit myUnit;
    public Attack attack1;
    public CreateAttackEvent createAttackEvent;
    public bool isInit;
    public IntVariable PauseStatus;

    // Update is called once per frame
    void Update()
    {
        if (PauseStatus.Value > 0) return;
        
        if (isInit == false) {
            init();
            return;
        }
        if (attack1 != null) {
            attack1.runCooldown(Time.deltaTime);
            attack1.doAttack();
        }
    }

    public void init() {

        if (myUnit == null)
            myUnit = gameObject ? gameObject.GetComponent<Unit>(): null;

        if (myUnit != null) {
            var attackId = myUnit.profile.attack1ID;
            attack1 = GetAttack(myUnit, attackId);
            if (attack1 != null) isInit = true;
        }
        
    }

    private Attack GetAttack(Unit myUnit, AttackID id) {
        return createAttackEvent.Raise(myUnit, id);
    }
}
