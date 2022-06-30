using UnityEngine;

public class DragTower : MonoBehaviour
{
    public IntVariable PauseStatus;
    public TowerSpotVariable selectedTowerSpot;
    public IntEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public Unit draggingUnit;
    public UnitProfile draggingUnitProfile;
    public Canvas UICanvas;
    public float canvasScaleFactor;
    public UnitParty currentParty;
    public bool isDragging;
    public int partyPosition;
    public GameObject UnitGfxContainer;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        canvasScaleFactor = UICanvas.scaleFactor;
        StartTowerDrag.RegisterListener(onStartTowerDrag);
        DoTowerDrag.RegisterListener(onDragTower);
        EndTowerDrag.RegisterListener(onEndTowerDrag);
        isDragging = false;
    }

    private int onStartTowerDrag(int pos) {
        partyPosition = pos;
        //If we have a unit for that tower position then let's start dragging it
        if (currentParty.party != null && partyPosition < currentParty.party.Count) {
            draggingUnit = currentParty.party[partyPosition].unit;
            draggingUnitProfile = currentParty.party[partyPosition].profile;
            rectTransform.anchoredPosition = Input.mousePosition / canvasScaleFactor;
            UIUtil.setUnitGfx(draggingUnitProfile.unitID, UnitGfxContainer.transform); // Set drag Unit Graphic

            // Set scale but graphic based on profile
            var unitBaseInfo = UnitProfile.GetBaseInfo(draggingUnitProfile.unitID);
            UnitGfxContainer.transform.localScale = new Vector3(unitBaseInfo.UIScale, unitBaseInfo.UIScale, unitBaseInfo.UIScale);
            
            PauseStatus.Value++;
            isDragging = true;
        }
        return 1;
    }

    private int onDragTower() {
        rectTransform.anchoredPosition = Input.mousePosition / canvasScaleFactor;
        return 1;
    }

    private int onEndTowerDrag()  {

        // If we are dragging on top of a tower spot then add the unit to that spot
        if (selectedTowerSpot.Value != null) {
            selectedTowerSpot.Value.AddUnit(draggingUnit);
        }else{
            // we we drop the unit outside a tower spot then call it back
            draggingUnit.removeFromBattle();
        }

        draggingUnit = null;
        draggingUnitProfile = null;
        rectTransform.anchoredPosition = originalPosition;
        PauseStatus.Value--;
        return 1;
    }
}
