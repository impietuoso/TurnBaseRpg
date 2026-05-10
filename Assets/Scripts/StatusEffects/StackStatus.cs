using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve, Serializable]
public abstract class StackStatus : Status {
    [Tooltip("-1 for infinity")]
    public float duration = 10;
    public int maxStacks = 5;
    public int Stacks { get; private set; }

    public override void Apply(Character target) {
        Stacks = 1;
        OnStacksChanged(target);
    }

    public override void Remove(Character target) {
        Stacks = 0;
        OnStacksChanged(target);
    }

    public override void Stack(Character target, Status other) {
        if (other is not StackStatus ss) return;
        duration = Math.Max(duration, ss.duration);
        if (Stacks >= maxStacks) return;
        Stacks++;
        OnStacksChanged(target);
    }

    public override void Tick(Character target, float deltaTime) {
        if (duration < 0) return;
        duration -= deltaTime;
        if (duration <= 0)
            target.StatusEffectList.Remove(source);
    }

    private void RemoveStack(Character target) {
        Stacks--;
        if (Stacks == 0)
            target.StatusEffectList.Remove(source);
        else OnStacksChanged(target);
    }

    protected abstract void OnStacksChanged(Character target);
}