using System;

[Serializable]
public class ShieldSkill : ISkillEffect {
    public int shieldValue;
    public bool usePercentageOfHealth;
    public int healthPercentage;

    public void PrepareArgs(CombatArgs args) {
        args.shield = usePercentageOfHealth ? args.target.Health.Max * healthPercentage : shieldValue;
    }
}
