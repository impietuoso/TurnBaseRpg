using System;
using UnityEngine;

[Serializable]
public class DamageSkill : ISkillEffect {
    public Element element;
    public int baseDamage;
    public bool ignoreShield;
    public bool isPercentageDamage;
    [Range(0f, 1f)] public float healthPercentage = 0.2f;
    [Range(0, 100)] public int hitChance;
    [Range(0, 100)] public int criticalChance;
    public float statMultiplier;
    public Attribute damageEStatScale;
    public float damageRange = 0.15f;

    public void PrepareArgs(CombatArgs args) {
        int finalDamage;
        if (isPercentageDamage) {
            finalDamage = Mathf.RoundToInt(args.target.Health.Max * healthPercentage);
        } else {
            var rangedDamage = UnityEngine.Random.Range(1 - damageRange, 1 + damageRange);
            var damageStat = damageEStatScale;
            var damageStatValue = args.user[damageStat] * statMultiplier;

            finalDamage = Mathf.RoundToInt((baseDamage + damageStatValue) * rangedDamage);
        }

        args.skillElement = args.skill.element;
        args.ignoreShield = ignoreShield;
        args.damage = finalDamage;
        args.criticalChance = criticalChance;
        args.hitChance = hitChance;
    }
}