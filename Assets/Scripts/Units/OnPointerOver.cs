using UnityEngine;

public class OnPointerOver : MonoBehaviour
{
    public Unit myUnit;
    public UnitRuntimeCollection HoveringUnits;

    private void Start() {
        myUnit = gameObject.GetComponent<Unit>();
    }
    private void OnMouseEnter() {
        if (myUnit != null)
        HoveringUnits.Add(myUnit);
    }

    private void OnMouseExit() {
        if (myUnit != null)
        HoveringUnits.Remove(myUnit);
    }
}
