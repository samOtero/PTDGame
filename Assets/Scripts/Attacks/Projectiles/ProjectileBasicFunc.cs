using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Functionality/Base Projectile")]
public class ProjectileBasicFunc : ScriptableObject
{
    public IntVariable PauseStatus;

    public void faceTarget(Projectile holder) {
        var targetLoc = holder.targetLocation;
        var atkZ = holder.gameObject.transform.position.z;
        var atkX = holder.gameObject.transform.position.x;
        var zDelta = Math.Abs(atkZ - targetLoc.z);
        var xDelta = Math.Abs(atkX - targetLoc.x);
        var newRotation = 0.0f;
        if (zDelta > xDelta) {
            if (atkZ < targetLoc.z) {
                newRotation = 0.0f;
            }else{
                newRotation = 180.0f;
            }
        }else{
            if (atkX > targetLoc.x) {
                newRotation = -90.0f;
            }else{
                newRotation = 90.0f;
            }
        }

        holder.gfx.transform.localEulerAngles = new Vector3(0, newRotation, 0);
    }

    public void doRun(Projectile holder)
    {
        //If we are paused then handle it
        if (CheckPause(holder) == true)
            return;

        //Handle updating our target location

        //Update the target location if needed
        //var needLocationUpdate = false;

        //If our target is alive or if we have a dynamic target then update our destination
       // if (myTarget != null && myTarget.myStatus != unitStatus.dead)
         //   needLocationUpdate = true;
        //else if (myTarget == null && dynamicTarget != null)
          //  needLocationUpdate = true;

        //if (needLocationUpdate == true)
         updateTargetLocation(holder);

        //Move our projectile
        var velocity = GetVelocity(holder);
        holder.transform.Translate(velocity * Time.deltaTime);

        if (holder.faceTarget) faceTarget(holder); //Face our target
        if (CheckIfHitTarget(holder))
            hitTarget(holder);

    }

    /// <summary>
    /// Check to see if we hit our target or any targets in our path
    /// </summary>
    /// <returns></returns>
    private bool CheckIfHitTarget(Projectile holder)
    {
        var gotToTarget = CheckReachedLocation(holder, holder.targetLocation);

        /*
        if (gotToTarget == false)
        {
            if (holder.targetList != null && holder.myTarget == null)
            {
                foreach (var checkTarget in holder.targetList.Items)
                {
                    //Check if projectile reached but only on the x axis since we already filter path and we aren't targetting any in particular
                    if (CheckReachedLocation(holder, checkTarget.positionRef.transform.position))
                    {
                        gotToTarget = true;
                        holder.myTarget = checkTarget; //Set this as our target
                        break;
                    }
                }
            }
        }
        */
        return gotToTarget;
    }

    /// <summary>
    /// Called when we reached our target
    /// </summary>
    /// <param name="holder"></param>
    protected virtual void hitTarget(Projectile holder)
    {
        //If we have any on hit effects to add then let's handle that now
        if (holder.OnHitEffect != null)
        {
            holder.OnHitEffect.doTrigger();

                //Add target to effect before applying it, this is for projectiles that don't know their target beforehand
                //if (holder.myTarget != null && effect.setTarget == true)
                  //  effect.AddTarget(holder.myTarget);

                //var targetUnit = effect.myUnit;
                //If the effect doesn't have a unit set then don't add it, this can happen when projectile doesn't find a target
                //if (targetUnit != null) 
                  //  targetUnit.myFunctionality.AddEffect(targetUnit, effect);
        }
        //Get rid of projectile, probably want to do some kind of pool instead
        holder.doRemove();
        Destroy(holder.gameObject);
    }

    /// <summary>
    /// Handle the game being paused
    /// </summary>
    /// <param name="holder"></param>
    /// <returns></returns>
    protected bool CheckPause(Projectile holder)
    {
        var paused = PauseStatus.Value > 0;

        if (holder.animator) {
            if (paused && holder.paused == false)
                holder.animator.speed = 0;
            else if (paused == false && holder.paused == true)
               holder.animator.speed = 1;
        }
        

        return paused;
    }

    /// <summary>
    /// Set velocity based on targte location and speed
    /// </summary>
    /// <returns></returns>
    private Vector3 GetVelocity(Projectile holder)
    {
        var heading = holder.targetLocation - holder.transform.position;
        var direction = heading / heading.magnitude;
        return direction * holder.speed;
    }

    /// <summary>
    /// Update Target Location based on our dynamic target
    /// </summary>
    private void updateTargetLocation(Projectile holder)
    {
        if (holder.myTarget == null) return;

        //If we have a target and it's dead then don't update our location
        if (holder.myTarget.isTargetable() == false || holder.myTarget.getID() != holder.targetID)
        {
            holder.myTarget = null; //Target is dead let's get rid of it and hit our last target
            return;
        }

         holder.targetLocation = holder.myTarget.getLocation();
    }

    /// <summary>
    /// Check if we reached location on the X axis only
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    private bool CheckReachedLocation(Projectile holder, Vector3 loc, bool onlyX=false)
    {
        double distance = 0;

        if (onlyX)
            distance = Math.Abs(holder.transform.position.x - loc.x);
        else
            distance = Vector2.Distance(holder.transform.position, loc);

        if (distance < holder.speed * Time.deltaTime)
            return true;

        return false;
    }
}
