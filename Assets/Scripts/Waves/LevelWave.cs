using System.Collections.Generic;
using UnityEngine;

public class LevelWave : MonoBehaviour
{
    public IntVariable PauseStatus;
    public BasicEvent DoRun;
    public List<WaveSegmentContainer> segmentContainers;
    public WaveSegmentContainer currentContainer;
    
    public int waveNum;
    public bool isCompleted;
    // Start is called before the first frame update
    void Start()
    {
        isCompleted = false;
        waveNum = 1;
        DoRun.RegisterListener(onDoRun);
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
}
