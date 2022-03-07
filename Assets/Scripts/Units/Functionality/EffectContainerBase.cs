using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Functionality/Effects/Base Container")]
// Handles Effect Management functionality for EffectContainers
public class EffectContainerBase : ScriptableObject
{
    public void doAddEffect(IEffectContainer container, Effect whichEffect) {
        if (container.Effects == null) container.Effects = new List<Effect>();
        if (container.EffectsToAdd == null) container.EffectsToAdd = new List<Effect>();

        //put it in our queue to be added after effects run
        container.EffectsToAdd.Add(whichEffect);
    }

    public void doRemoveEffect(IEffectContainer container, Effect whichEffect) {
        container.Effects.Remove(whichEffect);
    }

    public void doRemoveAll(IEffectContainer container) {
        //clear our any effects that haven't been added/initialized yet
        if (container.EffectsToAdd != null && container.EffectsToAdd.Count > 0) container.EffectsToAdd.Clear(); 

        //Clean up any running effects then remove them
        if (container.Effects != null && container.Effects.Count > 0) {
            foreach(var effect in container.Effects) {
                effect.doCleanUp();
            }
        }
    }

    public void doRunEffects(IEffectContainer container) {
        
        //First add any effects that need to be added to our list
        if (container.EffectsToAdd != null && container.EffectsToAdd.Count > 0) {
            foreach(var newEffect in container.EffectsToAdd) {
                container.Effects.Add(newEffect);
                newEffect.doInit(container);
            }

            //empty out our list of effects to add
            container.EffectsToAdd.Clear();
        }

        //Run our effects. this includes any that were added above
        if (container.Effects != null && container.Effects.Count > 0) {
            foreach(var effect in container.Effects) {
                effect.doRun();
            }

            //Remove any effects marked for removal
            //Want to do this after they run and not during
            RemoveAllMarked(container);
        }
    }

    // Remove effects that have been marked to be removed
    private void RemoveAllMarked(IEffectContainer container) {
        var effectsToRemove = container.Effects.Where(m => m.needRemove == true).ToList();
        foreach(var effect in effectsToRemove) {
            effect.doRemove();
        }
    }
}
