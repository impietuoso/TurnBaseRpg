public class StopSkillReactionEffect : ICombatEffect {
    void ICombatEffect.PrepareEffect(CombatArgs args) {
        args.stopReactionAttacks = true;
    }
}
