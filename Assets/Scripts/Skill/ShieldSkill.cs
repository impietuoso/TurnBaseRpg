using System;

[Serializable]
public class ShieldSkill : ISkillEffect {
    public int shieldValue;
    public bool usePercentageOfHealth;
    public int healthPercentage;

    public void Prepare(CombatArgs args) {
        args.shield = usePercentageOfHealth ? args.target.derivedStats.health.maxValue * healthPercentage : shieldValue;
    }
}
