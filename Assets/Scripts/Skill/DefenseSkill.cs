using System;
using UnityEngine;

[Serializable]
public class DefenseSkill : ISkillEffect {
    [Range(0f, 1f)]
    public float damageReduction;

    public bool ValidateTarget(Character user, Character target) {
        if (user == target) {
            return true;
        } else return false;
    }

    public void Prepare(CombatArgs args) {
        args.skillElement = args.skill.element;
        args.user.OnStartTurn += OnStartTurn;
        args.user.OnDefend += OnDefend;
        args.unavoidable = true;
    }

    private void OnDefend(CombatArgs args) {
        args.damage = (int)(args.damage * (1f - damageReduction));
    }
    
    private void OnStartTurn(Character target) {
        target.OnStartTurn -= OnStartTurn;
        target.OnDefend -= OnDefend;
    }
}