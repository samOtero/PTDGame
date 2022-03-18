using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{

    public int Value;
    public int initialValue;

    private void Reset() {
        Value = initialValue;
    }

    public void OnAfterDeserialize()
    {
        Reset();
    }

     public void OnBeforeSerialize()
    {
        //nothing
    }
}