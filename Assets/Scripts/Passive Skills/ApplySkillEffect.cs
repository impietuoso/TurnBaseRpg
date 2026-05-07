using UnityEngine;

public class ApplySkillEffect : IPassiveSkill {
    public Element element;
    [SerializeReference, Effect]
    public ISkillEffect effect;
    public void Subscribe(Character character) {
        character.OnAttack += ApplyEffect;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -=ApplyEffect;
    }

    public void ApplyEffect(CombatArgs args) {
        if (element && args.skillElement != element) return;
        effect.Prepare(args);
    }
}