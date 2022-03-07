using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    public Unit myUnit;
    public Direction currentDirection;
    public Waypoint initialPoint;
    public Waypoint currentPoint;
    public Waypoint targetPoint;
    public bool turnedAround;
    public bool freeRoam;
    public bool dontLeave;
    public Vector3 directionVector;
    public UnitEvent UnitLeftEvent;
    public IntVariable PauseStatus;

    public void reset(Waypoint startPoint) {
        if (myUnit == null) myUnit = gameObject.GetComponent<Unit>(); //lazy load
        freeRoam = myUnit.profile.freeRoam;
        myUnit.positionRef.transform.position = startPoint.transform.position;
        initialPoint = startPoint;
        turnedAround = false;
        setNewPoint(initialPoint);
    }

    private void setNewPoint(Waypoint newPoint) {
        if (myUnit == null) return;
        if (newPoint == null) return;

        //Set direction
        currentPoint = newPoint;
        targetPoint = GetTargetPoint(currentPoint);

        //Check to see if we got to the end
        if (targetPoint == null) {
            //If we can turn around then we want to cycle
            if (!freeRoam && (dontLeave || !turnedAround)) {
                turnedAround = !turnedAround;
                targetPoint = GetTargetPoint(currentPoint);
            }else {
                //If we reached our end
                if (UnitLeftEvent) UnitLeftEvent.Raise(myUnit);
            }
        }

        getNewRotation(currentPoint, turnedAround);
    }

    private Waypoint GetTargetPoint(Waypoint whichPoint) {
        return turnedAround ? whichPoint.prev : whichPoint.next;
    }

    private void getNewRotation(Waypoint whichPoint, bool goingBackwards) {
        Direction newDirection = goingBackwards ? whichPoint.backward : whichPoint.forward;
        currentDirection = newDirection;
        var rotation = myUnit.getRotationFromDirection(newDirection);
        myUnit.faceDirection(newDirection);
        directionVector = Quaternion.Euler(0,rotation,0) * Vector3.forward;
    }

    

    void Update() {
        if (PauseStatus.Value > 0) return;
        if (myUnit == null) return;
        if (currentPoint == null) return;
        myUnit.positionRef.transform.Translate(directionVector * Time.deltaTime * myUnit.currentSpeed);
        checkIfReachedPoint();
    }

    private bool checkIfReachedPoint() {
        if (targetPoint == null) return true;
        bool reachedPoint = false;
        if (currentDirection == Direction.NORTH && myUnit.positionRef.transform.position.z >= targetPoint.transform.position.z)
            reachedPoint = true;
        else  if (currentDirection == Direction.SOUTH && myUnit.positionRef.transform.position.z <= targetPoint.transform.position.z)
            reachedPoint = true;
        else if (currentDirection == Direction.EAST && myUnit.positionRef.transform.position.x >= targetPoint.transform.position.x)
            reachedPoint = true;
        else  if (currentDirection == Direction.WEST && myUnit.positionRef.transform.position.x <= targetPoint.transform.position.x)
            reachedPoint = true;

        if (reachedPoint == true)
            setNewPoint(targetPoint);
            
        return reachedPoint;
    }
}
