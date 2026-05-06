using System;
using UnityEngine;

[Serializable]
public class ManaHealSkill : ISkillEffect {
    //valor de mana é multiplicado por variavel fixa ou dano causado
    [Range(0f, 1f)]
    public float manaHealPercentage = 0.2f;

    public void Prepare(CombatArgs args) {
        args.OnResolve += HealMana;
    }

    public void HealMana(CombatArgs args) {
        args.OnResolve -= HealMana;////manaHeal
        var heal = args.user.derivedStats.mana.maxValue * manaHealPercentage;
        if (heal == 0 || args.result.miss) return;
        args.user.derivedStats.mana.AddClampedBaseValue((int)heal);
    }
}
