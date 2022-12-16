using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object hold the details for a segment of a wave
[CreateAssetMenu(menuName = "Wave/Segment")]
public class Wave_Segment : ScriptableObject
{
   public UnitProfileObj profile;
   public int pathNum;
   public int totalUnits;
   private int unitCount;
   public int startDelay;
   public int betweenDelay;
   private int counter;
   public bool isCompleted;
   private bool isStarted;
   public SpawnEnemyInPath SpawnEnemyInPathEvent;

   public void Init() {
    isCompleted = false;
    counter = startDelay;
    isStarted = false;
    unitCount = totalUnits;
   }

   public void Run() {

    if (isCompleted) return;

    // Handle start delay if any
    if (!isStarted) {
        if (counter == 0) {
            isStarted = true;
            return;
        }
        counter--;
        return;
    }

    if (unitCount == 0) {
        isCompleted = true;
        return;
    }

    // Once our counter is 0, we spawn the unit
    if (counter == 0) {
        unitCount--;
        counter = betweenDelay;
        // Spawn Units
        SpawnEnemyInPathEvent.Raise(profile, pathNum);
        return;
    }

    // Move the counter forward
    counter--;
   }
}
