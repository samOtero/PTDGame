using UnityEngine;

public class DisposeUnit : MonoBehaviour
{
    public UnitEvent UnitLeftEvent;

    private void Start() {
        if (UnitLeftEvent) UnitLeftEvent.RegisterListener(UnitLeftLevel);
    }

    public int UnitLeftLevel(Unit whichUnit) {
        Destroy(whichUnit.gameObject);
        return 0;
    }
}
