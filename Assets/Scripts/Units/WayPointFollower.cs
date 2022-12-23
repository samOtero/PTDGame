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
    public bool canCaptureCandy;
    public bool hasCandy;
    public CandyUnit carriedCandy;
    public Vector3 directionVector;
    public UnitEvent UnitLeftEvent;
    public IntVariable PauseStatus;
    public CandyRuntimeCollection candyList;
    public BasicEvent DoRun;
    public bool isEventRegistered;

    public void reset(Waypoint startPoint) {
        if (myUnit == null) myUnit = gameObject.GetComponent<Unit>(); //lazy load
        freeRoam = myUnit.profile.freeRoam;
        myUnit.positionRef.transform.position = startPoint.transform.position;
        initialPoint = startPoint;
        turnedAround = false;
        canCaptureCandy = myUnit.profile.canCaptureCandy;
        setNewPoint(initialPoint);
        if (isEventRegistered == false) {
            DoRun.RegisterListener(onDoRun);
            isEventRegistered = true;
        }
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

                //If we have candy then we want to capture it
                if (hasCandy) {
                    carriedCandy.onCaptured();
                    hasCandy = false;
                    carriedCandy = null;
                }
                myUnit.onLeaveLevel();
                if (UnitLeftEvent) UnitLeftEvent.Raise(myUnit);
            }
        }

        getNewRotation(currentPoint, turnedAround);
    }

    public void dropCandy() {
        if (!hasCandy) return;

        carriedCandy.onDrop();
        hasCandy = false;
        carriedCandy = null;
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

    // Check for candy
    private void checkForCandy() {
        //Check for candy in range
        var candyInRange = getCandyInRange();
        if (candyInRange != null) {
            hasCandy = true;
            //Attach candy to unit and set candy as caught
            carriedCandy = candyInRange;
            candyInRange.onPickup();
            candyInRange.transform.parent = myUnit.transform; // TODO: Change this to a particular spot for the candy
            //turn around back to our exit point
            // TODO: potentially refactor this so this logic can exist in one place
            if (!freeRoam && !turnedAround){
                turnedAround = !turnedAround;
                setNewPoint(targetPoint);
            }
        }
    }

    private CandyUnit getCandyInRange() {
        Vector2 unitLoc = new Vector2(myUnit.positionRef.transform.position.x, myUnit.positionRef.transform.position.z);
        Vector2 candyLoc = new Vector2();
        CandyUnit currentTarget;
        float deltaDistance;
        float range = 0.6f; // this should be close enough to the candy to be considered in range
        if (candyList == null) {
            Debug.LogError("Candy List is null on WayPointFollower");
            return null;
        }
        for(var i=0; i<candyList.Items.Count; i++) {
            currentTarget = candyList.Items[i];
            if (currentTarget.canPickup() == false) continue;
            candyLoc.x = currentTarget.transform.position.x;
            candyLoc.y = currentTarget.transform.position.z;
            deltaDistance = Vector2.Distance(unitLoc, candyLoc);
            if (deltaDistance <= range) {
                return currentTarget;
            }
        }

        return null;
    }

    public int onDoRun() {
        if (PauseStatus.Value > 0) return 0;
        if (myUnit == null || myUnit.isBattling == false || myUnit.isAlive == false) return 0; // Don't move if we aren't battling
        if (currentPoint == null) return 0;
        myUnit.positionRef.transform.Translate(directionVector * Time.deltaTime * myUnit.currentSpeed);
        checkIfReachedPoint();
        if (canCaptureCandy && !hasCandy) checkForCandy();
        return 1;
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
