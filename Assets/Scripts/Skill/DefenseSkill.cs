using System;
using UnityEngine;

[Obsolete]
[Serializable]
public class DefenseSkill : ICombatEffect {
    [Range(0f, 1f)]
    public float damageReduction;

    public bool ValidateTarget(Character user, Character target) {
        return user == target;
    }

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.element = args.skill.element;
        //TODO args.user.OnStartTurn += OnStartTurn;
        args.user.OnDefend += OnDefend;
        args.unavoidable = true;
    }

    private void OnDefend(CombatArgs args) {
        args.damage = (int)(args.damage * (1f - damageReduction));
    }
    
    private void OnStartTurn(Character target) {
        //TODO target.OnStartTurn -= OnStartTurn;
        target.OnDefend -= OnDefend;
    }
}