using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFadeUnit : Effect
{
    public Unit fadeTarget;
    public bool didFade;

     public EffectFadeUnit(Unit fadeTarget, float delay=0, string tag=null, string tag2=null): base(delay, tag, tag2) {
        this.fadeTarget = fadeTarget;
    }

    protected override void doRunCustom()
    {
        if (needRemove)
            return;

        if (didFade) return;

        if (fadeTarget.isAlive == false) return;
        
        fadeTarget.doFade(0.2f);
        didFade = true;
    }

    protected override void doRemoveCustom()
    {
       if (didFade) fadeTarget.doFade(1.0f);
    }
}
