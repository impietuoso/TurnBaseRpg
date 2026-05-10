using System;
using System.Collections;
using System.Collections.Generic;
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

    public IEnumerator Play(Skill skill, Character user, ITarget target) {
        if (castingParticle) {
            var particle = UnityEngine.Object.Instantiate(
                castingParticle,
                user.Position,
                Quaternion.identity);
            yield return new WaitWhile(() => particle);
        } else {
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(user.characterName + " Defends!");
        CombatArgs args = new CombatArgs();
        args.skill = skill;
        args.user = user;
        args.target = user;
        args.source = this;

        foreach (var effect in skill.skillEffects) {
            effect.Prepare(args);
        }

        UnityEngine.Object.Instantiate(skillParticle, args.target.Position, Quaternion.identity);
        yield return new WaitForSeconds(damageDelay);

        args.Resolve();
    }
}
