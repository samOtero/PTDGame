using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBtn : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public IntVariable PauseState;
    public Canvas buttonCanvas;
    public TowerSpotVariable selectedTowerSpot;
    public UnitProfile profile;
    public Unit myUnit;
    public GameObject TowerTemplate;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        selectedTowerSpot.Value = null; //reset any tower spot
    }

    // Create enemy unit from a profile
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

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition = originalPosition;
        if (selectedTowerSpot.Value != null) {
            if (myUnit == null) myUnit = CreateTower(profile);
            selectedTowerSpot.Value.AddUnit(myUnit);
        }
        PauseState.Value -= 1;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        PauseState.Value += 1;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / buttonCanvas.scaleFactor;
    }
}
