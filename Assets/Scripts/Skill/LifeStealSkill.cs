using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
public class LifeStealSkill : ISkillEffect {
    [Range(0f, 1f)]
    public float damagePercentage = 0.2f;

    public void Prepare(CombatArgs args) {
        args.OnResolve += Steal;
    }

    public void Steal(CombatArgs args) {
        args.OnResolve -= Steal;
        var stealHeal = args.result.deltaHp * damagePercentage;
        if (stealHeal == 0) return;
        CombatArgs newArgs = new CombatArgs();
        newArgs.heal = (int)stealHeal;
        newArgs.user = args.user;
        newArgs.target = args.user;
        newArgs.source = this;
        newArgs.Resolve();
    }
}