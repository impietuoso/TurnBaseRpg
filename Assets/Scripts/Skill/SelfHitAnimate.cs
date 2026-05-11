using System;
using System.Collections;
using System.Collections.Generic;
using TricksAndTreatsOrThreats.Behaviour;
using UnityEngine;

[Serializable]
public class SelfHitAnimate : ISkillAnimation {
    public float damageDelay = 1;
    public GameObject castingParticle;
    public GameObject skillParticle;

    public bool NeedTarget => false;

    public bool ValidateTarget(Character user, ITarget tgt) {
        if (tgt is not Character target) return false;
        return user == target;
    }

    public IEnumerable<ITarget> GetAffectedTargets(Character user, ITarget target) {
        yield return target;
    }

    public IEnumerator Play(Skill skill, ActionArgs aArgs) {
        if (castingParticle) {
            var vfxParent = aArgs.User.SpriteRenderer.transform;
            var particle = UnityEngine.Object.Instantiate(castingParticle, vfxParent);
            yield return new WaitWhile(() => particle);
        } else
            yield return new WaitForSeconds(0.1f);

        Debug.Log(aArgs.User.Member.charName + " Defends!");
        var args = new CombatArgs();
        args.actionArgs = aArgs;
        args.skill = skill;
        args.user = aArgs.User;
        args.target = aArgs.User;
        args.source = this;

        foreach (var effect in skill.skillEffects)
            effect.Prepare(args);

        var parent = args.target.SpriteRenderer.transform;
        UnityEngine.Object.Instantiate(skillParticle, parent);
        yield return new WaitForSeconds(damageDelay);

        args.Resolve();
    }
}