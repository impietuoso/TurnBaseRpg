using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ApplyStatusEffect : ISkillEffect {
    public StatusSO status;
    public bool targetUser;
    public void Prepare(CombatArgs args) {
        if(args.hitChance == 0) args.hitChance = 100;
        if (targetUser) {
            args.user?.StatusEffectList.Apply(status);
            Debug.Log(status.status.statusName + " was apply on " + args.user?.characterName + ".");
        } else args.statusEffects.Add(status);
    }
}

public class ApplyStatusEffectEvent : ICombatPhase {
    public Character target;
    public StatusSO status;

    public ApplyStatusEffectEvent(Character target, StatusSO stats) {
        this.target = target;
        this.status = stats;
    }

    public IEnumerator Execute(CombatManager cm) {
        yield return null;
        Debug.Log(status.status.statusName + " was apply to " + target.characterName);
        target?.StatusEffectList.Apply(status);
    }
}