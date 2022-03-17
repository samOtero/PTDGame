using UnityEngine;

public class OnDrag : MonoBehaviour
{
    public Unit myUnit;

    public IntEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        myUnit = GetComponent<Unit>();
    }

    void OnMouseDown()
    {
        StartTowerDrag.Raise(myUnit.partyPos); // Initialize tower dragging
    }

    void OnMouseUp() {
        EndTowerDrag.Raise(); // end tower dragging
    }

    void OnMouseDrag()
    {
        DoTowerDrag.Raise(); // do tower dragging
    }
}
