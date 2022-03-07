using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Functionality/Base Attack")]
public class BaseAttack : ScriptableObject
{

public void faceTarget(Unit attacker, ITargetable target) {
    var targetLoc = target.getLocation();
    var atkZ = attacker.positionRef.transform.position.z;
    var atkX = attacker.positionRef.transform.position.x;
    var zDelta = Math.Abs(atkZ - targetLoc.z);
    var xDelta = Math.Abs(atkX - targetLoc.x);
    var newDirection = Direction.NORTH;
    if (zDelta > xDelta) {
        if (atkZ < targetLoc.z) {
            newDirection = Direction.NORTH;
        }else{
            newDirection = Direction.SOUTH;
        }
    }else{
        if (atkX > targetLoc.x) {
            newDirection = Direction.WEST;
        }else{
            newDirection = Direction.EAST;
        }
    }
    attacker.faceDirection(newDirection);
}

public Unit getTargetInRange(Unit attacker, UnitRuntimeCollection targetList, float range) {
        Unit target = null;

        Vector2 attackerLoc = new Vector2(attacker.positionRef.transform.position.x, attacker.positionRef.transform.position.z);

        Vector2 targetLoc = new Vector2();
        Unit currentTarget;
        float deltaDistance;
        for(var i=0; i<targetList.Items.Count; i++) {
            currentTarget = targetList.Items[i];
            if (currentTarget.isAlive == false) continue;
            targetLoc.x = currentTarget.positionRef.transform.position.x;
            targetLoc.y = currentTarget.positionRef.transform.position.z;
            deltaDistance = Vector2.Distance(attackerLoc, targetLoc);
            if (deltaDistance <= range) {
                target = currentTarget;
                break;
            }
        }

        return target;
    }
    public float runCooldown(float timePassed, float currentCooldown) {
        if (currentCooldown > 0) {
            currentCooldown -= timePassed;
        }
        return currentCooldown;
    }
}
