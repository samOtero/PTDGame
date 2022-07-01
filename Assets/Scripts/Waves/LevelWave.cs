using System.Collections.Generic;
using UnityEngine;

public class LevelWave : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;
    public UnitEvent UnitDefeatedEvent;

    public UnitEvent UnitCapturedEvent;
    public Waypoint Path1;
    public Waypoint Path2;
    public Waypoint Path3;
    public Waypoint Path4;

    public UnitProfile profile1;

    public List<Unit> UnitList;
    public BaseUnit unitFunc;
    public int counter;
    public int counterTotal;
    public int waveNum;
    // Start is called before the first frame update
    void Start()
    {
        counter = counterTotal;
        waveNum = 1;
        UnitList = new List<Unit>();
        UnitLeftEvent.RegisterListener(UnitLeftLevelEvent);
        UnitDefeatedEvent.RegisterListener(onUnitDefeated);
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
    }

    void spawnEnemyOnPath(UnitProfile profile, Waypoint path) {
        var unit = unitFunc.CreateEnemy(profile, unitFunc.EnemyFollowPath);
        spawnEnemy(unit, path);
    }

    void Update() {
        if (counter > 0) {
            counter--;
            return;
        }
        counter = counterTotal;
        if (waveNum == 1) {
            waveNum++;
            spawnEnemyOnPath(profile1, Path1);
        }

        if (UnitList.Count > 0) {
            
            var unit = UnitList[0];
            UnitList.RemoveAt(0);
            //Destroy(unit.gameObject);
           spawnEnemy(unit, Path1);
        }
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
        UnitList.Add(whichUnit);
        return 0;
    }

    public int onUnitDefeated(Unit whichUnit) {
        UnitList.Add(whichUnit);
        return 0;
    }

    public int onUnitCaptured(Unit whichUnit) {
        UnitList.Add(whichUnit);
        return 0;
    }
}
