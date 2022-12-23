using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour, ITargetable, IHasLife
{
    public bool isEnemy;
    public bool isAlive;
    // If the unit is in the level or not
    public bool isBattling;
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
    public UnitEvent UnitGotExperience;

    public FloatVariable weaknessLevel;

    protected List<Func<float, int>> LifeChangeListeners;
    // Makes enemy unit unique when using pooling
    public int id;

    public UnitProfile profile;
    public WayPointFollower pathFollower;
    // Stores spot the tower is on, if any
    public TowerSpot currentSpot;
    public int partyPos; // Tower unit's position in the party
    public int attackSelected;
    public bool isDragged; 

    // Events for dragging a tower
    public IntEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BoxCollider dragCollider;

    public List<Unit> hitMeList;
    public int highestLevelHitMe;

    public int getID() {
        return id;
    }

    // Called when a tower started to be dragged
    public int onStartedTowerDrag(int partyPos) {
        // disable colliders
        if (dragCollider != null) dragCollider.enabled = false;
        return 1;
    }

    // Called when a tower stops being dragged
    public int onEndedTowerDrag() {
        // enable colliders
        if (dragCollider != null) dragCollider.enabled = true;
        return 1;
    }

    
    public void doInit(UnitProfile profile) {
        // set up info from profile given
        currentSpeed = profile.baseSpeed;
        totalLife = BaseUnit.calculateHP(profile);
        attackSelected = profile.attackSelected;
        highestLevelHitMe = 1;

        // Unit is not battling yet
        isBattling = false;
        isAlive = false;

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
        pathFollower = gameObject.GetComponent<WayPointFollower>();
        dragCollider = gameObject.GetComponent<BoxCollider>();

        // Register listeners for tower drags
        StartTowerDrag.RegisterListener(onStartedTowerDrag);
        EndTowerDrag.RegisterListener(onEndedTowerDrag);

        // auto reset when first creating the unit
        Reset();
        doHide(); // Hide units out of screen when creating them
    }

    public void Reset() {
        setLife(totalLife);
        id++; //increase id so we know this is no longer the same unit, this needs to come from a global value!
        isAlive = true;
        hitMeList = new List<Unit>();   
    }

    protected void setLife(int newLife) {
        currentLife = newLife;
        doLifeChangeEvent();
    }

    protected void doLifeChangeEvent() {
        if (LifeChangeListeners != null) {
            var lifePercent = getLifePercent();
            foreach(var listener in LifeChangeListeners) {
                listener(lifePercent);
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
        if (pathFollower) pathFollower.dropCandy();
        // Give experience to units that attacked me
        giveExperience();
        doHide();
        UnitCaptured.Raise(this);
    }

    public bool canCapture() {
        var isCapturable = false;
        if (getLifePercent() <= weaknessLevel.Value) isCapturable = true;
        return isCapturable;
    }

    public void removeFromBattle() {
        if (currentSpot) currentSpot.removeUnit(this);
        setIsBattling(false);
        currentSpot = null;
        doHide();
    }

    // Hide Unit from view
    public void doHide() {
        positionRef.transform.position = new Vector3(1000.0f, 500.0f, 0);
    }

    // This is to be called when a unit is put into the battle field or removed
    public void setIsBattling(bool isBattling) {
       this.isBattling = isBattling;
    }

    public void doFade(float newFade) {
       if (newFade >= 1) {
           MaterialUtil.ToOpaqueMode(graphicMaterial);
       }else{
           MaterialUtil.ToFadeMode(graphicMaterial, newFade);
       }
    }

    public int takeDamage(int howMuch, Unit fromWho) {
        if (isAlive == false) return 0;
        AddToHitMeList(fromWho);
        var newLife = currentLife - howMuch;
        if (newLife <= 0){
            onDefeat();
            return howMuch;
        }
        setLife(newLife);
        return howMuch;
    }

    // Add unit to the hit me list, so we can give them exp when defeated
    private void AddToHitMeList(Unit fromWho) {
        if (hitMeList.Contains(fromWho) == false) {
            hitMeList.Add(fromWho);
        }

        // Update highest level hit me
        if (fromWho.profile.lvl > highestLevelHitMe) {
            highestLevelHitMe = fromWho.profile.lvl;
        }
    }

    // This unit got experience
    public void getExperience(int howMuch) {
        var leveledUp = false;
        var totalLevelUps = 0;
        do {
            leveledUp = BaseUnit.receiveExperience(profile, howMuch);
            if (leveledUp) totalLevelUps++;
        }
        while(leveledUp);
        UnitGotExperience.Raise(this);
    }

    private void giveExperience() {
        //TODO: Remove units that have fainted from the hit me list
        var totalExp = BaseUnit.calculateExperience(profile, hitMeList.Count, highestLevelHitMe);
        foreach(var hitMe in hitMeList) {
            hitMe.getExperience(totalExp);
        }
    }

    private void onDefeat() {
        setLife(0);
        isAlive = false;
        setIsBattling(false);
        if (pathFollower) pathFollower.dropCandy();
        // Give experience to units that attacked me
        giveExperience();
        doHide();
        if (UnitDefeatedEvent) UnitDefeatedEvent.Raise(this);
    }

    // Used for enemies when they leave level
    public void onLeaveLevel()
    {
        setIsBattling(false);
        doHide();
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
        return isAlive && isBattling;
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
