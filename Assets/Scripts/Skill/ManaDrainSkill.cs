using System;
using UnityEngine;

[Serializable]
public class ManaDrainSkill : ICombatEffect {
    //valor de mana é multiplicado por variavel fixa ou dano causado
    [Range(0f, 1f)]
    public float damagePercentage = 0.2f;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.OnResolve += Steal;
    }

    public void Steal(CombatArgs args) {
        var stealMana = args.result.Mana.Delta * damagePercentage;
        if (stealMana >= 0) return;
        
        var chain = args.Chain(this, args.user);
        chain.mana = -(int)stealMana;
        chain.Resolve();
    }
}