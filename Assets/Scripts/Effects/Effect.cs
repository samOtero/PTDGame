using System.Collections.Generic;
using UnityEngine;
public interface IEffectContainer {
    List<Effect> Effects {get; set;}
    List<Effect> EffectsToAdd {get; set;}
    void doRemoveEffect(Effect whichEffect);
    void doAddEffect(Effect whichEffect);

    void doRunEffects();
}

public class Effect
{
    protected List<Effect> stack;
    protected List<Effect> endStack;
    public bool needRemove;
    protected bool isPaused;

    //Delay timer for effect
    protected float delay;

    // Object which is holding this effect
    protected IEffectContainer container;

    protected string tag;
    protected string tag2;

    public Effect(float delay=0, string tag=null, string tag2=null) {
        this.delay = delay;
        this.tag = tag;
        this.tag2 = tag2;
    }

    public void addToStack(Effect newEffect) {
        if (stack == null ) stack = new List<Effect>();
        stack.Add(newEffect);
    }

    public void addToEndStack(Effect newEffect) {
        if (endStack == null) endStack = new List<Effect>();
        endStack.Add(newEffect);
    }

    public void doPause() {
        isPaused = true;
        doPauseCustom();
        if (stack != null && stack.Count > 0) {
            foreach(var effect in stack) {
                effect.doPause();
            }
        }
    }
    
    protected virtual void doPauseCustom() {
        // Implement in subclasses as needed
    }

    public void doUnPause() {
        isPaused = false;
        doUnPauseCustom();
        if (stack != null && stack.Count > 0) {
            foreach(var effect in stack) {
                effect.doUnPause();
            }
        }
    }
    
    protected virtual void doUnPauseCustom() {
        // Implement in subclasses as needed
    }

    public void doRun() {
        if (delay > 0) {
            delay -= Time.deltaTime;
            return;
        }

        doRunCustom();

        if (stack != null && stack.Count > 0){
            foreach(var effect in stack) {
                effect.doRun();
            }
        }
    }

    protected virtual void doRunCustom() {
        //Implement in subclasses as needed
    }

    public void doInit(IEffectContainer container) {
        this.container = container;
        doInitCustom(container);
        if (stack != null && stack.Count > 0){
            foreach(var effect in stack) {
                effect.doInit(container);
            }
        }
    }

    protected virtual void doInitCustom(IEffectContainer container) {
        //Implement in subclasses
    }

    public void doCleanUp() {
        needRemove = true;
        doCleanUpCustom();

        if (stack != null && stack.Count > 0){
            foreach(var effect in stack) {
                effect.doCleanUp();
            }
        }
    }

    protected virtual void doCleanUpCustom() {
        //Implement in subclass
    }

    public void doRemove() {
        doRemoveCustom();

        if (stack != null && stack.Count > 0){
            foreach(var effect in stack) {
                effect.doRemove();
            }
        }

        container.doRemoveEffect(this);

        if (endStack != null && endStack.Count > 0) {
            foreach(var effect in endStack) {
                effect.addSelfToContainer(container);
            }
        }
    }

    protected virtual void doRemoveCustom() {
        //Implement in subclass
    }

    public void addSelfToContainer(IEffectContainer newContainer) {
        newContainer.doAddEffect(this);
    }

}
