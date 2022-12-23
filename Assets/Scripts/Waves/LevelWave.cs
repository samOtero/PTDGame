using System.Collections.Generic;
using UnityEngine;

public class LevelWave : MonoBehaviour
{
    public IntVariable PauseStatus;
    public BasicEvent DoRun;
    public List<WaveSegmentContainer> segmentContainers;
    public WaveSegmentContainer currentContainer;
    public IntVariable WavesCompleted;
    public CreateUnitEvent CreateEnemyUnitEvent;
    public SpawnEnemyInPath SpawnEnemyInPathEvent;
    public List<Waypoint> PathList;

    public int waveNum;
    public bool isCompleted;
    // Start is called before the first frame update
    void Start()
    {
        WavesCompleted.Value = 0;
        isCompleted = false;
        waveNum = 1;
        DoRun.RegisterListener(onDoRun);
        SpawnEnemyInPathEvent.RegisterListener(onSpawnEnemyInPath);
        setContainer();
    }

    private void setContainer() {
        if (segmentContainers.Count < waveNum) {
            isCompleted = true;
            WavesCompleted.Value = 1;
            return;
        }
        currentContainer = segmentContainers[waveNum - 1];
        currentContainer.Init();
    }


    public int onDoRun() {
        if (PauseStatus.Value > 0) return 0; // Don't run if the game is paused
        if (isCompleted) return 0;
        if (currentContainer.isCompleted) {
            waveNum++;
            setContainer();
            return 1;
        }

        currentContainer.Run();
        return 1;
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
