public class StopSkillReactionEffect : ISkillEffect {
    public void PrepareArgs(CombatArgs args) {
        args.stopReactionAttacks = true;
    }
}
