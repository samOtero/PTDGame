using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;
    public UnitEvent UnitDefeatedEvent;
    public UnitEvent UnitCapturedEvent;
    public BasicEvent CandyCapturedEvent;
    public IntVariable WavesCompleted;
    public IntVariable LevelOver;
    public UnitRuntimeCollection EnemyList;
    public CandyRuntimeCollection CandyList;
    public UnitEvent AddToPoolEvent;
    public IntEvent SetRunSpeed;
    // Start is called before the first frame update
    void Start()
    {
        UnitLeftEvent.RegisterListener(onUnitLeftLevelEvent);
        UnitDefeatedEvent.RegisterListener(onUnitDefeated);
        UnitCapturedEvent.RegisterListener(onUnitCaptured);
        CandyCapturedEvent.RegisterListener(onCandyCaptured);
    }

    public int onCandyCaptured()
    {
        checkForEndOfGame();
        return 0;
    }

    public int onUnitLeftLevelEvent(Unit whichUnit)
    {
        checkForEndOfGame();
        AddToPoolEvent.Raise(whichUnit);
        return 0;
    }

    public int onUnitDefeated(Unit whichUnit)
    {
        checkForEndOfGame();
        AddToPoolEvent.Raise(whichUnit);
        return 0;
    }

    public int onUnitCaptured(Unit whichUnit)
    {
        checkForEndOfGame();
        AddToPoolEvent.Raise(whichUnit);
        return 0;
    }

    private void checkForEndOfGame()
    {
        // Check if we already dealt with it
        if (LevelOver.Value != 0) return;
        // Check for loss first
        if (CandyList.Items.Count == 0)
        {
            // Delay a bit before showing losing screen
            Debug.Log("Lose the game!!! :(");
            SetRunSpeed.Raise(-1); // Slow motion mode
            LevelOver.Value = -1;
            return;
        }

        // TODO: Are all your units defeated check

        if (WavesCompleted.Value == 0) return;
        bool wonGame = true;
        int i;
        Unit enemy;

        for(i = 0; i<EnemyList.Items.Count; i++)
        {
            enemy = EnemyList.Items[i];
            if (enemy.isAlive && enemy.isBattling)
            {
                wonGame = false;
                break;
            }
        }

        if (wonGame)
        {
            LevelOver.Value = 1;
            // Delay a bit before showing screen
            // Show win screen
            Debug.Log("Won the game!!!!");
        }
    }
}
