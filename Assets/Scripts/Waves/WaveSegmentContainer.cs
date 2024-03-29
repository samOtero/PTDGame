using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object hold the details for a group of segments of a wave
[CreateAssetMenu(menuName = "Wave/Segment Container")]
public class WaveSegmentContainer : ScriptableObject
{
   public List<Wave_Segment> segments;
   public bool isCompleted;
    public int startDelay;
    public int counter;

    public void Init()
   {
        counter = startDelay;
        isCompleted = false;
      for (int i = 0; i < segments.Count; i++)
      {
         segments[i].Init();
      }
   }

   public void Run() {
        if (isCompleted) return;
        if (counter > 0)
        {
            counter--;
            return;
        }

        var allSegmentsCompleted = true;
        for (int i = 0; i < segments.Count; i++) {
             segments[i].Run();
             if (segments[i].isCompleted == false) {
                allSegmentsCompleted = false;
             }
        }
        if (allSegmentsCompleted) isCompleted = true;
   }
}
