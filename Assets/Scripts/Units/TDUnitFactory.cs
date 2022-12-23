using System.Collections.Generic;
using UnityEngine;

public class TDUnitFactory : MonoBehaviour
{
    public CreateUnitEvent CreateEvent;
    public UnitEvent AddToPoolEvent;
    public List<Unit> UnitList;

    private void Start()
    {
        UnitList = new List<Unit>();
        CreateEvent.RegisterListener(onCreateUnitEvent);
        AddToPoolEvent.RegisterListener(onAddToPool);
    }

    private Unit GetFromPool(UnitProfile profile)
    {
        if (UnitList.Count > 0)
        {
            // TODO: Actually look at enemy profile to check to see if we can use this unit
            // Use the enemyTypeId!!
            var unit = UnitList[0];
            UnitList.RemoveAt(0);
            return unit;
        }

        return null;
    }

    public int onAddToPool(Unit unit)
    {
        UnitList.Add(unit);
        return 1;
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
        //Try to get unit from pool
        var config = getUnitTypeConfig(unitType);

        if (config.isEnemy)
        {
            var poolEnemy = GetFromPool(unitProfile);
            if (poolEnemy != null) return poolEnemy;
        }

        var newUnit = Instantiate(Resources.Load(config.templateResourceName)) as GameObject;
        var unitGfxName = UnitProfile.GetWholeUnitGfxName(unitProfile.unitID);
        newUnit.name = config.objPrefix + unitGfxName;

        // Get graphic resource
        var graphicResourceName = "unitGfx/" + unitGfxName;
        var unitGfx = Instantiate(Resources.Load(graphicResourceName), newUnit.transform) as GameObject;
        unitGfx.name = "unitGfx";

        //Set unit script
        var unitScript = newUnit.GetComponent<Unit>();
        unitScript.doInit(unitProfile);

        // Set party position
        if (config.needPartyPosition && partyPosition != -1) unitScript.partyPos = partyPosition;

        return unitScript;
    }
}
