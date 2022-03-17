using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds the current party's unit references
[CreateAssetMenu(menuName = "Unit/Party")]
public class UnitParty : ScriptableObject
{
    public List<TowerContainer> party;
}
