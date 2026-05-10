using System;
using UnityEngine.Scripting;

[Preserve, Serializable]
public abstract class StackTickStatus : Status {
    public float duration = 10;
    public float maxDuration = 30;
    public float tickDelay = 1;
    public float timer;
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
        if (other is not TickStatus ts) return;
        duration = Math.Min(duration + ts.duration, maxDuration);
        if (Stacks >= maxStacks) return;
        Stacks++;
        OnStacksChanged(target);
    }

    public override void Tick(Character target, float deltaTime) {
        timer += deltaTime;
        if (timer >= tickDelay) {
            timer -= tickDelay;
            OnTick(target);
        }

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

    protected abstract void OnTick(Character target);
}