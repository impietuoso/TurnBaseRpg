using System;
using UnityEngine;

[Serializable]
public class ManaBurnSkill : ISkillEffect {
    public bool useDamageDealt;
    public int manaBurn;
    [Range(0f, 1f)]
    public float damagePercentage;

    public void Prepare(CombatArgs args) {
        if(useDamageDealt) args.OnResolve += GetDamage;
            else args.mana = -manaBurn;
    }

    public void GetDamage(CombatArgs args) {
        var damage = args.result.deltaHp * damagePercentage;
        args.mana = -(int)damage;
    }
}