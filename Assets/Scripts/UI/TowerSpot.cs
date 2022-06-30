using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    public TowerSpotVariable selectedTowerSpot;
    public Unit myUnit;

    public void AddUnit(Unit newUnit) {
        // If we are adding the unit that is already there then do nothing
        if (myUnit == newUnit) return;

        // Remove the new unit from it's previous spot
        if (newUnit.currentSpot != null) newUnit.currentSpot.removeUnit(newUnit);

        // If this spot already had a unit
        if (myUnit != null) {
            // Check to see if new unit came from a spot
            if (newUnit.currentSpot != null) {
                // Swap unit spots
                var newUnitSpot = newUnit.currentSpot;
                newUnitSpot.AddUnit(myUnit); // Swap the unit but ignore additional swaps
            }else{
                // Callback unit
                myUnit.removeFromBattle();
            }
        }

        myUnit = newUnit;
        myUnit.currentSpot = this;
        var newPos = transform.position;
        newPos.y = 1.0f;
        myUnit.positionRef.transform.position = newPos;
        myUnit.setIsBattling(true); // When added to a spot the unit is now battling!
    }

    // Remove a unit from this spot
    public void removeUnit(Unit unitToRemove) {
        if (myUnit != unitToRemove) return;
        myUnit = null;
    }

    private void OnMouseEnter() {
        selectedTowerSpot.Value = this;
    }

    private void OnMouseExit() {
        selectedTowerSpot.Value = null;
    }
}
