using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalEffect : ISkillEffect {
    public StatusSO status;
    [SerializeReference, TypeDropdown]
    public List<ISkillEffect> effect;

    public void PrepareArgs(CombatArgs args) {
        if (args.target.StatusEffectList.Contain(status)) {
            foreach (var item in effect) {
                item.PrepareArgs(args);
            }
        }
    }
}