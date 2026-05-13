using System;
using UnityEngine;

[Serializable]
public class HealSkill : ICombatEffect {
    public int healAmount;
    public bool isPercentageHeal;
    [Range(0f, 1f)]
    public float healthPercentage = 0.2f;
    public float statMultiplier;
    public Attribute healEStatScale;
    
    void ICombatEffect.PrepareEffect(CombatArgs args) {
        var finalHeal = 0;
        if (isPercentageHeal) {
            finalHeal = Mathf.RoundToInt(args.target.Health.Max * healthPercentage);
        } else {
            var healStat = healEStatScale;
            var healStatValue = args.user[healStat] * statMultiplier;

            finalHeal = Mathf.RoundToInt(healAmount + healStatValue);
        }

        args.heal = finalHeal;
        args.critChance = 0;
        args.hitChance = 100;
    }
}