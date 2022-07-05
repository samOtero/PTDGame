using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRunner : MonoBehaviour
{
    public BasicEvent DoRun;
    public int levelSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Run our level
        DoRun.Raise();
        if (levelSpeed > 1) DoRun.Raise();
        if (levelSpeed > 2) DoRun.Raise();
        if (levelSpeed > 3) DoRun.Raise();
    }
}
