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
    public IntEvent AddedToParty;

    private void Start() {
        AddedToParty.RegisterListener(onAddedToParty);
        init();
    }

    public void init()  {
        //Use party position to init our graphic and nickname
        if (currentParty.party != null && partyPosition < currentParty.party.Count) {
            currentProfile = currentParty.party[partyPosition].profile; // Grab profile from our party object
            if (currentProfile == null) {
                isEmpty = true;
                UnitNickName.text = "Empty";
            }else{
                isEmpty = false;
                UnitNickName.text = currentProfile.nickname; // Assign unit's nickname to button text
                UIUtil.setUnitGfx(currentProfile.unitID, unitGfxContainer.transform); // Set Button Unit Graphic
            }
        }
    }

    // Called when a unit is added to the party mid level
    public int onAddedToParty(int partyIndex) {
        // If we are empty and we added a unit here then init our graphic and nickname
        if (isEmpty && partyIndex == partyPosition) {
            init();
        }
        return 0;
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (isEmpty) return; // don't do it if we are empty
        //Stop Dragging the tower
        EndTowerDrag.Raise();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (isEmpty) return; // don't do it if we are empty
        StartTowerDrag.Raise(partyPosition); // Start dragging the tower
    }

    public void OnDrag(PointerEventData eventData) {
        if (isEmpty) return; // don't do it if we are empty
        //Drag the tower
        DoTowerDrag.Raise();
    }
}
