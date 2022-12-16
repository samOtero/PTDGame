using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IntEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    public Text UnitNickName;
    public Text UnitLevel;
    public GameObject ExpGroup;
    public Text UnitExp;
    public Transform expBar;
    public int partyPosition;
    public UnitParty currentParty;
    public UnitProfile currentProfile;
    public GameObject unitGfxContainer;
    public bool isEmpty;
    public IntEvent AddedToParty;
    public UnitEvent UnitGotExperience;

    private void Start() {
        AddedToParty.RegisterListener(onAddedToParty);
        UnitGotExperience.RegisterListener(onUnitGotExperience);
        init();
    }

    public void init()  {
        //Use party position to init our graphic and nickname
        if (currentParty.party != null && partyPosition < currentParty.party.Count) {
            var currentPartyUnit = currentParty.party[partyPosition];
            currentProfile = currentPartyUnit.getUnitProfile(); // Grab profile from our party object
            if (currentProfile == null) {
                isEmpty = true;
                UnitNickName.text = "Empty";
                UnitLevel.text = "";
                UnitExp.text = "";
                ExpGroup.SetActive(false);
            }else{
                isEmpty = false;
                UnitNickName.text = currentProfile.nickname; // Assign unit's nickname to button text
                updateExperience(); // Update level and exp bar
                UIUtil.setUnitGfx(currentProfile.unitID, unitGfxContainer.transform); // Set Button Unit Graphic
                // Set scale but graphic based on profile
                var unitBaseInfo = UnitProfile.GetBaseInfo(currentProfile.unitID);
                unitGfxContainer.transform.localScale = new Vector3(unitBaseInfo.UIScale, unitBaseInfo.UIScale, unitBaseInfo.UIScale);
            }
        }
    }

    public int onUnitGotExperience(Unit whichUnit) {
        if (whichUnit.profile != currentProfile) return 0; // only interested in my unit
        //Update level and experience bar
        updateExperience();
        return 1;
    }

    private void updateExperience() {
        UnitLevel.text = "Lvl " + currentProfile.lvl;
        ExpGroup.SetActive(true);
        UnitExp.text = "Exp";
        var newScale = expBar.localScale;
        newScale.x = currentProfile.experiencePercent;
        expBar.localScale = newScale;
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
