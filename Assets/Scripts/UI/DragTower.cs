using UnityEngine;

public class DragTower : MonoBehaviour
{
    public IntVariable PauseStatus;
    public TowerSpotVariable selectedTowerSpot;
    public GameObject TowerTemplate;
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
    public bool needToCreate;
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
            needToCreate = !currentParty.party[partyPosition].hasBeenCreated;
            rectTransform.anchoredPosition = Input.mousePosition / canvasScaleFactor;
            UIUtil.setUnitGfx(draggingUnitProfile.unitID, UnitGfxContainer.transform); // Set drag Unit Graphic
            PauseStatus.Value++;
            isDragging = true;
        }
        return 1;
    }

    private int onDragTower() {
        rectTransform.anchoredPosition = Input.mousePosition / canvasScaleFactor;
        return 1;
    }

    // Create unit from a profile
    // TODO: Move this to it's own class
    private Unit CreateTower(UnitProfile profile) {
        var unitTemplate = TowerTemplate; //Would get which template we need from the profile, for now we just have only one
        var newUnit = Instantiate(unitTemplate);
        var unitGfxName = UnitProfile.GetUnitGfxName(profile.unitID);
        newUnit.name = "Tower_"+unitGfxName;

        // Get graphic resource
        var graphicResourceName = "unitGfx/"+unitGfxName;
        var unitGfx = Object.Instantiate(Resources.Load(graphicResourceName), newUnit.transform) as GameObject;
        unitGfx.name = "unitGfx";

        //Set unit script
        var unitScript = newUnit.GetComponent<Unit>();
        unitScript.doInit(profile);
        unitScript.partyPos = partyPosition;

        return unitScript;
    }

    private int onEndTowerDrag()  {

        // If we are dragging on top of a tower spot then add the unit to that spot
        if (selectedTowerSpot.Value != null) {
            // If we haven't instatiated a tower yet then create one
            if (needToCreate) {
                draggingUnit = CreateTower(draggingUnitProfile);
                //Store the unit in the party reference
                currentParty.party[partyPosition].unit = draggingUnit;
                currentParty.party[partyPosition].hasBeenCreated = true;
                needToCreate = false;
            } 
            selectedTowerSpot.Value.AddUnit(draggingUnit);
        }

        draggingUnit = null;
        draggingUnitProfile = null;
        rectTransform.anchoredPosition = originalPosition;
        PauseStatus.Value--;
        return 1;
    }
}
