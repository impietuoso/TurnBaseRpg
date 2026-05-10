using System;
using System.Collections;
using System.Collections.Generic;

public interface ISkillAnimation {
    [Obsolete] public bool TrySkipSelection(Character user, Skill skill) {
        return false;
    }
    public bool ValidateTarget(Character user, ITarget target);
    public IEnumerator Play(Skill skill, Character user, ITarget target);
    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target);
}