using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedBtn : MonoBehaviour, IPointerUpHandler
{
    public IntEvent SetRunSpeed;
    public int whichLevelSpeed;

    public void OnPointerUp(PointerEventData eventData) {
        SetRunSpeed.Raise(whichLevelSpeed);
    }
}
