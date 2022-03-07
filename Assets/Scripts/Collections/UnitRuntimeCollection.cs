using UnityEngine;

/// <summary>
/// Basic list for units
/// </summary>
[CreateAssetMenu(menuName = "Collections/Unit")]
public class UnitRuntimeCollection : RuntimeCollection<Unit>, ISerializationCallbackReceiver
{
    public void OnAfterDeserialize()
    {
        Reset(); //clear our list before trying to save anything here
    }

    public void OnBeforeSerialize()
    {
        //nothing
    }

    public void Reset()
    {
        Items.Clear();
    }
}
