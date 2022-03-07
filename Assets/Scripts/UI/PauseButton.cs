using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public IntVariable PauseStatus;
    public bool pauseToggle;

    public void onPressed() {
        if (pauseToggle) {
            pauseToggle = false;
            PauseStatus.Value = 0;
        }else{
            pauseToggle = true;
            PauseStatus.Value = 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseToggle = true;
        onPressed();
    }
}
