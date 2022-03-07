using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Projectile : EffectForUnit
{
    protected Effect_Trigger_Remove onHitEffect;
    protected string projectileGfxResource;
    public Effect_Projectile(string projectileGfxResource, Effect_Trigger_Remove onHitEffect, Unit target, Unit fromWho, float delay=0, string tag=null, string tag2=null): base(target, fromWho, delay, tag, tag2) {
        this.projectileGfxResource = projectileGfxResource;
        this.onHitEffect = onHitEffect;
    }

    protected override void doRunCustom()
    {
        if (needRemove)
            return;

        // Remove this effect
        needRemove = true;

        //By the time this effect tries to trigger our target might no longer be available
        //so let's check for that
        if (target.getID() != targetID || target.isTargetable() == false) return;

        //Create and send projectile
        const string projectileContainerName = "ProjectileContainer";
        var projectile = Object.Instantiate(Resources.Load(projectileContainerName)) as GameObject;
        var projectileGfx = Object.Instantiate(Resources.Load(projectileGfxResource), projectile.transform) as GameObject;

        //Set starting position
        var newLoc = fromWho.getLocation() + new Vector3(0f, 1f, 0f); //Default position

        projectile.transform.position = newLoc;

        //Get projectile script
        var projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.init(target, onHitEffect, projectileGfx, 10.0f);

    }
}
