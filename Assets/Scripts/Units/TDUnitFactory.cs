using UnityEngine;

public class TDUnitFactory : MonoBehaviour
{
    public CreateUnitEvent CreateEvent;

    private void Start()
    {
        CreateEvent.RegisterListener(onCreateUnitEvent);
    }

    private UnitCreateConfig getUnitTypeConfig(TDUnitTypes unitType)
    {
        UnitCreateConfig config  = new UnitCreateConfig();
        switch (unitType)
        {
            case TDUnitTypes.BASIC_TOWER:
                config.isEnemy = false;
                config.objPrefix = "Tower_";
                config.templateResourceName = "TowerContainer";
                config.needPartyPosition = true;
                break;
            case TDUnitTypes.BASIC_ENEMY:
            default:
                config.isEnemy = true;
                config.objPrefix = "Enemy_";
                config.templateResourceName = "EnemyContainer";
                break;
        }

        return config;
    }

    public Unit onCreateUnitEvent(UnitProfile unitProfile, TDUnitTypes unitType, int partyPosition=-1)
    {
        var config = getUnitTypeConfig(unitType);

        var newUnit = Instantiate(Resources.Load(config.templateResourceName)) as GameObject;
        var unitGfxName = UnitProfile.GetWholeUnitGfxName(unitProfile.unitID);
        newUnit.name = config.objPrefix + unitGfxName;

        // Get graphic resource
        var graphicResourceName = "unitGfx/" + unitGfxName;
        var unitGfx = Object.Instantiate(Resources.Load(graphicResourceName), newUnit.transform) as GameObject;
        unitGfx.name = "unitGfx";

        //Set unit script
        var unitScript = newUnit.GetComponent<Unit>();
        unitScript.doInit(unitProfile);

        // Set party position
        if (config.needPartyPosition && partyPosition != -1) unitScript.partyPos = partyPosition;

        return unitScript;
    }
}
