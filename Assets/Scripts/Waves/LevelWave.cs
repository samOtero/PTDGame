using System.Collections.Generic;
using UnityEngine;

public class LevelWave : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;
    public UnitEvent UnitDefeatedEvent;
    public UnitEvent UnitCapturedEvent;
    public SpawnEnemyInPath SpawnEnemyInPathEvent;
    public List<WaveSegmentContainer> segmentContainers;
    public WaveSegmentContainer currentContainer;
    public List<Waypoint> PathList;
    public List<Unit> UnitList;
    public BaseUnit unitFunc;
    public int waveNum;
    public bool isCompleted;
    // Start is called before the first frame update
    void Start()
    {
        isCompleted = false;
        waveNum = 1;
        UnitList = new List<Unit>();
        UnitLeftEvent.RegisterListener(UnitLeftLevelEvent);
        UnitDefeatedEvent.RegisterListener(onUnitDefeated);
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
        SpawnEnemyInPathEvent.RegisterListener(onSpawnEnemyInPath);
        setContainer();
    }

    private void setContainer() {
        if (segmentContainers.Count < waveNum) {
            isCompleted = true;
            return;
        }
        currentContainer = segmentContainers[waveNum - 1];
        currentContainer.Init();
    }

    // When the spawn enemy in path event is called, we will spawn the enemy
    public int onSpawnEnemyInPath(UnitProfile profile, int pathNum) {
        var path = PathList[pathNum];
        // Do some pooling here
        var enemyUnit = GetFromPool(profile);
        if (enemyUnit != null) spawnEnemy(enemyUnit, path);
        else spawnEnemyOnPath(profile, path);
        return 1;
    }

    private Unit GetFromPool(UnitProfile profile) {
         if (UnitList.Count > 0) {
            // TODO: Actually look at enemy profile to check to see if we can use this unit
            var unit = UnitList[0];
            UnitList.RemoveAt(0);
            return unit;
        }

        return null;
    }

    private void AddToPool(Unit unit) {
        UnitList.Add(unit);
    }

    void spawnEnemyOnPath(UnitProfile profile, Waypoint path) {
        var unit = unitFunc.CreateEnemy(profile, unitFunc.EnemyFollowPath);
        spawnEnemy(unit, path);
    }

    void Update() {
        if (isCompleted) return;
        if (currentContainer.isCompleted) {
            waveNum++;
            setContainer();
            return;
        }

        currentContainer.Run();
    }

    private void spawnEnemy(Unit whichUnit, Waypoint path) {
        WayPointFollower follower = whichUnit.gameObject.GetComponent<WayPointFollower>();
        if (follower) {
            follower.reset(path);
        }
        whichUnit.Reset();
        whichUnit.setIsBattling(true); // When enemy is spawned it is now battling!
    }

    public int UnitLeftLevelEvent(Unit whichUnit) {
        whichUnit.doHide();
        AddToPool(whichUnit);
        return 0;
    }

    public int onUnitDefeated(Unit whichUnit) {
        AddToPool(whichUnit);
        return 0;
    }

    public int onUnitCaptured(Unit whichUnit) {
        AddToPool(whichUnit);
        return 0;
    }
}
