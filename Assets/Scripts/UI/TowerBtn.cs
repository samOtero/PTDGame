using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IntEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    public Text UnitNickName;
    public int partyPosition;
    public UnitParty currentParty;
    public UnitProfile currentProfile;
    public GameObject unitGfxContainer;
    public bool isEmpty;

    private void Start() {
        init();
    }

    public void init()  {
        //Use party position to init our graphic and nickname
        isEmpty = true;
        if (currentParty.party != null && partyPosition < currentParty.party.Count) {
            currentProfile = currentParty.party[partyPosition].profile; // Grab profile from our party object
            isEmpty = false;
            UnitNickName.text = currentProfile.nickname; // Assign unit's nickname to button text
            UIUtil.setUnitGfx(currentProfile.unitID, unitGfxContainer.transform); // Set Button Unit Graphic
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        //Stop Dragging the tower
        EndTowerDrag.Raise();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        StartTowerDrag.Raise(partyPosition); // Start dragging the tower
    }

    public void OnDrag(PointerEventData eventData) {
        //Drag the tower
        DoTowerDrag.Raise();
    }
}
