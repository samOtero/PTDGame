using UnityEngine;

// Object that can be targetable by a projectile
public interface ITargetable
{
   bool isTargetable();
   Vector3 getLocation();
   int getID();
}
