using System;
using UnityEngine;

[Serializable]
public class LifeStealSkill : ICombatEffect {
    [Range(0f, 1f)]
    public float damagePercentage = 0.2f;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.OnResolve += Steal;
    }

    public void Steal(CombatArgs args) {
        args.OnResolve -= Steal;
        var stealHeal = args.result.Health.Delta * damagePercentage;
        if (stealHeal == 0) return;

        var newArgs = args.Chain(this, args.user);
        newArgs.heal = (int)stealHeal;
        newArgs.Resolve();
    }
}