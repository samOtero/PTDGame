using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Functionality/Base Attack")]
public class BaseAttack : ScriptableObject
{

// Calculate total damage done by an attack against a target
public int getDamage(Unit attacker, Unit target, ELMTTYPE moveType, float stab, int movePower, bool isPhysical) {
    var totalDamage = 0;
    var weakness = getWeakness(moveType, target);
    var attacker_attack = isPhysical ? attacker.profile.baseAttack : attacker.profile.baseSpAttack;
    var defender_defense = isPhysical ? target.profile.baseDefense : target.profile.baseSpDefense;
    var attacker_level = attacker.profile.lvl;
    // using damage calculation from https://bulbapedia.bulbagarden.net/wiki/Damage#Damage_calculation
    int damage1 = ((2 * attacker_level) / 5) + 2;
    int damage2 = movePower;
    int damage3 = attacker_attack / defender_defense;
    int damage4 = ((damage1 * damage2 * damage3)/ 50) + 2;
    // After we get damage apply weakness and stab bonuses
    int damage5 = (int)(damage4 * weakness * stab);
    totalDamage = damage5;
    return totalDamage;
}

// Get total weakness of an attack against a target
public float getWeakness(ELMTTYPE moveType, Unit defender) {
    float weakness = checkTypeWeakness(moveType, defender);
    // Other effects to weakness will be added here
    return weakness;
}

// Apply type weakness to an attack against a target elements
public float checkTypeWeakness(ELMTTYPE moveType, Unit defender) {
    float weakness = 1;
    float currentWeakness = 1;
    var defenderTypes = defender.profile.elements;
    foreach(var defenderType in defenderTypes) {
        currentWeakness = checkTypes(moveType, defenderType);
        weakness *= currentWeakness;
    }

    return weakness;
}

// Compare two types and return the weakness of the first type against the second type
public float checkTypes(ELMTTYPE attackType, ELMTTYPE defenderType) {
    // Normal VS
    if (attackType == ELMTTYPE.NORMAL) {
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        if (defenderType == ELMTTYPE.ROCK) return 0.5f;
        if (defenderType == ELMTTYPE.GHOST) return 0.0f;
        return 1.0f;
    }

    // Fire VS
    if (attackType == ELMTTYPE.FIRE) {
        if (defenderType == ELMTTYPE.FIRE) return 0.5f;
        if (defenderType == ELMTTYPE.WATER) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 2.0f;
        if (defenderType == ELMTTYPE.ICE) return 2.0f;
        if (defenderType == ELMTTYPE.BUG) return 2.0f;
        if (defenderType == ELMTTYPE.ROCK) return 0.5f;
        if (defenderType == ELMTTYPE.DRAGON) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 2.0f;
        return 1.0f;
    }

    // Water VS
    if (attackType == ELMTTYPE.WATER) {
        if (defenderType == ELMTTYPE.FIRE) return 2.0f;
        if (defenderType == ELMTTYPE.WATER) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 2.0f;
        if (defenderType == ELMTTYPE.ROCK) return 2.0f;
        if (defenderType == ELMTTYPE.DRAGON) return 0.5f;
        return 1.0f;
    }

    // Electric VS
    if (attackType == ELMTTYPE.ELECTRIC) {
        if (defenderType == ELMTTYPE.WATER) return 2.0f;
        if (defenderType == ELMTTYPE.ELECTRIC) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 0.0f;
        if (defenderType == ELMTTYPE.FLYING) return 2.0f;
        if (defenderType == ELMTTYPE.DRAGON) return 0.5f;
        return 1.0f;
    }

    // Grass VS
    if (attackType == ELMTTYPE.GRASS) {
        if (defenderType == ELMTTYPE.FIRE) return 0.5f;
        if (defenderType == ELMTTYPE.WATER) return 2.0f;
        if (defenderType == ELMTTYPE.GRASS) return 0.5f;
        if (defenderType == ELMTTYPE.POISON) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 2.0f;
        if (defenderType == ELMTTYPE.FLYING) return 0.5f;
        if (defenderType == ELMTTYPE.BUG) return 0.5f;
        if (defenderType == ELMTTYPE.ROCK) return 2.0f;
        if (defenderType == ELMTTYPE.DRAGON) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // ICE VS
    if (attackType == ELMTTYPE.ICE) {
        if (defenderType == ELMTTYPE.FIRE) return 0.5f;
        if (defenderType == ELMTTYPE.WATER) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 2.0f;
        if (defenderType == ELMTTYPE.ICE) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 2.0f;
        if (defenderType == ELMTTYPE.FLYING) return 2.0f;
        if (defenderType == ELMTTYPE.DRAGON) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Fighting VS
    if (attackType == ELMTTYPE.FIGHT) {
        if (defenderType == ELMTTYPE.NORMAL) return 2.0f;
        if (defenderType == ELMTTYPE.ICE) return 2.0f;
        if (defenderType == ELMTTYPE.POISON) return 0.5f;
        if (defenderType == ELMTTYPE.FLYING) return 0.5f;
        if (defenderType == ELMTTYPE.PSYCHIC) return 0.5f;
        if (defenderType == ELMTTYPE.BUG) return 0.5f;
        if (defenderType == ELMTTYPE.ROCK) return 2.0f;
        if (defenderType == ELMTTYPE.GHOST) return 0.0f;
        if (defenderType == ELMTTYPE.DARK) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 2.0f;
        return 1.0f;
    }

    // Poison VS
    if (attackType == ELMTTYPE.POISON) {
        if (defenderType == ELMTTYPE.GRASS) return 2.0f;
        if (defenderType == ELMTTYPE.POISON) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 0.5f;
        if (defenderType == ELMTTYPE.ROCK) return 0.5f;
        if (defenderType == ELMTTYPE.GHOST) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 0.0f;
        return 1.0f;
    }

    // Ground VS
    if (attackType == ELMTTYPE.GROUND) {
        if (defenderType == ELMTTYPE.FIRE) return 2.0f;
        if (defenderType == ELMTTYPE.ELECTRIC) return 2.0f;
        if (defenderType == ELMTTYPE.GRASS) return 0.5f;
        if (defenderType == ELMTTYPE.POISON) return 2.0f;
        if (defenderType == ELMTTYPE.FLYING) return 0.0f;
        if (defenderType == ELMTTYPE.BUG) return 0.5f;
        if (defenderType == ELMTTYPE.ROCK) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 2.0f;
        return 1.0f;
    }

    // Flying VS
    if (attackType == ELMTTYPE.FLYING) {
        if (defenderType == ELMTTYPE.ELECTRIC) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 2.0f;
        if (defenderType == ELMTTYPE.FIGHT) return 2.0f;
        if (defenderType == ELMTTYPE.BUG) return 2.0f;
        if (defenderType == ELMTTYPE.ROCK) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Psychic VS
    if (attackType == ELMTTYPE.PSYCHIC) {
        if (defenderType == ELMTTYPE.FIGHT) return 2.0f;
        if (defenderType == ELMTTYPE.POISON) return 2.0f;
        if (defenderType == ELMTTYPE.PSYCHIC) return 0.5f;
        if (defenderType == ELMTTYPE.DARK) return 0.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Bug VS
    if (attackType == ELMTTYPE.BUG) {
        if (defenderType == ELMTTYPE.FIRE) return 0.5f;
        if (defenderType == ELMTTYPE.GRASS) return 2.0f;
        if (defenderType == ELMTTYPE.FIGHT) return 0.5f;
        if (defenderType == ELMTTYPE.POISON) return 0.5f;
        if (defenderType == ELMTTYPE.FLYING) return 0.5f;
        if (defenderType == ELMTTYPE.PSYCHIC) return 2.0f;
        if (defenderType == ELMTTYPE.GHOST) return 0.5f;
        if (defenderType == ELMTTYPE.DARK) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Rock VS
    if (attackType == ELMTTYPE.ROCK) {
        if (defenderType == ELMTTYPE.FIRE) return 2.0f;
        if (defenderType == ELMTTYPE.ICE) return 2.0f;
        if (defenderType == ELMTTYPE.FIGHT) return 0.5f;
        if (defenderType == ELMTTYPE.GROUND) return 0.5f;
        if (defenderType == ELMTTYPE.FLYING) return 2.0f;
        if (defenderType == ELMTTYPE.BUG) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Ghost VS
    if (attackType == ELMTTYPE.GHOST) {
        if (defenderType == ELMTTYPE.NORMAL) return 0.0f;
        if (defenderType == ELMTTYPE.PSYCHIC) return 2.0f;
        if (defenderType == ELMTTYPE.GHOST) return 2.0f;
        if (defenderType == ELMTTYPE.DARK) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Dragon VS
    if (attackType == ELMTTYPE.DRAGON) {
        if (defenderType == ELMTTYPE.DRAGON) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Dark VS
    if (attackType == ELMTTYPE.DARK) {
        if (defenderType == ELMTTYPE.FIGHT) return 0.5f;
        if (defenderType == ELMTTYPE.PSYCHIC) return 2.0f;
        if (defenderType == ELMTTYPE.GHOST) return 2.0f;
        if (defenderType == ELMTTYPE.DARK) return 0.5f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    // Steel VS
    if (attackType == ELMTTYPE.STEEL) {
        if (defenderType == ELMTTYPE.FIRE) return 0.5f;
        if (defenderType == ELMTTYPE.WATER) return 0.5f;
        if (defenderType == ELMTTYPE.ELECTRIC) return 0.5f;
        if (defenderType == ELMTTYPE.ICE) return 2.0f;
        if (defenderType == ELMTTYPE.ROCK) return 2.0f;
        if (defenderType == ELMTTYPE.STEEL) return 0.5f;
        return 1.0f;
    }

    throw(new Exception("Type not implemented in checkTypes for attackType"+ attackType + " and defenderType " + defenderType));
    return 1.0f;
}

// Get stab multiplier for a move and user
public float getStabMultiplier(Unit attacker, ELMTTYPE movetype) {
    float stabAmount = 1;
    if (attacker.profile.elements.Contains(movetype)) {
        stabAmount = 1.5f;
    }
    return stabAmount;
}

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
