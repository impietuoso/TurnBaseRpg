using UnityEngine;
public class ChangeElementalDamage : IPassiveSkill {
    public Element element;
    public bool changeAttack = true;
    [Range(0,2)]
    public float damageIncrease = 1;
    public void Subscribe(Character character) {
        if (changeAttack)
            character.OnAttack += IncreaseDamage;
        else
            character.OnDefend += IncreaseDamage;
    }

    public void Unsubscribe(Character character) {
        if (changeAttack)
            character.OnAttack -= IncreaseDamage;
        else
            character.OnDefend -= IncreaseDamage;
    }

    public void IncreaseDamage(CombatArgs args) {
        if (args.skillElement  && args.skillElement == element) {
            args.damage = (int)(args.damage * damageIncrease);
        }
    }
}