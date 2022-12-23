using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyUnit : MonoBehaviour
{
    public CandyRuntimeCollection candyList;
    public Transform CandyContainer;
    public BasicEvent CandyCapturedEvent;
    public bool onFloor;
    // Start is called before the first frame update
    void Start()
    {
        onFloor = true;
        candyList.Add(this);
    }

    public void onDrop() {
        onFloor = true;
        transform.parent = CandyContainer;
    }

    public void onPickup() {
        onFloor = false;
    }

    public void onCaptured() {
        candyList.Remove(this);
        onFloor = false;
        Destroy(gameObject);
        CandyCapturedEvent.Raise();
    }

    public bool canPickup() {
        return onFloor;
    }
}
