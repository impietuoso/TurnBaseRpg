using System;
using UnityEngine;

[Serializable]
public class DamageSkill : ISkillEffect {
    public int baseDamage;
    public bool ignoreShield;
    public bool isPercentageDamage;
    [Range(0f, 1f)]
    public float healthPercentage = 0.2f;
    [Range(0, 100)]
    public int hitChance;
    [Range(0, 100)]
    public int criticalChance;
    public float statMultiplier;
    public StatName damageStatScale;
    public float damageRange = 0.15f;

    public void Prepare(CombatArgs args) {
        int finalDamage = 0;
        if (isPercentageDamage) {
            finalDamage = Mathf.RoundToInt(args.target.derivedStats.health.maxValue * healthPercentage);
        } else {
            float rangedDamage = UnityEngine.Random.Range(1 - damageRange, 1 + damageRange);
            
            var damageStat = damageStatScale;
            var damageStatValue = args.user.stats[damageStat] * statMultiplier;

            finalDamage = Mathf.RoundToInt((baseDamage + damageStatValue) * rangedDamage);
        }

        args.skillElement = args.skill.element;
        args.ignoreShield = ignoreShield;
        args.damage = finalDamage;
        args.criticalChance = criticalChance;
        args.hitChance = hitChance;
    }
}