using UnityEngine;

/// <summary>
/// Basic list for candies
/// </summary>
[CreateAssetMenu(menuName = "Collections/Candy")]
public class CandyRuntimeCollection : RuntimeCollection<CandyUnit>, ISerializationCallbackReceiver
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
