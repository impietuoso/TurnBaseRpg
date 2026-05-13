using System;

[Serializable]
public class ShieldSkill : ICombatEffect {
    public int shieldValue;
    public bool usePercentageOfHealth;
    public int healthPercentage;

    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.shield = usePercentageOfHealth ? args.target.Health.Max * healthPercentage : shieldValue;
    }
}
