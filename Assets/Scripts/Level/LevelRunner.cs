using UnityEngine;

public class LevelRunner : MonoBehaviour
{
    public BasicEvent DoRun;
    public IntEvent SetRunSpeed;
    public int levelSpeed;
    public int count;
    public int countAmount = 6; // Slow motion amount
    // Start is called before the first frame update
    void Start()
    {
        resetCount();
        SetRunSpeed.RegisterListener(onSetRunSpeed);
    }

    public int onSetRunSpeed(int newSpeed) {
        levelSpeed = newSpeed;
        resetCount();
        return 1;
    }

    private void resetCount()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
            // Run our level
            DoRun.Raise();
            if (levelSpeed >= 2) DoRun.Raise();
            if (levelSpeed >= 3) DoRun.Raise();
            if (levelSpeed >= 4) DoRun.Raise();
            if (levelSpeed == -1)
            {
                count = countAmount;
            }
        }
        else
        {
            count--;
        }
    }
}
