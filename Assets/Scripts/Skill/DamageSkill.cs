using System;
using UnityEngine;

[Obsolete, Serializable]
public class DamageSkill : ICombatEffect {
    public Element element;
    public int baseDamage;
    public bool ignoreShield;
    public bool isPercentageDamage;
    [Range(0f, 1f)] public float healthPercentage = 0.2f;
    [Range(0, 100)] public int hitChance;
    [Range(0, 100)] public int criticalChance;
    public float statMultiplier;
    public Attribute damageStatScale;
    public float damageRange = 0.15f;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        throw new NotImplementedException();
    }
}