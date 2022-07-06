using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : MonoBehaviour, IEffectContainer
{
    public List<Effect> Effects {get; set;}
    public List<Effect> EffectsToAdd {get; set;}

     public EffectContainerBase effectBaseFunc;

     public EffectEvent addEffectEvent;
     
     public IntVariable PauseStatus;
     public BasicEvent DoRun;

    void Start() {
         addEffectEvent.RegisterListener(onAddEffectEvent);
         DoRun.RegisterListener(onDoRun);
    }

    public int onDoRun() {
        if (PauseStatus.Value > 0) return 0;
        
        doRunEffects();
        return 1;
    }

    public int onAddEffectEvent(Effect whichEffect) {
        doAddEffect(whichEffect);
        return 1;
    }

    public void doRemoveEffect(Effect whichEffect) {
        effectBaseFunc.doRemoveEffect(this, whichEffect);
    }
    public void doAddEffect(Effect whichEffect){
        effectBaseFunc.doAddEffect(this, whichEffect);
    }

    public void doRunEffects() {
        effectBaseFunc.doRunEffects(this);
    }
}
