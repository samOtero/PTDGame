using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveManager : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;
    public UnitEvent UnitDefeatedEvent;
    public UnitEvent UnitCapturedEvent;
    public UnitEvent AddToPoolEvent;
    public SpawnEnemyInPath SpawnEnemyInPathEvent;
    public CreateUnitEvent CreateEnemyUnitEvent;
    public List<Waypoint> PathList;

    // Start is called before the first frame update
    void Start()
    {
        UnitLeftEvent.RegisterListener(onUnitLeftLevelEvent);
        UnitDefeatedEvent.RegisterListener(onUnitDefeated);
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
        SpawnEnemyInPathEvent.RegisterListener(onSpawnEnemyInPath);
    }

    public int onUnitLeftLevelEvent(Unit whichUnit)
    {
        AddToPoolEvent.Raise(whichUnit);
        return 0;
    }

    public int onUnitDefeated(Unit whichUnit)
    {
        AddToPoolEvent.Raise(whichUnit);
        return 0;
    }

    public int onUnitCaptured(Unit whichUnit)
    {
        AddToPoolEvent.Raise(whichUnit);
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
    public int onSpawnEnemyInPath(UnitProfileObj profile, int pathNum)
    {
        Waypoint path = PathList[pathNum];
        UnitProfile newProfile = new UnitProfile(profile);
        Unit unit = CreateEnemyUnitEvent.Raise(newProfile, TDUnitTypes.BASIC_ENEMY);
        spawnEnemy(unit, path);
        return 1;
    }
}
