using UnityEngine;
public class ChangeElementalDamage : IPassiveSkill {
    public Element element;
    [Range(0,2)]
    public float damageIncrease = 1;
    public void Subscribe(Character character) {
        character.OnAttack += IncreaseDamage;
    }

    public void Unsubscribe(Character character) {
        character.OnAttack -= IncreaseDamage;
    }

    public void IncreaseDamage(CombatArgs args) {
        if (args.skillElement  && args.skillElement == element) {
            args.damage = (int)(args.damage * damageIncrease);
        }
    }
}