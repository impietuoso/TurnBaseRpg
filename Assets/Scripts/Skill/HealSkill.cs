using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealSkill : ISkillEffect {
    public int healAmount;
    public bool isPercentageHeal;
    [Range(0f, 1f)]
    public float healthPercentage = 0.2f;
    public float statMultiplier;
    public StatName healStatScale;
    
    public void Prepare(CombatArgs args) {
        int finalHeal = 0;
        if (isPercentageHeal) {
            finalHeal = Mathf.RoundToInt(args.target.derivedStats.health.maxValue * healthPercentage);
        } else {
            var healStat = healStatScale;
            var healStatValue = args.user.stats[healStat] * statMultiplier;

            finalHeal = Mathf.RoundToInt(healAmount + healStatValue);
        }

        args.heal = finalHeal;
        args.criticalChance = 0;
        args.hitChance = 100;
    }
}