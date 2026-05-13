using System;
using UnityEngine;

[Serializable]
public class ManaHealSkill : ICombatEffect {
    //valor de mana é multiplicado por variavel fixa ou dano causado
    [Range(0f, 1f)]
    public float manaHealPercentage = 0.2f;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.OnResolve += HealMana;
    }

    public void HealMana(CombatArgs args) {
        args.OnResolve -= HealMana; ////manaHeal
        var heal = args.user.Mana.Max * manaHealPercentage;
        if (heal == 0 || args.result.Miss) return;
        args.user.Mana.Current += (int)heal;
    }
}