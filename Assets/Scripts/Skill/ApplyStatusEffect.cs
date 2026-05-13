using System;
using UnityEngine;

[Serializable]
public class ApplyStatusEffect : ISkillEffect {
    public StatusSO status;
    public bool targetUser;
    public void PrepareArgs(CombatArgs args) {
        if(args.hitChance == 0) args.hitChance = 100;
        if (targetUser) {
            args.user?.StatusEffectList.Apply(status);
            Debug.Log(status.status.statusName + " was apply on " + args.user?.Member.charName + ".");
        } else args.statusEffects.Add(status);
    }
}
