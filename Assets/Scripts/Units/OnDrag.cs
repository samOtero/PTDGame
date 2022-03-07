using UnityEngine;

public class OnDrag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public Unit myUnit;
    public IntVariable PauseStatus;
    // Start is called before the first frame update
    void Start()
    {
        myUnit = GetComponent<Unit>();
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(myUnit.positionRef.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = myUnit.positionRef.transform.position - GetMouseAsWorldPoint();
        PauseStatus.Value++;
    }

    void OnMouseUp() {
        PauseStatus.Value--;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        myUnit.positionRef.transform.position = GetMouseAsWorldPoint() + mOffset;
    }
}
