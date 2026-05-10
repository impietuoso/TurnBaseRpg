using System;
using System.Collections;
using System.Collections.Generic;

public interface ISkillAnimation
{
    public bool TrySkipSelection(Character user, Skill skill);
    public bool ValidateTarget(Character user, ITarget target);
    public IEnumerator Play(Skill skill, Character user, ITarget target) => Play(skill, user, (Character)target, null);
    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target) => GetAffectedTargets(user, (Character)target);

    [Obsolete] public IEnumerable<Character> GetAffectedTargets(Character user, Character target);
    [Obsolete] public IEnumerator Play(Skill skill, Character user, Character target, CombatManager cm);
}