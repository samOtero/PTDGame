using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveManager : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;
    public UnitEvent UnitDefeatedEvent;
    public UnitEvent UnitCapturedEvent;
    public SpawnEnemyInPath SpawnEnemyInPathEvent;
    public List<Waypoint> PathList;
    public List<Unit> UnitList;
    public BaseUnit unitFunc;

    // Start is called before the first frame update
    void Start()
    {
        UnitList = new List<Unit>();
        UnitLeftEvent.RegisterListener(UnitLeftLevelEvent);
        UnitDefeatedEvent.RegisterListener(onUnitDefeated);
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
        SpawnEnemyInPathEvent.RegisterListener(onSpawnEnemyInPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int UnitLeftLevelEvent(Unit whichUnit)
    {
        whichUnit.doHide();
        AddToPool(whichUnit);
        return 0;
    }

    public int onUnitDefeated(Unit whichUnit)
    {
        AddToPool(whichUnit);
        return 0;
    }

    public int onUnitCaptured(Unit whichUnit)
    {
        AddToPool(whichUnit);
        return 0;
    }

    private void spawnEnemy(Unit whichUnit, Waypoint path)
    {
        WayPointFollower follower = whichUnit.gameObject.GetComponent<WayPointFollower>();
        if (follower)
        {
            follower.reset(path);
        }
        whichUnit.Reset();
        whichUnit.setIsBattling(true); // When enemy is spawned it is now battling!
    }

    // When the spawn enemy in path event is called, we will spawn the enemy
    public int onSpawnEnemyInPath(UnitProfile profile, int pathNum)
    {
        var path = PathList[pathNum];
        // Do some pooling here
        var enemyUnit = GetFromPool(profile);
        if (enemyUnit != null) spawnEnemy(enemyUnit, path);
        else spawnEnemyOnPath(profile, path);
        return 1;
    }

    private Unit GetFromPool(UnitProfile profile)
    {
        if (UnitList.Count > 0)
        {
            // TODO: Actually look at enemy profile to check to see if we can use this unit
            var unit = UnitList[0];
            UnitList.RemoveAt(0);
            return unit;
        }

        return null;
    }

    private void AddToPool(Unit unit)
    {
        UnitList.Add(unit);
    }

    private void spawnEnemyOnPath(UnitProfile profile, Waypoint path)
    {
        var unit = unitFunc.CreateEnemy(profile, unitFunc.EnemyFollowPath);
        spawnEnemy(unit, path);
    }
}
