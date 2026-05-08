using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelfHitAnimate : ISkillAnimation {
    public float damageDelay = 1;
    public GameObject castingParticle;
    public GameObject skillParticle;

    public bool TrySkipSelection(Character user, Skill skill) {
        CombatManager.instance.UsingSkillOnTarget(user, skill, user);
        return true;
    }

    public bool ValidateTarget(Character user, Character target) {
        if (user == target) return true;
        else return false;
    }

    public IEnumerable<Character> GetAffectedTargets(Character user, Character target) {
        yield return target;
    }

    public IEnumerator Play(Skill skill, Character user, Character target, CombatManager cm) {
        var ui = CombatManager.instance.combatUI;
        if (castingParticle) {
            var particle = UnityEngine.Object.Instantiate(
                castingParticle,
                ui.GetCharacterWorldPosition(user),
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

        UnityEngine.Object.Instantiate(skillParticle, ui.GetCharacterWorldPosition(args.target), Quaternion.identity);
        yield return new WaitForSeconds(damageDelay);

        args.Resolve();
    }
}
