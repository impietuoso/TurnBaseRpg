using System;
using UnityEngine;

[Serializable]
public class ManaBurnSkill : ICombatEffect {
    public bool useDamageDealt;
    public int manaBurn;
    [Range(0f, 1f)]
    public float damagePercentage;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        if (useDamageDealt) args.OnResolve += GetDamage;
        else args.mana = -manaBurn;
    }

    public void GetDamage(CombatArgs args) {
        var burn = args.result.Health.Delta * damagePercentage;
        var chain = args.Chain(this);
        chain.mana = -(int)burn;
        chain.Resolve();
    }
}