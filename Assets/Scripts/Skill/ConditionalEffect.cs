using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConditionalEffect : ICombatEffect {
    public StatusSO status;
    [SerializeReference, TypeDropdown]
    public List<ICombatEffect> effect;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        if (!args.target.StatusEffectList.Contain(status)) return;
        foreach (var item in effect) item.PrepareArgs(args);
    }
}