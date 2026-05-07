using System;

public class StopSkillReactionEffect : ISkillEffect {
    public void Prepare(CombatArgs args) {
        args.stopReactionAttacks = true;
    }
}
