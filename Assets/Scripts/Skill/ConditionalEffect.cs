using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalEffect : ISkillEffect {
    public StatusSO status;
    [SerializeReference, TypeDropdown(typeof(ISkillEffect))]
    public List<ISkillEffect> effect;

    public void Prepare(CombatArgs args) {
        if (args.target.StatusEffectList.Contain(status)) {
            foreach (var item in effect) {
                item.Prepare(args);
            }
        }
    }
}