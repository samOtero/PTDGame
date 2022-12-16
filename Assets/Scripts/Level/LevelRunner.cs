using UnityEngine;

public class LevelRunner : MonoBehaviour
{
    public BasicEvent DoRun;
    public IntEvent SetRunSpeed;
    public int levelSpeed;
    // Start is called before the first frame update
    void Start()
    {
        SetRunSpeed.RegisterListener(onSetRunSpeed);
    }

    public int onSetRunSpeed(int newSpeed) {
        levelSpeed = newSpeed;
        return 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Run our level
        DoRun.Raise();
        if (levelSpeed >= 2) DoRun.Raise();
        if (levelSpeed >= 3) DoRun.Raise();
        if (levelSpeed >= 4) DoRun.Raise();
    }
}
