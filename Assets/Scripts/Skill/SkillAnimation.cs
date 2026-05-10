using System;
using System.Collections;
using System.Collections.Generic;
using TricksAndTreatsOrThreats.Behaviour;

public interface ISkillAnimation {
    [Obsolete] public bool TrySkipSelection(Character user, Skill skill) => false;
    public bool NeedTarget { get; }
    public bool ValidateTarget(Character user, ITarget target);
    public IEnumerator Play(Skill skill, ActionArgs aArgs);
    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target);
}