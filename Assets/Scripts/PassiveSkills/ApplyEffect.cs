using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve, Serializable]
public class ApplyEffect : IPassive {
    public Element element;
    [SerializeReference, TypeDropdown]
    public ICombatEffect effect;

    public void Subscribe(Character character) {
        character.OnAttack += Apply;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -= Apply;
    }

    public void Apply(CombatArgs args) {
        if (element && args.element != element) return;
        effect.PrepareArgs(args);
    }
}