using System;
using UnityEngine.Scripting;

[Preserve, Serializable]
public abstract class TickStatus : Status {
    public float duration = 10;
    public float maxDuration = 30;
    public float tickDelay = 1;
    public float timer;

    public override void Stack(Character target, Status other) {
        if (other is not TickStatus ts) return;
        duration = Math.Min(duration + ts.duration, maxDuration);
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

    protected abstract void OnTick(Character target);
}