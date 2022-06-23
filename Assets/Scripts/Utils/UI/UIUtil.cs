using UnityEngine;

// Utility functions related to the UI
public static class UIUtil {
    // Sets a gameobject and all it's children's layer to the UI Layer so they can render correctly
    public static void SetUILayer(GameObject obj) {
        obj.layer = LayerMask.NameToLayer("UI");

        foreach(Transform child in obj.transform) {
            SetUILayer(child.gameObject);
        }
    }

    // Will create and add UI Unit Graphic based on a given unitID to a given container
    // Used for Tower Button and Drag Unit 
    public static void setUnitGfx(int unitID, Transform container) {
        //First Need to delete anything that is already there
        foreach (Transform child in container) {
             GameObject.Destroy(child.gameObject);
        }
        // Get graphic resource
        var unitGfxName = UnitProfile.GetUnitGfxName(unitID);
        var graphicResourceName = "unitGfx/gfx/"+unitGfxName;
        var unitGfx = Object.Instantiate(Resources.Load(graphicResourceName), container) as GameObject;
        UIUtil.SetUILayer(unitGfx); // Set UnitGfx to the UI layer so it will render correctly
        unitGfx.name = "unitGfx";
    }
}


