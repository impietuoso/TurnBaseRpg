using System.Collections;
using System.Collections.Generic;

public interface ISkillAnimation {
    public bool skipSelection { get; }
    public bool ValidateTarget(Character user, Character target);
    public IEnumerable<Character> GetAffectedTargets(Character user, Character target);
    public IEnumerator Play(Skill skill, Character user, Character target, CombatManager cm);
}