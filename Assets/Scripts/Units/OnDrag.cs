using UnityEngine;

public class OnDrag : MonoBehaviour
{
    public Unit myUnit;

    public UnitOrProfileEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        myUnit = GetComponent<Unit>();
    }

    void OnMouseDown()
    {
        StartTowerDrag.Raise(myUnit, myUnit.profile); // Initialize tower dragging
    }

    void OnMouseUp() {
        EndTowerDrag.Raise(); // end tower dragging
    }

    void OnMouseDrag()
    {
        DoTowerDrag.Raise(); // do tower dragging
    }
}
