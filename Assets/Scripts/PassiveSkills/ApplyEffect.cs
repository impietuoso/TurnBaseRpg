using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve, Serializable]
public class ApplyEffect : IPassive {
    public Element element;
    [SerializeReference, TypeDropdown]
    public ISkillEffect effect;
    public void Subscribe(Character character) {
        character.OnAttack += ApplyEffect;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -=ApplyEffect;
    }

    public void ApplyEffect(CombatArgs args) {
        if (element && args.skillElement != element) return;
        effect.PrepareArgs(args);
    }
}