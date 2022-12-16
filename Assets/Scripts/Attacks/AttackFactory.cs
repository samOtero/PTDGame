using UnityEngine;

public class AttackFactory : MonoBehaviour
{
    public CreateAttackEvent createAttackEvent;

    void Start() {
        createAttackEvent.RegisterListener(createAttack);
    }
    
    public Attack createAttack(Unit unit, AttackID attackID) {
        var resourcePath = "attacks/";
        string attackResourceName = getAttackResourceName(attackID);
        GameObject attackObj = Instantiate(Resources.Load(resourcePath + attackResourceName), unit.transform) as GameObject;
        Attack attackScript = attackObj.GetComponent<Attack>();
        attackScript.init(unit);
        return attackScript;
    }

    private string getAttackResourceName(AttackID ID)
    {
        string attackResourceName;
        switch (ID)
        {
            case AttackID.BASIC_INSTANT_DAMAGE:
                attackResourceName = "Attack_InstantDamage";
                break;
            case AttackID.BASIC_PROJECTILE:
            default:
                attackResourceName = "Attack_Projectile";

                break;
        }

        return attackResourceName;
    }
}
