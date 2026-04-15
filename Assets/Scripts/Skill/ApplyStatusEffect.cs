using System;
using UnityEngine;

[Serializable]
public class ApplyStatusEffect : ISkillEffect {
    public StatusSO status;
    public bool targetUser;

    public void Prepare(CombatArgs args) {
        if(targetUser) args.user?.StatusEffectList.Apply(status);
        else args.statusEffects.Add(status);
        Debug.Log(status.status + " was apply on " + (targetUser ? args.user.characterName : args.target.characterName) + ".");
    }
}