using System;
using UnityEngine;

[Serializable]
public class ManaDrainSkill : ISkillEffect {
    //valor de mana é multiplicado por variavel fixa ou dano causado
    [Range(0f, 1f)]
    public float damagePercentage = 0.2f;

    public void Prepare(CombatArgs args) {
        args.OnResolve += Steal;
    }

    public void Steal(CombatArgs args) {
        args.OnResolve -= Steal;
        var stealMana = args.result.deltaMp * damagePercentage;
        if (stealMana == 0) return;
        CombatArgs newArgs = new CombatArgs();
        newArgs.actionArgs = args.actionArgs;
        newArgs.skill = args.skill;
        newArgs.mana = -(int)stealMana;
        newArgs.user = args.user;
        newArgs.target = args.user;
        newArgs.source = this;
        newArgs.Resolve();
    }
}