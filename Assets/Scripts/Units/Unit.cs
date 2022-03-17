using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour, ITargetable, IHasLife
{
    public bool isEnemy;
    public bool isAlive;
    public bool isHidding;
    public int totalLife;
    public int currentLife;

    public float currentSpeed;
    public UnitRuntimeCollection EnemyList;
    public UnitRuntimeCollection FriendlyList;

    public GameObject positionRef;
    public GameObject graphicRef;
    public Material graphicMaterial;

    public UnitEvent UnitDefeatedEvent;
    public UnitEvent UnitCaptured;

    public FloatVariable weaknessLevel;

    protected List<Func<float, int>> LifeChangeListeners;
    public int id;

    public UnitProfile profile;
    public int partyPos; // Tower unit's position in the party
    public int attackSelected;
    public bool isDragged; 

    public int getID() {
        return id;
    }

    
    public void doInit(UnitProfile profile) {
        // set up info from profile given
        currentSpeed = profile.baseSpeed;
        totalLife = profile.baseHP;
        attackSelected = profile.attackSelected;

        this.profile = profile;
        // Add unit to it's respective list
        if (isEnemy && EnemyList) EnemyList.Add(this);
        else if (!isEnemy && FriendlyList) FriendlyList.Add(this);
        positionRef = getPositionObject();
        graphicRef = getUnitGraphicObject();
        if (graphicRef) {
            var renderer = graphicRef.GetComponentInChildren<MeshRenderer>();
            graphicMaterial = renderer.materials[0];
        }
        id = 1; //should get this from global
        Reset();
    }

    public void Reset() {
        setLife(totalLife);
        id++; //increase id so we know this is no longer the same unit, this needs to come from a global value!
        isAlive = true;
        isHidding = false;     
    }

    protected void setLife(int newLife) {
        currentLife = newLife;
        doLifeChangeEvent();
    }

    protected void doLifeChangeEvent() {
        if (LifeChangeListeners != null) {
            foreach(var listener in LifeChangeListeners) {
                listener(getLifePercent());
            }
        }
    }

    public void registerToLifeChangeEvent(Func<float, int> listener) {
        if (LifeChangeListeners == null) LifeChangeListeners = new List<Func<float, int>>();
        if (LifeChangeListeners.Contains(listener) == false)
            LifeChangeListeners.Add(listener);
    }

    
    public void unregisterToLifeChangeEvent(Func<float, int> listener) {
        if (LifeChangeListeners == null) return;
        if (LifeChangeListeners.Contains(listener) == true)
            LifeChangeListeners.Remove(listener);
    }

    public float getLifePercent() {
        float floatCurrent = Convert.ToSingle(currentLife);
        return floatCurrent / totalLife;
    }

    public void doCapture() {
        setLife(0);
        isAlive = false;
        doHide();
        UnitCaptured.Raise(this);
    }

    public bool canCapture() {
        var isCapturable = false;
        if (getLifePercent() <= weaknessLevel.Value) isCapturable = true;
        return isCapturable;
    }

    // Hide Unit from view
    public void doHide() {
        isHidding = true;
        positionRef.transform.position = new Vector3(1000.0f, 500.0f, 0);
    }

    public void doFade(float newFade) {
       if (newFade >= 1) {
           MaterialUtil.ToOpaqueMode(graphicMaterial);
       }else{
           MaterialUtil.ToFadeMode(graphicMaterial);
       }
    }

    public int takeDamage(int howMuch) {
        if (isAlive == false) return 0;
        var newLife = currentLife - howMuch;
        if (newLife <= 0){
            onDefeat();
            return howMuch;
        }
        setLife(newLife);
        return howMuch;
    }

    private void onDefeat() {
        setLife(0);
        isAlive = false;
        doHide();
        if (UnitDefeatedEvent) UnitDefeatedEvent.Raise(this);
    }


    public UnitRuntimeCollection getEnemyList() {
        if (isEnemy) return FriendlyList;
        return EnemyList;
    }

    public UnitRuntimeCollection getFriendlyList() {
        if (isEnemy) return EnemyList;
        return FriendlyList;
    }

    public void faceDirection(Direction whichDirection) {
        var rotation = getRotationFromDirection(whichDirection);
        Vector3 newRotation = new Vector3(0, rotation, 0);
        graphicRef.transform.localEulerAngles = newRotation;
    }

    public float getRotationFromDirection(Direction direction) {
        float newRotation = 0.0f;
        switch(direction) {
            case Direction.NORTH:
            newRotation = 0.0f;
            break;
            case Direction.SOUTH:
            newRotation = 180.0f;
            break;
            case Direction.EAST:
            newRotation = 90.0f;
            break;
            case Direction.WEST:
            newRotation = -90.0f;
            break;
        }
        return newRotation;
    }

    public bool isTargetable() {
        return isAlive && !isHidding;
    }

    public Vector3 getLocation() {
        return getPositionObject().transform.position;
    }

    // Gameobject that will control the position of the unit
    private GameObject getPositionObject() {
        return gameObject;
    }

    // Gameobject for the unit graphic that will control unit rotation and animation
    private GameObject getUnitGraphicObject() {
        return gameObject.transform.Find("unitGfx").Find("gfxContainer").gameObject;
    }
}
