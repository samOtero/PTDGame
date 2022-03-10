using UnityEngine;

public class DragTower : MonoBehaviour
{
    public IntVariable PauseStatus;
    public TowerSpotVariable selectedTowerSpot;
    public GameObject TowerTemplate;
    public UnitOrProfileEvent StartTowerDrag;
    public BasicEvent EndTowerDrag;
    public BasicEvent DoTowerDrag;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public Unit draggingUnit;
    public UnitProfile draggingUnitProfile;
    public Canvas UICanvas;
    public float canvasScaleFactor;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        canvasScaleFactor = UICanvas.scaleFactor;
        StartTowerDrag.RegisterListener(onStartTowerDrag);
        DoTowerDrag.RegisterListener(onDragTower);
        EndTowerDrag.RegisterListener(onEndTowerDrag);
    }

    private int onStartTowerDrag(Unit unit, UnitProfile profile) {
        draggingUnit = unit;
        draggingUnitProfile = profile;
        rectTransform.anchoredPosition = Input.mousePosition / canvasScaleFactor;
        PauseStatus.Value++;
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

        return unitScript;
    }

    private int onEndTowerDrag()  {

        // If we are dragging on top of a tower spot then add the unit to that spot
        if (selectedTowerSpot.Value != null) {
            // If we haven't instatiated a tower yet then create one
            if (draggingUnit == null) draggingUnit = CreateTower(draggingUnitProfile);
            selectedTowerSpot.Value.AddUnit(draggingUnit);
        }

        draggingUnit = null;
        draggingUnitProfile = null;
        rectTransform.anchoredPosition = originalPosition;
        PauseStatus.Value--;
        return 1;
    }
}
