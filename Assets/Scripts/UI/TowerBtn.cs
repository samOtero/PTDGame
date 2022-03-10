using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBtn : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public UnitProfile profile;
    public Unit myUnit;
    public UnitOrProfileEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        //Stop Dragging the tower
        EndTowerDrag.Raise();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        StartTowerDrag.Raise(myUnit, profile); // Start dragging the tower
    }

    public void OnDrag(PointerEventData eventData) {
        //Drag the tower
        DoTowerDrag.Raise();
    }
}
